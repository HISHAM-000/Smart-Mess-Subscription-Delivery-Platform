using MessMate.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Domain.Interfaces.Contracts
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IRefreshTokenRepository RefreshTokens { get; }
        IMessRepository Messes { get; }
        ISubscriptionPlanRepository SubscriptionPlans { get; }
        ICustomerSubscriptionRepository CustomerSubscriptions { get; }
        IOrderRepository Orders { get; }
        IMealSkipRepository MealSkips { get; }
        IDeliveryRepository Deliveries { get; }
         IMenuRepository Menus { get; }
         IMenuItemRepository MenuItems { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
