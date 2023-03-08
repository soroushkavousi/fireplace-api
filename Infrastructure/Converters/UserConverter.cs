using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.ValueObjects;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

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
            if (user.Email != null)
                emailEntity = _serviceProvider.GetService<EmailConverter>().ConvertToEntity(user.Email.PureCopy());

            GoogleUserEntity googleUserEntity = null;
            if (user.GoogleUser != null)
                googleUserEntity = _serviceProvider.GetService<GoogleUserConverter>().ConvertToEntity(user.GoogleUser.PureCopy());

            List<AccessTokenEntity> accessTokenEntities = null;
            if (user.AccessTokens != null && user.AccessTokens.Count != 0)
                accessTokenEntities = user.AccessTokens.Select(
                    accessToken => _serviceProvider.GetService<AccessTokenConverter>().ConvertToEntity(accessToken.PureCopy())).ToList();

            List<SessionEntity> sessionEntities = null;
            if (user.Sessions != null && user.Sessions.Count != 0)
                sessionEntities = user.Sessions.Select(
                    session => _serviceProvider.GetService<SessionConverter>().ConvertToEntity(session.PureCopy())).ToList();

            var userEntity = new UserEntity(user.Id, user.Username, user.State.ToString(),
                user.CreationDate, user.DisplayName, user.About, user.AvatarUrl, user.BannerUrl,
                user.ModifiedDate, user.Password?.Hash, user.ResetPasswordCode, emailEntity, googleUserEntity,
                accessTokenEntities, sessionEntities);

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

            var user = new User(userEntity.Id, userEntity.Username, userEntity.State.ToEnum<UserState>(),
                userEntity.CreationDate, userEntity.DisplayName, userEntity.About, userEntity.AvatarUrl,
                userEntity.BannerUrl, userEntity.ModifiedDate, Password.OfHash(userEntity.PasswordHash),
                userEntity.ResetPasswordCode, email, googleUser, accessTokens, sessions);

            return user;
        }
    }
}
