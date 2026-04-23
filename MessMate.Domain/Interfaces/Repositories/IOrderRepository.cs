using MessMate.Domain.Entities;
using MessMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Domain.Interfaces.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<List<Order>> GetByCustomerIdAsync(int customerId);
        Task<Order?> GetByIdWithDetailsAsync(int orderId);
        Task<bool> ExistsForDateAndSlotAsync(int subscriptionId, DateTime date, MealSlot slot);
        Task<List<Order>> GetByMessIdAndDateAsync(int messId, DateTime date);
        Task<List<Order>> GetBySubscriptionAndDateRangeAsync(
            int subscriptionId,
            DateOnly from,
            DateOnly to);
        //Task<List<Order>> GetBySubscriptionIdAsync(int subscriptionId);
    }
}
