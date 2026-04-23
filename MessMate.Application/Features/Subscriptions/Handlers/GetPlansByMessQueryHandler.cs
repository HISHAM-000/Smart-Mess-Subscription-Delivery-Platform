using MediatR;
using MessMate.Application.Features.Subscriptions.DTOs;
using MessMate.Application.Features.Subscriptions.Queries;
using MessMate.Domain.Interfaces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Subscriptions.Handlers
{
    public class GetPlansByMessQueryHandler
        : IRequestHandler<GetPlansByMessQuery, List<SubscriptionPlanDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPlansByMessQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SubscriptionPlanDto>> Handle(
        GetPlansByMessQuery request, CancellationToken cancellationToken)
        {
            var plans = await _unitOfWork.SubscriptionPlans
            .GetByMessIdAsync(request.MessId, activeOnly: true);

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
