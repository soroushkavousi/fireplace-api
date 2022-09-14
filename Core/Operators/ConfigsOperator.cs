using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Identifiers;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Operators
{
    public class ConfigsOperator
    {
        private readonly ILogger<ConfigsOperator> _logger;
        private readonly IConfigsRepository _configsRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ConfigsOperator(ILogger<ConfigsOperator> logger,
            IConfigsRepository configsRepository, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _configsRepository = configsRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task LoadConfigsAsync()
        {
            var environmentName = _webHostEnvironment.EnvironmentName.ToUpper().ToEnum<Enums.EnvironmentName>();
            var identifier = ConfigsIdentifier.OfEnvironmentName(environmentName);
            Configs.Current = await _configsRepository.GetConfigsByIdentifierAsync(identifier);
        }
    }
}
