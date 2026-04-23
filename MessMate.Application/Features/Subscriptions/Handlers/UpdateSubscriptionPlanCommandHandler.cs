using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Features.Subscriptions.Commands;
using MessMate.Application.Interfaces.Services;
using MessMate.Domain.Interfaces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Subscriptions.Handlers
{
    public class UpdateSubscriptionPlanCommandHandler 
        : IRequestHandler<UpdateSubscriptionPlanCommand,bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        public UpdateSubscriptionPlanCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(
            UpdateSubscriptionPlanCommand request,
            CancellationToken cancellationToken)
        {
            var mess = await _unitOfWork.Messes
               .GetByOwnerIdAsync(_currentUser.UserId);

            if (mess == null)
                throw new NotFoundException("Mess not found");

            var plan = await _unitOfWork.SubscriptionPlans
             .GetByIdAsync(request.PlanId);

            if (plan == null)
                throw new NotFoundException("Plan not found");

            if (plan.MessId != mess.Id)
                throw new ForbiddenException("You cannot update this plan.");

            if (!request.IsBreakfast && !request.IsLunch && !request.IsDinner)
                throw new BadRequestException("At least one meal must be selected.");

            var hasActiveSubscriptions = await _unitOfWork.CustomerSubscriptions
                .HasActiveSubscriptionsForPlanAsync(plan.Id);

            plan.Name = request.Name;

            if (hasActiveSubscriptions)
            {
                if (plan.Price != request.Price ||
                    plan.PlanType != request.PlanType ||
                    plan.MinActiveDays != request.MinActiveDays ||
                    plan.IsBreakfast != request.IsBreakfast ||
                    plan.IsLunch != request.IsLunch ||
                    plan.IsDinner != request.IsDinner)
                {
                    throw new BadRequestException(
                        "Cannot modify plan details while active subscriptions exist. Only name can be updated.");
                }
            }
            else
            {
                plan.Price = request.Price;
                plan.MinActiveDays = request.MinActiveDays;
                plan.IsBreakfast = request.IsBreakfast;
                plan.IsLunch = request.IsLunch;
                plan.IsDinner = request.IsDinner;
            }

            await _unitOfWork.SubscriptionPlans.UpdateAsync(plan);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
