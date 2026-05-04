using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Features.Subscriptions.DTOs;
using MessMate.Application.Features.Subscriptions.Queries;
using MessMate.Application.Interfaces.Services;
using MessMate.Domain.Entities;
using MessMate.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Subscriptions.Handlers
{

    public class GetPlanByIdHandler : IRequestHandler<GetPlanByIdQuery, SubscriptionPlanDto>
    {
        private readonly IGenericRepository<SubscriptionPlan> _planRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IGenericRepository<MessMate.Domain.Entities.Mess> _messRepository;

        public GetPlanByIdHandler(
            IGenericRepository<SubscriptionPlan> planRepository,
            IGenericRepository<MessMate.Domain.Entities.Mess> messRepository,
            ICurrentUserService currentUser)
        {
            _planRepository = planRepository;
            _currentUser = currentUser;
            _messRepository = messRepository;
        }

        public async Task<SubscriptionPlanDto> Handle(GetPlanByIdQuery request, CancellationToken ct)
        {
            var userId = _currentUser.UserId;

            var mess = await _messRepository.GetAsync(m => m.OwnerId == userId);

            if (mess == null)
                throw new NotFoundException("Mess not found");

            var plan = await _planRepository.GetAsync(p =>
                p.Id == request.Id && p.MessId == mess.Id);

            if (plan == null)
                throw new NotFoundException("Plan not found");

            return new SubscriptionPlanDto
            {
                Id = plan.Id,
                Name = plan.Name,

                PlanType = plan.PlanType.ToString(),

                Price = plan.Price,
                DurationDays = plan.DurationDays,
                MinActiveDays = plan.MinActiveDays,

                IsBreakfast = plan.IsBreakfast,
                IsLunch = plan.IsLunch,
                IsDinner = plan.IsDinner
            };
        }
    }

}
