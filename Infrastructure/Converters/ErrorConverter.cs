using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using FireplaceApi.Infrastructure.Entities;
using FireplaceApi.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Enums;

namespace FireplaceApi.Infrastructure.Converters
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
                error.ClientMessage, error.HttpStatusCode,  
                error.CreationDate, error.ModifiedDate,
                error.Id);

            return errorEntity;
        }

        public Error ConvertToModel(ErrorEntity errorEntity)
        {
            if (errorEntity == null)
                return null;

            var error = new Error(errorEntity.Id.Value, errorEntity.Name.ToEnum<ErrorName>(),
                errorEntity.Code, errorEntity.ClientMessage, errorEntity.HttpStatusCode,
                errorEntity.CreationDate, errorEntity.ModifiedDate);

            return error;
        }
    }
}
