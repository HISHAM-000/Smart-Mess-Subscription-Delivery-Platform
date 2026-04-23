using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Features.Subscriptions.Commands;
using MessMate.Application.Interfaces.Services;
using MessMate.Application.Services;
using MessMate.Domain.Entities;
using MessMate.Domain.Enums;
using MessMate.Domain.Interfaces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Subscriptions.Handlers
{
    public class EnrollSubscriptionCommandHandler
        : IRequestHandler<EnrollSubscriptionCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly OrderGenerationService _orderService;
        public EnrollSubscriptionCommandHandler(
            IUnitOfWork unitOfWork, ICurrentUserService currentUser,
            OrderGenerationService orderService)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _orderService = orderService;
        }

        public async Task<int> Handle(
        EnrollSubscriptionCommand request,
        CancellationToken cancellationToken)
        {
            
            var plan = await _unitOfWork.SubscriptionPlans
                .GetByIdAsync(request.PlanId);

            if (plan == null || plan.MessId != request.MessId)
                throw new NotFoundException("Plan not found");

            var mess = await _unitOfWork.Messes
                .GetByIdAsync(request.MessId)
                ?? throw new NotFoundException("Mess not found");

            if (!mess.IsApproved)
                throw new UnauthorizedException("Mess not approved");

           
            var hasActive = await _unitOfWork.CustomerSubscriptions
                .HasAnyActiveSubscriptionAsync(_currentUser.UserId);

            if (hasActive)
                throw new AlreadyExistsException("Already subscribed");

            var today = DateTime.UtcNow.Date;
            var now = DateTime.UtcNow;

            
            var subscription = new CustomerSubscription
            {
                CustomerId = _currentUser.UserId,
                PlanId = request.PlanId,
                MessId = request.MessId,
                StartDate = today,
                EndDate = today.AddDays(plan.DurationDays),
                Status = SubscriptionStatus.Active,
                PaymentMethod = request.PaymentMethod,
                PaymentStatus = request.PaymentMethod == PaymentMethod.Cash
                    ? PaymentStatus.Paid
                    : PaymentStatus.Pending,
                DeliveryAddress = request.DeliveryAddress,
                DeliveryLat = request.DeliveryLat,
                DeliveryLng = request.DeliveryLng,
                CreatedBy = _currentUser.UserId,
            };

          
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                await _unitOfWork.CustomerSubscriptions.AddAsync(subscription);
                await _unitOfWork.SaveChangesAsync();

                subscription.Plan = plan;

                await _orderService.GenerateOrdersForSubscriptionAsync(
                    subscription, today, now, cancellationToken);

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();

                return subscription.Id;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        
    }
    }
}
