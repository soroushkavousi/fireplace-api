using FireplaceApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FireplaceApi.Presentation.HostedServices;

// Add a hosted service to check the API readines
public class ReadinessCheckerHostedService : IHostedService
{
    private readonly ILogger<ReadinessCheckerHostedService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ReadinessCheckerHostedService(ILogger<ReadinessCheckerHostedService> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await CheckDatabase(cancellationToken);
    }

    private async Task CheckDatabase(CancellationToken cancellationToken)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApiDbContext>();

        try
        {
            var pendingMigrations = dbContext.Database.GetPendingMigrations();
            var databaseName = dbContext.Database.GetDbConnection().Database;
            var isTestDatabase = databaseName.Contains("test", StringComparison.OrdinalIgnoreCase);
            if (pendingMigrations.Any() && !isTestDatabase)
            {
                _logger.LogServerCritical($"Database migrations are not applied!!!",
                    parameters: new { PendingMigrations = $"[ {string.Join(", ", pendingMigrations)} ]" });
            }
            else
            {
                if (await dbContext.ConfigsEntities.AnyAsync(cancellationToken: cancellationToken) == false)
                {
                    _logger.LogServerCritical("No configs are found in the database!!!");
                }

                if (await dbContext.ErrorEntities.AnyAsync(cancellationToken: cancellationToken) == false)
                {
                    _logger.LogServerCritical("No errors are found in the database!!!");
                }
            }
        }
        catch (Exception)
        {
            var serverMessage = $"Could not connect to the database!!!";
            _logger.LogServerCritical(serverMessage);
            await Console.Out.WriteLineAsync(serverMessage);
            throw;
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}
