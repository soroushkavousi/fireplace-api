using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using FireplaceApi.Api.Controllers.Parameters.ErrorParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FireplaceApi.Core.Models;

namespace FireplaceApi.Api.Converters
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
    }
}
