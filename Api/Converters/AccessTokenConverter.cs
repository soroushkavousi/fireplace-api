﻿using FireplaceApi.Api.Controllers;
using FireplaceApi.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

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
