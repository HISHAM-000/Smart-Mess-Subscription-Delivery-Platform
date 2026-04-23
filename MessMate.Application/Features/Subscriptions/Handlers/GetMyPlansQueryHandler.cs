using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Features.Subscriptions.DTOs;
using MessMate.Application.Features.Subscriptions.Queries;
using MessMate.Application.Interfaces.Services;
using MessMate.Domain.Interfaces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Subscriptions.Handlers
{
    public class GetMyPlansQueryHandler
    : IRequestHandler<GetMyPlansQuery, List<SubscriptionPlanDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public GetMyPlansQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<List<SubscriptionPlanDto>> Handle(
            GetMyPlansQuery request,
            CancellationToken cancellationToken)
        {

            var mess = await _unitOfWork.Messes
                .GetByOwnerIdAsync(_currentUser.UserId)
                ?? throw new NotFoundException("No mess found for this owner.");

            var plans = await _unitOfWork.SubscriptionPlans
                .GetByMessIdAsync(mess.Id, activeOnly: false);

            return plans.Select(p => new SubscriptionPlanDto
            {
                Id = p.Id,
                Name = p.Name,
                PlanType = p.PlanType.ToString(),
                Price = p.Price,
                DurationDays = p.DurationDays,
                MinActiveDays = p.MinActiveDays,
                IsBreakfast = p.IsBreakfast,
                IsLunch = p.IsLunch,
                IsDinner = p.IsDinner,
            }).ToList();
        }
    }
}
