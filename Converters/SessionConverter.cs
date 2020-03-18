using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using GamingCommunityApi.Entities;
using GamingCommunityApi.Entities.UserInformationEntities;
using GamingCommunityApi.Models;
using GamingCommunityApi.Models.UserInformations;
using GamingCommunityApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamingCommunityApi.Extensions;
using GamingCommunityApi.Enums;
using System.Net;
using GamingCommunityApi.Controllers.Parameters.SessionParameters;
using GamingCommunityApi.Controllers.Parameters.UserParameters;

namespace GamingCommunityApi.Converters
{
    public class SessionConverter
    {
        private readonly ILogger<SessionConverter> _logger;
        private readonly IServiceProvider _serviceProvider;

        public SessionConverter(ILogger<SessionConverter> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        // Dto

        public SessionDto ConvertToDto(Session session)
        {
            if (session == null)
                return null;

            UserDto userDto = null;
            if (session.User != null)
                userDto = _serviceProvider.GetService<UserConverter>().ConvertToDto(session.User.PureCopy());

            var sessionDto = new SessionDto(session.Id, session.UserId, session.IpAddress.ToString(),
                userDto);

            return sessionDto;
        }

        // Entity

        public SessionEntity ConvertToEntity(Session session)
        {
            if (session == null)
                return null;

            UserEntity userEntity = null;
            if (session.User != null)
                userEntity = _serviceProvider.GetService<UserConverter>().ConvertToEntity(session.User.PureCopy());

            var sessionEntity = new SessionEntity(session.UserId, session.IpAddress.ToString(),
                session.State.ToString(), session.Id, userEntity);

            return sessionEntity;
        }

        public Session ConvertToModel(SessionEntity sessionEntity)
        {
            if (sessionEntity == null)
                return null;

            User user = null;
            if (sessionEntity.UserEntity != null)
                user = _serviceProvider.GetService<UserConverter>().ConvertToModel(sessionEntity.UserEntity.PureCopy());

            var session = new Session(sessionEntity.Id.Value, sessionEntity.UserEntityId, 
                sessionEntity.IpAddress.ToIPAddress(), sessionEntity.State.ToEnum<SessionState>(), user);

            return session;
        }
    }
}
