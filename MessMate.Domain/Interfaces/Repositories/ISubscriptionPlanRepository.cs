using MessMate.Domain.Entities;
using MessMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Domain.Interfaces.Repositories
{
    public interface ISubscriptionPlanRepository : IGenericRepository<SubscriptionPlan>
    {
        Task<List<SubscriptionPlan>> GetByMessIdAsync(int messId, bool activeOnly);
        Task<bool> ExistsAsync(int planId, int messId);
        Task<bool> ExistsExactPlanAsync(
            int messId,
            string name,
            PlanType planType,
            decimal price,
            int durationDays,
            int minActiveDays,
            bool isBreakfast,
            bool isLunch,
            bool isDinner
        );
        Task<bool> CheckAnyActiveUsersAsync(int planId);
    }
}
