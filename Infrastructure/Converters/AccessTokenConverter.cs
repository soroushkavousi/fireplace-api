using FireplaceApi.Domain.Models;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace FireplaceApi.Infrastructure.Converters
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


        public AccessTokenEntity ConvertToEntity(AccessToken accessToken)
        {
            if (accessToken == null)
                return null;

            UserEntity userEntity = null;
            if (accessToken.User != null)
                userEntity = _serviceProvider.GetService<UserConverter>().ConvertToEntity(accessToken.User.PureCopy());

            var accessTokenEntity = new AccessTokenEntity(accessToken.Id, accessToken.UserId, accessToken.Value,
                accessToken.CreationDate, accessToken.ModifiedDate, userEntity);

            return accessTokenEntity;
        }

        public AccessToken ConvertToModel(AccessTokenEntity accessTokenEntity)
        {
            if (accessTokenEntity == null)
                return null;

            User user = null;
            if (accessTokenEntity.UserEntity != null)
                user = _serviceProvider.GetService<UserConverter>().ConvertToModel(accessTokenEntity.UserEntity.PureCopy());

            var accessToken = new AccessToken(accessTokenEntity.Id, accessTokenEntity.UserEntityId,
                accessTokenEntity.Value, accessTokenEntity.CreationDate, accessTokenEntity.ModifiedDate, user);

            return accessToken;
        }
    }
}
