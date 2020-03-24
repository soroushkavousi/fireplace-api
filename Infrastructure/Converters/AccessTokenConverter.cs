using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using GamingCommunityApi.Infrastructure.Entities;
using GamingCommunityApi.Infrastructure.Entities.UserInformationEntities;
using GamingCommunityApi.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamingCommunityApi.Core.Models.UserInformations;

namespace GamingCommunityApi.Infrastructure.Converters
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

            var accessTokenEntity = new AccessTokenEntity(accessToken.UserId, accessToken.Value,
                accessToken.Id, userEntity);

            return accessTokenEntity;
        }

        public AccessToken ConvertToModel(AccessTokenEntity accessTokenEntity)
        {
            if (accessTokenEntity == null)
                return null;

            User user = null;
            if (accessTokenEntity.UserEntity != null)
                user = _serviceProvider.GetService<UserConverter>().ConvertToModel(accessTokenEntity.UserEntity.PureCopy());

            var accessToken = new AccessToken(accessTokenEntity.Id.Value, accessTokenEntity.UserEntityId,
                accessTokenEntity.Value, user);

            return accessToken;
        }
    }
}
