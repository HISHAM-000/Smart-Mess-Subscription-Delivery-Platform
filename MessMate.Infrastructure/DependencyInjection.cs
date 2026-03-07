using MessMate.Application.Common.Models;
using MessMate.Application.Interfaces.Repositories;
using MessMate.Application.Interfaces.Services;
using MessMate.Infrastructure.Data;
using MessMate.Infrastructure.Repositories;
using MessMate.Infrastructure.Services;
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

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenService, JwtService>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IRoleApplicationRepository, RoleApplicationRepository>();
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.Configure<JwtSettings>(
            configuration.GetSection("JwtSettings"));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
                   if (jwtSettings == null)
                       throw new Exception("JwtSettings configuration is missing.");
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,

                       ValidIssuer = jwtSettings.Issuer,
                       ValidAudience = jwtSettings.Audience,

                       IssuerSigningKey = new SymmetricSecurityKey(
                           Encoding.UTF8.GetBytes(jwtSettings.Key)),
                       ClockSkew = TimeSpan.Zero
                   };

                   options.Events = new JwtBearerEvents
                   {
                       OnChallenge = async context =>
                       {
                           context.HandleResponse();

                           context.Response.StatusCode = 401;
                           context.Response.ContentType = "application/json";

                           await context.Response.WriteAsync(
                               System.Text.Json.JsonSerializer.Serialize(new
                               {
                                   success = false,
                                   message = "You are not authenticated."
                               }));
                       },

                       OnForbidden = async context =>
                       {

                           context.Response.StatusCode = StatusCodes.Status403Forbidden;
                           context.Response.ContentType = "application/json";

                           var response = System.Text.Json.JsonSerializer.Serialize(new
                           {
                               success = false,
                               message = "You are not authorized to access this resource."
                           });

                           await context.Response.WriteAsync(response);
                       }
                   };
               });

            return services;
        }

    }
}
