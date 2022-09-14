using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Models;
using FireplaceApi.Infrastructure.Entities;
using FireplaceApi.Infrastructure.ValueObjects;
using Microsoft.Extensions.Logging;
using System;

namespace FireplaceApi.Infrastructure.Converters
{
    public class ConfigsConverter
    {
        private readonly ILogger<ConfigsConverter> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ConfigsConverter(ILogger<ConfigsConverter> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public ConfigsEntity ConvertToEntity(Configs configs)
        {
            if (configs == null)
                return null;

            var configsEntityData = new ConfigsEntityData(configs.Api, configs.File,
                configs.Pagination, configs.Email, configs.Google);
            var configsEntity = new ConfigsEntity(configs.Id, configs.EnvironmentName.ToString(),
                configsEntityData, configs.CreationDate, configs.ModifiedDate);

            return configsEntity;
        }

        public Configs ConvertToModel(ConfigsEntity configsEntity)
        {
            if (configsEntity == null)
                return null;

            var configs = new Configs(configsEntity.Id, configsEntity.EnvironmentName.ToEnum<EnvironmentName>(),
                configsEntity.Data.Api, configsEntity.Data.File, configsEntity.Data.Pagination,
                configsEntity.Data.Email, configsEntity.Data.Google, configsEntity.CreationDate,
                configsEntity.ModifiedDate);
            return configs;
        }
    }
}
