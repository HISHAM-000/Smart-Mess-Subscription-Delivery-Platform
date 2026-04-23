using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Features.Subscriptions.Commands;
using MessMate.Application.Interfaces.Services;
using MessMate.Domain.Enums;
using MessMate.Domain.Interfaces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Subscriptions.Handlers
{
    public class PauseSubscriptionCommandHandler
    : IRequestHandler<PauseSubscriptionCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public PauseSubscriptionCommandHandler(
            IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(
        PauseSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var subscription = await _unitOfWork.CustomerSubscriptions
            .GetByIdAsync(request.SubscriptionId);
            if (subscription == null)
                throw new NotFoundException("Subscription not found");

            if (subscription.CustomerId != _currentUser.UserId)
                throw new ForbiddenException("You cannot pause this subscription.");

            if (subscription.Status != SubscriptionStatus.Active)
                throw new BadRequestException("Only active subscriptions can be paused.");

            if (request.PauseUntil <= request.PauseFrom)
                throw new BadRequestException("PauseUntil must be after PauseFrom");

            if (request.PauseFrom < today)
                throw new BadRequestException("PauseFrom cannot be in the past");

            if (request.PauseFrom == today)
                throw new BadRequestException("Cannot pause starting today");

            int pauseDays = request.PauseUntil.DayNumber - request.PauseFrom.DayNumber;

            if (pauseDays <= 0)
                throw new BadRequestException("Invalid pause duration");

            if (pauseDays > 30)
                throw new BadRequestException("Pause duration cannot exceed 30 days");

            if (subscription.PausedUntil != null)
            {
                var existingPauseUntil = DateOnly.FromDateTime(subscription.PausedUntil.Value);

                if (request.PauseFrom <= existingPauseUntil)
                    throw new BadRequestException("Pause period overlaps with existing pause.");
            }

            subscription.Status = SubscriptionStatus.Paused;
            subscription.PausedFrom = request.PauseFrom.ToDateTime(TimeOnly.MinValue);
            subscription.PausedUntil = request.PauseUntil.ToDateTime(TimeOnly.MinValue);
            subscription.PausedDays += pauseDays;
            subscription.EndDate = subscription.EndDate.AddDays(pauseDays); 
            subscription.UpdatedAt = DateTime.UtcNow;


            await _unitOfWork.CustomerSubscriptions.UpdateAsync(subscription);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
