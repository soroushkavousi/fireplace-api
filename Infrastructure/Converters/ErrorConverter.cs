﻿using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Models;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.Extensions.Logging;
using System;

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

            var errorEntity = new ErrorEntity(error.Id, error.Name.ToString(),
                error.Code, error.ClientMessage, error.HttpStatusCode,
                error.CreationDate, error.ModifiedDate);

            return errorEntity;
        }

        public Error ConvertToModel(ErrorEntity errorEntity)
        {
            if (errorEntity == null)
                return null;

            var error = new Error(errorEntity.Id, errorEntity.Name.ToEnum<ErrorName>(),
                errorEntity.Code, errorEntity.ClientMessage, errorEntity.HttpStatusCode,
                errorEntity.CreationDate, errorEntity.ModifiedDate);

            return error;
        }
    }
}
