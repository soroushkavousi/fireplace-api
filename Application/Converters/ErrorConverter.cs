using FireplaceApi.Application.Controllers;
using FireplaceApi.Domain.Models;
using Microsoft.Extensions.Logging;
using System;

namespace FireplaceApi.Application.Converters
{
    public class ErrorConverter : BaseConverter<Error, ErrorDto>
    {
        private readonly ILogger<ErrorConverter> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ErrorConverter(ILogger<ErrorConverter> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public override ErrorDto ConvertToDto(Error error)
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
