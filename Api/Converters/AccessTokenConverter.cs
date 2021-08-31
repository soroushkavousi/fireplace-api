using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FireplaceApi.Api.Controllers.Parameters.AccessTokenParameters;
using FireplaceApi.Api.Controllers.Parameters.UserParameters;
using FireplaceApi.Core.Models.UserInformations;

namespace FireplaceApi.Api.Converters
{
    public class AccessTokenConverter
    {
        private readonly ILogger<AccessTokenConverter> _logger;
        private readonly IServiceProvider _serviceProvider;

        public AccessTokenConverter(ILogger<AccessTokenConverter> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public AccessTokenDto ConvertToDto(AccessToken accessToken)
        {
            if (accessToken == null)
                return null;

            UserDto userDto = null;
            if (accessToken.User != null)
                userDto = _serviceProvider.GetService<UserConverter>()
                    .ConvertToDto(accessToken.User.PureCopy());

            var accessTokenDto = new AccessTokenDto(accessToken.UserId, accessToken.Value,
                accessToken.CreationDate, userDto);

            return accessTokenDto;
        }
    }
}
