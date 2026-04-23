using MessMate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Domain.Interfaces.Repositories
{
    public interface ICustomerSubscriptionRepository : IGenericRepository<CustomerSubscription>
    {
        Task<List<CustomerSubscription>> GetByCustomerIdAsync(int customerId);
        Task<CustomerSubscription?> GetActiveByCustomerAndMessAsync(int customerId, int messId);
        Task<List<CustomerSubscription>> GetActiveSubscriptionsForDateAsync(DateTime date);
        Task<bool> HasActiveSubscriptionsByMessIdAsync(int messId);
        Task<List<CustomerSubscription>> GetSubscriptionsToResumeAsync(DateTime today);
        Task<List<CustomerSubscription>> GetSubscriptionsToExpireAsync(DateTime today);
        Task<bool> HasAnyActiveSubscriptionAsync(int customerId);
        Task<bool> HasActiveSubscriptionsForPlanAsync(int planId);
    }
}
