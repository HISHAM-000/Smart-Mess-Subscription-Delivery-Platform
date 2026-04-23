using MediatR;
using MessMate.Application.Features.Subscriptions.DTOs;
using MessMate.Application.Features.Subscriptions.Queries;
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
    public class GetMySubscriptionsQueryHandler 
        : IRequestHandler<GetMySubscriptionsQuery, List<MySubscriptionDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public GetMySubscriptionsQueryHandler(
            IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<List<MySubscriptionDto>> Handle(
        GetMySubscriptionsQuery request, CancellationToken cancellationToken)
        {
            var subscriptions = await _unitOfWork.CustomerSubscriptions
            .GetByCustomerIdAsync(_currentUser.UserId);

            return subscriptions.Select(s => new MySubscriptionDto
            {
                Id = s.Id,
                PlanName = s.Plan.Name,
                MessName = s.Mess.Name,
                PlanType = s.Plan.PlanType.ToString(),
                Price = s.Plan.Price,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                Status = s.Status.ToString(),
                PaymentStatus = s.PaymentStatus.ToString(),
                SkippedMeals = s.SkippedMeals,
                PausedDays = s.PausedDays,
                IsBreakfast = s.Plan.IsBreakfast,
                IsLunch = s.Plan.IsLunch,
                IsDinner = s.Plan.IsDinner,
                IsPaused = s.Status == SubscriptionStatus.Paused,
                PausedFrom = s.PausedFrom.HasValue
                     ? DateOnly.FromDateTime(s.PausedFrom.Value) : null,
                PausedUntil = s.PausedUntil.HasValue
                     ? DateOnly.FromDateTime(s.PausedUntil.Value) : null,
            }).ToList();
        }
    }
}
