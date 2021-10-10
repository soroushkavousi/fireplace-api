using Microsoft.Extensions.Logging;
using System;

namespace FireplaceApi.Api.Converters
{
    public class GlobalConverter
    {
        private readonly ILogger<GlobalConverter> _logger;
        private readonly IServiceProvider _serviceProvider;

        public GlobalConverter(ILogger<GlobalConverter> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
    }
}
