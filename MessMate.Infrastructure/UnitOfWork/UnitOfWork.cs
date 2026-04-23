using MessMate.Domain.Interfaces.Contracts;
using MessMate.Domain.Interfaces.Repositories;
using MessMate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace MessMate.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;

        public IUserRepository Users { get; }
        public IRefreshTokenRepository RefreshTokens { get; }
        public IMessRepository Messes { get; }
        public ISubscriptionPlanRepository SubscriptionPlans { get; }
        public ICustomerSubscriptionRepository CustomerSubscriptions { get; }
        public IOrderRepository Orders { get; }
        public IMealSkipRepository MealSkips { get; }
        public IDeliveryRepository Deliveries { get; }
        public IMenuRepository Menus { get; }
        public IMenuItemRepository MenuItems { get; }

        public UnitOfWork(AppDbContext context, IUserRepository users,
            IRefreshTokenRepository refreshToken,
            IMessRepository messes,
            ISubscriptionPlanRepository subscriptionPlans,
            ICustomerSubscriptionRepository customerSubscriptions,
            IOrderRepository orders,
            IMealSkipRepository mealSkips,
            IDeliveryRepository deliveries,
            IMenuRepository menus,
            IMenuItemRepository menuItems)
        {
            _context = context;
            Users = users;
            RefreshTokens = refreshToken;
            Messes = messes;
            SubscriptionPlans = subscriptionPlans;
            CustomerSubscriptions = customerSubscriptions;
            Orders = orders;
            MealSkips = mealSkips;
            Deliveries = deliveries;
            Menus = menus;
            MenuItems = menuItems;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
         public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_transaction != null)
                await _transaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
                await _transaction.RollbackAsync();
        }
        }
}
