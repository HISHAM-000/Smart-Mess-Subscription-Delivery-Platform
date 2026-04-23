using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Features.Subscriptions.Commands;
using MessMate.Application.Interfaces.Services;
using MessMate.Domain.Entities;
using MessMate.Domain.Interfaces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Subscriptions.Handlers
{
    public class CreateSubscriptionPlanCommandHandler
        : IRequestHandler<CreateSubscriptionPlanCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public CreateSubscriptionPlanCommandHandler(
        IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<int> Handle(
        CreateSubscriptionPlanCommand request, CancellationToken cancellationToken)
        {
            var mess = await _unitOfWork.Messes.GetByOwnerIdAsync(_currentUser.UserId);
            if (mess == null)
                throw new NotFoundException("Mess not found");

            if (!mess.IsApproved)
                throw new ForbiddenException("Your mess is not approved yet.");
            int durationDays = request.PlanType == Domain.Enums.PlanType.Weekly ? 7 : 30;

            var exists = await _unitOfWork.SubscriptionPlans
                .ExistsExactPlanAsync(
                    mess.Id,
                    request.Name.Trim().ToLower(),
                    request.PlanType,
                    request.Price,
                    durationDays,
                    request.MinActiveDays,
                    request.IsBreakfast,
                    request.IsLunch,
                    request.IsDinner
                );

            if (exists)
                throw new AlreadyExistsException("Plan already exists.");

            if (!request.IsBreakfast && !request.IsLunch && !request.IsDinner)
                throw new BadRequestException("At least one meal must be selected.");

            if (request.MinActiveDays > durationDays)
                throw new BadRequestException(
                    $"MinActiveDays cannot exceed plan duration of {durationDays} days.");

            var plan = new SubscriptionPlan
            {
                MessId = mess.Id,
                Name = request.Name,
                PlanType = request.PlanType,
                Price = request.Price,
                DurationDays = durationDays,
                MinActiveDays = request.MinActiveDays,
                IsBreakfast = request.IsBreakfast,
                IsLunch = request.IsLunch,
                IsDinner = request.IsDinner,
                IsActive = true,
                CreatedBy = _currentUser.UserId,
            };
            await _unitOfWork.SubscriptionPlans.AddAsync(plan);
            await _unitOfWork.SaveChangesAsync();
            return plan.Id;
        }
    }
}
