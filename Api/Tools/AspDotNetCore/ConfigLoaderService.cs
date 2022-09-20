using FireplaceApi.Core.Operators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FireplaceApi.Api.Tools
{
    public class ConfigLoaderService : IHostedService
    {
        private readonly ILogger<ConfigLoaderService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ConfigLoaderService(ILogger<ConfigLoaderService> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            var configsOperator = scope.ServiceProvider
                .GetRequiredService<ConfigsOperator>();

            await configsOperator.LoadConfigsAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
