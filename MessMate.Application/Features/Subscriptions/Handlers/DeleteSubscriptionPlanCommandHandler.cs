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
    public class DeleteSubscriptionPlanCommandHandler
        : IRequestHandler<DeleteSubscriptionPlanCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public DeleteSubscriptionPlanCommandHandler(
            IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(
        DeleteSubscriptionPlanCommand request, CancellationToken cancellationToken)
        {
            var mess = await _unitOfWork.Messes.GetByOwnerIdAsync(_currentUser.UserId);

            if (mess == null)
                throw new NotFoundException("Mess not found");

            var plan = await _unitOfWork.SubscriptionPlans.GetByIdAsync(request.PlanId);

            if (plan == null)
                throw new NotFoundException("Plan not found");

            var exists = await _unitOfWork.SubscriptionPlans
                .CheckAnyActiveUsersAsync(request.PlanId);

            if (exists)
                throw new BadRequestException("Cannot delete plan with active subscriptions.");

            if (!plan.IsActive)
                throw new BadRequestException("Plan is already deleted");

            if (plan.MessId != mess.Id)
                throw new ForbiddenException("You cannot delete this plan.");

            plan.IsActive = false;
            plan.IsDeleted = true;
            plan.DeletedOn = DateTime.UtcNow;
            plan.DeletedBy = _currentUser.UserId;

            await _unitOfWork.SubscriptionPlans.UpdateAsync(plan);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
