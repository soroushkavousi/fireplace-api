using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Models;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.Extensions.Logging;
using System;

namespace FireplaceApi.Infrastructure.Converters
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

        public GlobalEntity ConvertToEntity(Global global)
        {
            if (global == null)
                return null;

            var globalEntity = new GlobalEntity(global.Id,
                global.EnvironmentName.ToString(), global.Values,
                global.CreationDate, global.ModifiedDate);

            return globalEntity;
        }

        public Global ConvertToModel(GlobalEntity globalEntity)
        {
            if (globalEntity == null)
                return null;

            var global = new Global(globalEntity.Id,
                globalEntity.EnvironmentName.ToEnum<EnvironmentName>(), globalEntity.Values,
                globalEntity.CreationDate, globalEntity.ModifiedDate);

            return global;
        }
    }
}
