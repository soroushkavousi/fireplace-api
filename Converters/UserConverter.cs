using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using GamingCommunityApi.Controllers.Parameters.UserParameters;
using GamingCommunityApi.Entities;
using GamingCommunityApi.Entities.UserInformationEntities;
using GamingCommunityApi.Models;
using GamingCommunityApi.Models.UserInformations;
using GamingCommunityApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamingCommunityApi.Enums;
using GamingCommunityApi.Extensions;
using GamingCommunityApi.Controllers.Parameters.EmailParameters;
using GamingCommunityApi.ValueObjects;
using GamingCommunityApi.Controllers.Parameters.SessionParameters;

namespace GamingCommunityApi.Converters
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

        // Dto

        public UserDto ConvertToDto(User user)
        {
            if (user == null)
                return null;

            EmailDto emailDto = null;
            if (user.Email != null)
                emailDto = _serviceProvider.GetService<EmailConverter>().ConvertToDto(user.Email.PureCopy());

            string accessTokenValue = null;
            if(user.AccessTokens != null && user.AccessTokens.Count != 0)
                accessTokenValue = user.AccessTokens.Last().Value;

            List<SessionDto> sessionDtos = null;
            if (user.Sessions != null && user.Sessions.Count != 0)
                sessionDtos = user.Sessions.Select(
                    session => _serviceProvider.GetService<SessionConverter>().ConvertToDto(session.PureCopy())).ToList();

            var userDto = new UserDto(user.Id, user.FirstName, user.LastName,
                user.Username, user.State.ToString(), emailDto, 
                accessTokenValue, sessionDtos);

            return userDto;
        }

        // Entity

        public UserEntity ConvertToEntity(User user)
        {
            if (user == null)
                return null;

            EmailEntity emailEntity = null;
            if(user.Email != null)
                emailEntity = _serviceProvider.GetService<EmailConverter>().ConvertToEntity(user.Email.PureCopy());

            List<AccessTokenEntity> accessTokenEntities = null;
            if(user.AccessTokens != null && user.AccessTokens.Count != 0)
                accessTokenEntities = user.AccessTokens.Select(
                    accessToken => _serviceProvider.GetService<AccessTokenConverter>().ConvertToEntity(accessToken.PureCopy())).ToList();

            List<SessionEntity> sessionEntities = null;
            if (user.Sessions != null && user.Sessions.Count != 0)
                sessionEntities = user.Sessions.Select(
                    session => _serviceProvider.GetService<SessionConverter>().ConvertToEntity(session.PureCopy())).ToList();

            var userEntity = new UserEntity(user.FirstName, user.LastName,
                user.Username, user.Password.Hash, user.State.ToString(), user.Id,
                emailEntity, accessTokenEntities, sessionEntities);

            return userEntity;
        }

        public User ConvertToModel(UserEntity userEntity)
        {
            if (userEntity == null)
                return null;

            Email email = null;
            if (userEntity.EmailEntity != null)
                email = _serviceProvider.GetService<EmailConverter>().ConvertToModel(userEntity.EmailEntity.PureCopy());

            List<AccessToken> accessTokens = null;
            if (userEntity.AccessTokenEntities != null && userEntity.AccessTokenEntities.Count != 0)
                accessTokens = userEntity.AccessTokenEntities.Select(
                    accessTokenEntity => _serviceProvider.GetService<AccessTokenConverter>().ConvertToModel(accessTokenEntity.PureCopy())).ToList();

            List<Session> sessions = null;
            if (userEntity.SessionEntities != null && userEntity.SessionEntities.Count != 0)
                sessions = userEntity.SessionEntities.Select(
                    sessionEntity => _serviceProvider.GetService<SessionConverter>().ConvertToModel(sessionEntity.PureCopy())).ToList();

            var user = new User(userEntity.Id.Value, userEntity.FirstName, userEntity.LastName,
                userEntity.Username, Password.OfHash(userEntity.PasswordHash), userEntity.State.ToEnum<UserState>(),
                email, accessTokens, sessions);

            return user;
        }
    }
}
