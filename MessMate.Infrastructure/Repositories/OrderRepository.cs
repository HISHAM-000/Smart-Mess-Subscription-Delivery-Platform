using MessMate.Domain.Entities;
using MessMate.Domain.Enums;
using MessMate.Domain.Interfaces.Repositories;
using MessMate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Infrastructure.Repositories
{
    public class OrderRepository : GenericRepository<Order>,IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context) { }

        public async Task<Order?> GetByIdWithDetailsAsync(int orderId)
        {
            var result =await _context.Orders
                .Include(c => c.Subscription)
                .ThenInclude(c => c.Plan)
                .Include(c => c.Mess)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.IsDeleted != true);

            return result;
        }

        public async Task<List<Order>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.Orders
                .Include(o => o.MenuItem) 
                .Include(o => o.Mess)
                .Where(o => o.CustomerId == customerId && o.IsDeleted != true)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<bool> ExistsForDateAndSlotAsync(
            int subscriptionId,
            DateTime date,
            MealSlot slot)
        {
            var start = date.Date;
            var end = start.AddDays(1);

            return await _context.Orders
                .AsNoTracking()
                .AnyAsync(o =>
                    o.SubscriptionId == subscriptionId &&
                    o.OrderDate >= start &&
                    o.OrderDate < end &&
                    o.MealSlot == slot &&
                    o.IsDeleted != true);
        }


        public async Task<List<Order>> GetByMessIdAndDateAsync(
            int messId,
            DateTime date)
        {
            var start = date.Date;
            var end = start.AddDays(1);

            return await _context.Orders
                .Include(o => o.MenuItem)
                .Include(o => o.Customer)
                .Include(o => o.Subscription)
                .Where(o =>
                    o.MessId == messId &&
                    o.OrderDate >= start &&
                    o.OrderDate < end &&
                    o.IsDeleted != true)
                .OrderBy(o => o.MealSlot)
                .ToListAsync();
        }

        public async Task<List<Order>> GetBySubscriptionAndDateRangeAsync(
            int subscriptionId,
            DateOnly from,
            DateOnly to)
        {
            var start = from.ToDateTime(TimeOnly.MinValue);
            var end = to.ToDateTime(TimeOnly.MinValue).AddDays(1);

            return await _context.Orders
                .Where(o =>
                    o.SubscriptionId == subscriptionId &&
                    o.OrderDate >= start &&
                    o.OrderDate < end &&
                    o.IsDeleted != true)
                .ToListAsync();
        }
    }
}
