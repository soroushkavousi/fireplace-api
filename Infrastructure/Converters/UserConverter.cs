using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FireplaceApi.Infrastructure.Entities;
using FireplaceApi.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.ValueObjects;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Enums;

namespace FireplaceApi.Infrastructure.Converters
{
    public class UserConverter
    {
        private readonly ILogger<UserConverter> _logger;
        private readonly IServiceProvider _serviceProvider;

        public UserConverter(ILogger<UserConverter> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public UserEntity ConvertToEntity(User user)
        {
            if (user == null)
                return null;

            EmailEntity emailEntity = null;
            if(user.Email != null)
                emailEntity = _serviceProvider.GetService<EmailConverter>().ConvertToEntity(user.Email.PureCopy());

            GoogleUserEntity googleUserEntity = null;
            if (user.GoogleUser != null)
                googleUserEntity = _serviceProvider.GetService<GoogleUserConverter>().ConvertToEntity(user.GoogleUser.PureCopy());

            List<AccessTokenEntity> accessTokenEntities = null;
            if(user.AccessTokens != null && user.AccessTokens.Count != 0)
                accessTokenEntities = user.AccessTokens.Select(
                    accessToken => _serviceProvider.GetService<AccessTokenConverter>().ConvertToEntity(accessToken.PureCopy())).ToList();

            List<SessionEntity> sessionEntities = null;
            if (user.Sessions != null && user.Sessions.Count != 0)
                sessionEntities = user.Sessions.Select(
                    session => _serviceProvider.GetService<SessionConverter>().ConvertToEntity(session.PureCopy())).ToList();

            var userEntity = new UserEntity(user.FirstName, user.LastName,
                user.Username, user.State.ToString(), user.CreationDate,
                user.ModifiedDate, user.Password?.Hash, user.Id,
                emailEntity, googleUserEntity, accessTokenEntities, sessionEntities);

            return userEntity;
        }

        public User ConvertToModel(UserEntity userEntity)
        {
            if (userEntity == null)
                return null;

            Email email = null;
            if (userEntity.EmailEntity != null)
                email = _serviceProvider.GetService<EmailConverter>().ConvertToModel(userEntity.EmailEntity.PureCopy());

            GoogleUser googleUser = null;
            if (userEntity.GoogleUserEntity != null)
                googleUser = _serviceProvider.GetService<GoogleUserConverter>().ConvertToModel(userEntity.GoogleUserEntity.PureCopy());

            List<AccessToken> accessTokens = null;
            if (userEntity.AccessTokenEntities != null && userEntity.AccessTokenEntities.Count != 0)
                accessTokens = userEntity.AccessTokenEntities.Select(
                    accessTokenEntity => _serviceProvider.GetService<AccessTokenConverter>().ConvertToModel(accessTokenEntity.PureCopy())).ToList();

            List<Session> sessions = null;
            if (userEntity.SessionEntities != null && userEntity.SessionEntities.Count != 0)
                sessions = userEntity.SessionEntities.Select(
                    sessionEntity => _serviceProvider.GetService<SessionConverter>().ConvertToModel(sessionEntity.PureCopy())).ToList();

            var user = new User(userEntity.Id.Value, userEntity.FirstName, userEntity.LastName,
                userEntity.Username, userEntity.State.ToEnum<UserState>(), userEntity.CreationDate, 
                userEntity.ModifiedDate, Password.OfHash(userEntity.PasswordHash), email, 
                googleUser, accessTokens, sessions);

            return user;
        }
    }
}
