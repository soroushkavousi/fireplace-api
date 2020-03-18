using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GamingCommunityApi.Controllers.Parameters.ErrorParameters;
using GamingCommunityApi.Entities;
using GamingCommunityApi.Enums;
using GamingCommunityApi.Extensions;
using GamingCommunityApi.Models;
using GamingCommunityApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingCommunityApi.Converters
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

        // Dto

        public ErrorDto ConvertToDto(Error error)
        {
            if (error == null)
                return null;

            var errorDto = new ErrorDto(error.Code, error.ClientMessage, error.HttpStatusCode);

            return errorDto;
        }

        public ApiExceptionErrorDto ConvertToApiExceptionDto(Error error)
        {
            if (error == null)
                return null;

            var errorDto = new ApiExceptionErrorDto(error.Code, error.ClientMessage);

            return errorDto;
        }

        // Entity

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
