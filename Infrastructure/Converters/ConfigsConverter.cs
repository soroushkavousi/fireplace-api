using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Models;
using FireplaceApi.Infrastructure.Entities;
using FireplaceApi.Infrastructure.ValueObjects;

namespace FireplaceApi.Infrastructure.Converters;

public class ConfigsConverter
{
    public ConfigsEntity ConvertToEntity(Configs configs)
    {
        if (configs == null)
            return null;

        var configsEntityData = new ConfigsEntityData(configs.Api, configs.File,
            configs.QueryResult, configs.Email, configs.Google);
        var configsEntity = new ConfigsEntity(configs.Id, configs.EnvironmentName.ToString(),
            configsEntityData, configs.CreationDate, configs.ModifiedDate);

        return configsEntity;
    }

    public Configs ConvertToModel(ConfigsEntity configsEntity)
    {
        if (configsEntity == null)
            return null;

        var configs = new Configs(configsEntity.Id, configsEntity.EnvironmentName.ToEnum<EnvironmentName>(),
            configsEntity.Data.Api, configsEntity.Data.File, configsEntity.Data.QueryResult,
            configsEntity.Data.Email, configsEntity.Data.Google, configsEntity.CreationDate,
            configsEntity.ModifiedDate);
        return configs;
    }
}
