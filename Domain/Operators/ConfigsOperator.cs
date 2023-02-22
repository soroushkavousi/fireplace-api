using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Identifiers;
using FireplaceApi.Domain.Interfaces;
using FireplaceApi.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Operators
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
            try
            {
                Configs.Current = await _configsRepository.GetConfigsByIdentifierAsync(identifier);
            }
            catch (Exception ex)
            {
                _logger.LogAppCritical("Could not load configs from the database!", ex: ex);
            }
            finally
            {
                if (Configs.Current == null || Configs.Current.Api == null)
                {
                    _logger.LogAppCritical("No configs are found in the database!");
                    Configs.Current = Configs.Default;
                }
                else
                    _logger.LogAppInformation($"The configs of environment {environmentName} were successfully retrieved from the database.");
            }
        }
    }
}
