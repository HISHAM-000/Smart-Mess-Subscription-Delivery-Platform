using MessMate.Application.Common.Models;
using MessMate.Application.Interfaces.Services;
using MessMate.Application.Services;
using MessMate.Domain.Interfaces.Contracts;
using MessMate.Domain.Interfaces.Repositories;
using MessMate.Infrastructure.BackgroundJobs;


//using MessMate.Application.Interfaces.Repositories;
//using MessMate.Application.Interfaces.Services;
//using MessMate.Domain.Interfaces;
using MessMate.Infrastructure.Data;
using MessMate.Infrastructure.Repositories;
using MessMate.Infrastructure.Services;
using MessMate.Infrastructure.UnitOfWork;


//using MessMate.Infrastructure.Repositories;
//using MessMate.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUserRepository,UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            services.AddScoped<ITokenService, JwtService>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IMessRepository, MessRepository>();
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<ISubscriptionPlanRepository, SubscriptionPlanRepository>();
            services.AddScoped<ICustomerSubscriptionRepository, CustomerSubscriptionRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IMealSkipRepository, MealSkipRepository>();
            services.AddHostedService<DailyOrderGenerationJob>();
            services.AddScoped<IDeliveryRepository, DeliveryRepository>();
            services.AddScoped<OrderGenerationService>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IMenuItemRepository, MenuItemRepository>();

            return services;
        }

    }
}
