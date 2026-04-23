using MessMate.Domain.Entities;
using MessMate.Domain.Enums;
using MessMate.Domain.Interfaces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Services
{
    public class OrderGenerationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderGenerationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> GenerateOrdersForTodayAsync(
          DateTime today,
          DateTime now,
          CancellationToken ct)
        {
            var subscriptions = await _unitOfWork.CustomerSubscriptions
                .GetActiveSubscriptionsForDateAsync(today);

            Console.WriteLine($"Subscriptions Count: {subscriptions.Count}");

            foreach (var sub in subscriptions)
            {
                Console.WriteLine($"SubId: {sub.Id}, User: {sub.CustomerId}");
            }

            int ordersCreated = 0;

            foreach (var subscription in subscriptions)
            {
                ordersCreated += await GenerateForSingleSubscription(
                    subscription, today, now, ct);
            }

            return ordersCreated;
        }

        public async Task GenerateOrdersForSubscriptionAsync(
           CustomerSubscription subscription,
           DateTime today,
           DateTime now,
           CancellationToken ct)
        {
            var nows = DateTime.Now;
            var todayy = now.Date;
            await GenerateForSingleSubscription(subscription, todayy, nows, ct);
        }

        private async Task<int> GenerateForSingleSubscription(
       CustomerSubscription subscription,
       DateTime today,
       DateTime now,
       CancellationToken ct)
        {
            var plan = subscription.Plan;

            var slots = GetEnabledSlots(plan);

            int mealsPerDay = slots.Count;
            if (mealsPerDay == 0) return 0;

            int totalMeals = plan.DurationDays * mealsPerDay;
            if (totalMeals == 0) return 0;

            decimal amountPerMeal = plan.Price / totalMeals;

            var menu = await _unitOfWork.Menus
                .GetByMessAndDayAsync(subscription.MessId, today.DayOfWeek);

            if (menu == null)
            {
                return 0;
            }

            var menuItems = await _unitOfWork.MenuItems
                .GetByMenuIdAsync(menu.Id);

            var slotCutoffs = new Dictionary<MealSlot, TimeSpan>
        {
            { MealSlot.Breakfast, new TimeSpan(7, 0, 0) },
            { MealSlot.Lunch,     new TimeSpan(11, 0, 0) },
            { MealSlot.Dinner,    new TimeSpan(18, 0, 0) }
        };

            int count = 0;

            foreach (var slot in slots)
            {
                var cutoff = today + slotCutoffs[slot];

                if (now >= cutoff)
                    continue;

                var exists = await _unitOfWork.Orders
                    .ExistsForDateAndSlotAsync(subscription.Id, today, slot);

                if (exists)
                    continue;

                var menuItem = menuItems.FirstOrDefault(x =>
                    x.MealSlot == slot &&
                    x.IsAvailable &&
                    x.IsDeleted != true);

                if (menuItem == null)
                {
                    continue;
                }

                await _unitOfWork.Orders.AddAsync(new Order
                {
                    SubscriptionId = subscription.Id,
                    CustomerId = subscription.CustomerId,
                    MessId = subscription.MessId,
                    OrderDate = today,
                    MealSlot = slot,
                    MenuItemId = menuItem.Id,
                    Status = OrderStatus.Pending,
                    Amount = amountPerMeal,
                    CreatedBy = subscription.CustomerId,
                });

                count++;
            }

            return count;
        }

        private static List<MealSlot> GetEnabledSlots(SubscriptionPlan plan)
        {
            var slots = new List<MealSlot>();

            if (plan.IsBreakfast) slots.Add(MealSlot.Breakfast);
            if (plan.IsLunch) slots.Add(MealSlot.Lunch);
            if (plan.IsDinner) slots.Add(MealSlot.Dinner);

            return slots;
        }
    }
}
