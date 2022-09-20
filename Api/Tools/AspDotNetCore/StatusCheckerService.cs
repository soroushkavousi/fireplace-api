using FireplaceApi.Core.Extensions;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FireplaceApi.Api.Tools
{
    public class StatusCheckerService : IHostedService
    {
        private readonly ILogger<StatusCheckerService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public StatusCheckerService(ILogger<StatusCheckerService> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider
                .GetRequiredService<FireplaceApiDbContext>();

            var pendingMigrations = dbContext.Database.GetPendingMigrations();
            if (pendingMigrations.Any())
            {
                _logger.LogAppCritical("Database migrations are not applied!!!",
                    parameters: new { PendingMigrations = $"[ {string.Join(", ", pendingMigrations)} ]" });
            }
            else
            {
                if (await dbContext.ConfigsEntities.AnyAsync(cancellationToken: cancellationToken) == false)
                {
                    _logger.LogAppCritical("No configs are found in the database!!!");
                }

                if (await dbContext.ErrorEntities.AnyAsync(cancellationToken: cancellationToken) == false)
                {
                    _logger.LogAppCritical("No errors are found in the database!!!");
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
