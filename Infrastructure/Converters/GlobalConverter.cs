using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GamingCommunityApi.Infrastructure.Entities;
using GamingCommunityApi.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamingCommunityApi.Core.Models;
using GamingCommunityApi.Core.Extensions;
using GamingCommunityApi.Core.Enums;

namespace GamingCommunityApi.Infrastructure.Converters
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

            var globalEntity = new GlobalEntity(global.Id.To<int>(), global.Values);

            return globalEntity;
        }

        public Global ConvertToModel(GlobalEntity globalEntity)
        {
            if (globalEntity == null)
                return null;

            var global = new Global(globalEntity.Id.To<GlobalId>(), globalEntity.Values);

            return global;
        }
    }
}
