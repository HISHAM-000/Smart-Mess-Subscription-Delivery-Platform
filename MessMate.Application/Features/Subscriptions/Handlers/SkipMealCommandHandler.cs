using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Features.Subscriptions.Commands;
using MessMate.Application.Interfaces.Services;
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
    public class SkipMealCommandHandler : IRequestHandler<SkipMealCommand, SkipMealResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public SkipMealCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<SkipMealResult> Handle(
        SkipMealCommand request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.Orders.GetByIdWithDetailsAsync(request.OrderId);
            if (order == null)
                throw new NotFoundException("Order not found");

            if (order.CustomerId != _currentUser.UserId)
                throw new ForbiddenException("You cannot skip this order.");

            if (order.Status != OrderStatus.Pending)
                throw new BadRequestException($"Order is already {order.Status}.");

            var cutoff = order.OrderDate.Date.AddHours(8);
            if (DateTime.UtcNow >= cutoff)
                throw new BadRequestException("Cutoff time has passed. You cannot skip this meal");

            var subscription = order.Subscription;
            var plan = order.Subscription.Plan;

            int totalMeals = plan.DurationDays * MealsPerDay(plan);
            int maxSkippable = totalMeals - (plan.MinActiveDays * MealsPerDay(plan));

            if (subscription.SkippedMeals >= maxSkippable)
                throw new BadRequestException("You have reached the maximum skippable meals for this plan.");

            decimal refundAmount = plan.Price / totalMeals;

            order.Status = OrderStatus.Skipped;
            order.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.Orders.UpdateAsync(order);

            subscription.SkippedMeals++;
            subscription.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.CustomerSubscriptions.UpdateAsync(subscription);

            var skip = new MealSkip
            {
                OrderId = order.Id,
                SubscriptionId = subscription.Id,
                CustomerId = _currentUser.UserId,
                MealDate = order.OrderDate,
                MealSlot = order.MealSlot.ToString(),
                RefundAmount = refundAmount,
                RefundStatus = RefundStatus.Pending,
                CreatedBy = _currentUser.UserId,
            };

            await _unitOfWork.MealSkips.AddAsync(skip);
            await _unitOfWork.SaveChangesAsync();

            return new SkipMealResult(
                skip.Id,
                refundAmount,
                $"Meal skipped. Refund of ₹{refundAmount:F2} is pending.");
        }

        private static int MealsPerDay(SubscriptionPlan plan)
        => (plan.IsBreakfast ? 1 : 0)
         + (plan.IsLunch ? 1 : 0)
         + (plan.IsDinner ? 1 : 0);
    }

}
