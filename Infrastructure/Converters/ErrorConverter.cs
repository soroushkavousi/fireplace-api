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
    public class ErrorConverter
    {
        private readonly ILogger<ErrorConverter> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ErrorConverter(ILogger<ErrorConverter> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public ErrorEntity ConvertToEntity(Error error)
        {
            if (error == null)
                return null;

            var errorEntity = new ErrorEntity(error.Name.ToString(), error.Code,
                error.ClientMessage, error.HttpStatusCode, error.Id);

            return errorEntity;
        }

        public Error ConvertToModel(ErrorEntity errorEntity)
        {
            if (errorEntity == null)
                return null;

            var error = new Error(errorEntity.Id.Value, errorEntity.Name.ToEnum<ErrorName>(),
                errorEntity.Code, errorEntity.ClientMessage, errorEntity.HttpStatusCode);

            return error;
        }
    }
}
