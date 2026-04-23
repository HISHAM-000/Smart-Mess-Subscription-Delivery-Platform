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
    public class SubscriptionPlanRepository : GenericRepository<SubscriptionPlan>,
        ISubscriptionPlanRepository
    {
        public SubscriptionPlanRepository(AppDbContext context) : base(context) { }

        public async Task<List<SubscriptionPlan>> GetByMessIdAsync(int messId, bool activeOnly)
        {
            var query = _context.SubscriptionPlans
                .Where(p => p.MessId == messId && p.IsDeleted != true);

            if (activeOnly)
                query = query.Where(p => p.IsActive);

            return await query.OrderByDescending(p => p.CreatedOn).ToListAsync();
        }

        public async Task<bool> ExistsAsync(int planId, int messId)
        {
            var plans = await _context.SubscriptionPlans.AnyAsync(p => p.Id == planId              
                  && p.MessId == messId
                  && p.IsActive
                  && p.IsDeleted != true);

            return plans;
        }

        public async Task<bool> ExistsExactPlanAsync(
            int messId,
            string name,
            PlanType planType,
            decimal price,
            int durationDays,
            int minActiveDays,
            bool isBreakfast,
            bool isLunch,
            bool isDinner)
        {
            return await _context.SubscriptionPlans.AnyAsync(x =>
                x.MessId == messId &&
                x.Name.ToLower() == name &&   // normalized
                x.PlanType == planType &&
                x.Price == price &&
                x.DurationDays == durationDays &&
                x.MinActiveDays == minActiveDays &&
                x.IsBreakfast == isBreakfast &&
                x.IsLunch == isLunch &&
                x.IsDinner == isDinner
            );
        }

        public async Task<bool> CheckAnyActiveUsersAsync(int id)
        {
            return await _context.CustomerSubscriptions
                .AnyAsync(c => c.PlanId == id && 
                c.Status == SubscriptionStatus.Active);

        }
    }
}
