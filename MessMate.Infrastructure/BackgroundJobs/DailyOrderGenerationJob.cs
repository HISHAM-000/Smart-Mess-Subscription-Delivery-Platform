using MessMate.Application.Services;
using MessMate.Domain.Entities;
using MessMate.Domain.Enums;
using MessMate.Domain.Interfaces.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Infrastructure.BackgroundJobs
{
    public class DailyOrderGenerationJob : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<DailyOrderGenerationJob> _logger;
        private readonly TimeSpan _runAt = new TimeSpan(6, 0, 0);

        public DailyOrderGenerationJob(
            IServiceScopeFactory scopeFactory,
            ILogger<DailyOrderGenerationJob> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("DailyOrderGenerationJob started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                var delay = CalculateDelay();
                await Task.Delay(delay, stoppingToken);

                if (!stoppingToken.IsCancellationRequested)
                {
                    await ProcessDailyWorkAsync(stoppingToken);
                }
            }
        }

        private TimeSpan CalculateDelay()
        {
            var now = DateTime.Now;
            var nextRun = now.Date.Add(_runAt);

            if (now > nextRun)
                nextRun = nextRun.AddDays(1);

            return nextRun - now;
        }

        private async Task ProcessDailyWorkAsync(CancellationToken ct)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();

                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var orderService = scope.ServiceProvider.GetRequiredService<OrderGenerationService>();

                var today = DateTime.Now.Date;
                var now = DateTime.Now;

                await ResumePausedSubscriptionsAsync(unitOfWork, today);
                await ExpireSubscriptionsAsync(unitOfWork, today);

                var count = await orderService.GenerateOrdersForTodayAsync(today, now, ct);

                await unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Orders generated: {Count}", count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in daily job.");
            }
        }

        private async Task ResumePausedSubscriptionsAsync(IUnitOfWork unitOfWork, DateTime today)
        {
            var subs = await unitOfWork.CustomerSubscriptions
                .GetSubscriptionsToResumeAsync(today);

            foreach (var sub in subs)
            {
                sub.Status = SubscriptionStatus.Active;
                sub.PausedFrom = null;
                sub.PausedUntil = null;
            }
        }

        private async Task ExpireSubscriptionsAsync(IUnitOfWork unitOfWork, DateTime today)
        {
            var subs = await unitOfWork.CustomerSubscriptions
                .GetSubscriptionsToExpireAsync(today);

            foreach (var sub in subs)
            {
                sub.Status = SubscriptionStatus.Expired;
            }
        }
    }
}
