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
    public class CustomerSubscriptionRepository : GenericRepository<CustomerSubscription>, 
        ICustomerSubscriptionRepository
    {
        public CustomerSubscriptionRepository(AppDbContext context) : base(context) { }

        public async Task<List<CustomerSubscription>> GetByCustomerIdAsync(int customerId)
        {
            var result = await _context.CustomerSubscriptions
                .Include(c => c.Plan)
                .Include(c => c.Mess)
                .Where(s => s.CustomerId == customerId && s.IsDeleted != true)
                .OrderByDescending(c => c.CreatedOn)
                .ToListAsync();

            return result;
        }

        public async Task<CustomerSubscription?> GetActiveByCustomerAndMessAsync(
            int customerId, int messId)
        {
            var result =await _context.CustomerSubscriptions
                .Include(c => c.Plan)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId
                && c.MessId == messId
                && c.Status == SubscriptionStatus.Active
                && c.IsDeleted != true);

            return result;
                
        }

        public async Task<bool> HasAnyActiveSubscriptionAsync(int customerId)
        {
            var today = DateTime.UtcNow.Date;

            return await _context.CustomerSubscriptions
                .AnyAsync(s =>
                    s.CustomerId == customerId &&
                    s.IsDeleted != true &&
                    s.Status == SubscriptionStatus.Active &&
                    s.PaymentStatus == PaymentStatus.Paid &&
                    s.StartDate.Date <= today &&
                    s.EndDate.Date >= today &&
                   (
                    (s.PausedFrom == null && s.PausedUntil == null)
                    ||
                    !(
                        s.PausedFrom.HasValue &&
                        s.PausedUntil.HasValue &&
                        s.PausedFrom.Value.Date <= today &&
                        s.PausedUntil.Value.Date >= today
                    )
                )
                );
        }

        public async Task<List<CustomerSubscription>> GetActiveSubscriptionsForDateAsync(
            DateTime date)
        {
            var result = await _context.CustomerSubscriptions
                .Include(c => c.Plan)
                    .Where(s =>
                        s.Status == SubscriptionStatus.Active &&
                        s.PaymentStatus == PaymentStatus.Paid &&
                        s.StartDate.Date <= date.Date &&
                        s.EndDate.Date >= date.Date &&
                        s.IsDeleted != true &&
                        (
                        (s.PausedFrom == null && s.PausedUntil == null)
                        ||
                        !(
                            s.PausedFrom.HasValue &&
                            s.PausedUntil.HasValue &&
                            s.PausedFrom.Value.Date <= date.Date &&
                            s.PausedUntil.Value.Date >= date.Date
                          )
                       )
                     )
                .ToListAsync();

            return result;
        }

        public async Task<List<Order>> GetBySubscriptionIdAsync(int subscriptionId)
        {
            var result = await _context.Orders
                .Where(o => o.SubscriptionId == subscriptionId && o.IsDeleted != true)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
            return result;
        }

        public async Task<bool> HasActiveSubscriptionsByMessIdAsync(int messId)
        {
            var today = DateTime.UtcNow.Date;

            return await _context.CustomerSubscriptions
                .AnyAsync(s =>
                    s.MessId == messId &&
                    s.IsDeleted != true &&
                    (s.Status == SubscriptionStatus.Active ||
                     s.Status == SubscriptionStatus.Paused) &&
                    s.EndDate.Date >= today
                );
        }

        public async Task<List<CustomerSubscription>> GetSubscriptionsToResumeAsync(
            DateTime today)
            => await _context.CustomerSubscriptions
                .Where(s =>
                    s.Status == SubscriptionStatus.Paused &&
                    s.PausedUntil.HasValue &&
                    s.PausedUntil.Value.Date < today &&
                    s.IsDeleted != true)
                .ToListAsync();

        public async Task<List<CustomerSubscription>> GetSubscriptionsToExpireAsync(
            DateTime today)
            => await _context.CustomerSubscriptions
                .Where(s =>
                    s.Status == SubscriptionStatus.Active &&
                    s.EndDate.Date < today &&
                    s.IsDeleted != true)
                .ToListAsync();

        public async Task<bool> HasActiveSubscriptionsForPlanAsync(int planId)
        {
            return await _context.CustomerSubscriptions.AnyAsync(x =>
                x.PlanId == planId &&
                x.Status == SubscriptionStatus.Active
            );
        }
    }
}
