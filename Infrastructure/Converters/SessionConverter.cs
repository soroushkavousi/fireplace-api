using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Models;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace FireplaceApi.Infrastructure.Converters
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

        public SessionEntity ConvertToEntity(Session session)
        {
            if (session == null)
                return null;

            UserEntity userEntity = null;
            if (session.User != null)
                userEntity = _serviceProvider.GetService<UserConverter>().ConvertToEntity(session.User.PureCopy());

            var sessionEntity = new SessionEntity(session.UserId, session.IpAddress.ToString(),
                session.State.ToString(), session.CreationDate, session.ModifiedDate,
                session.Id, userEntity);

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
                sessionEntity.IpAddress.ToIPAddress(), sessionEntity.State.ToEnum<SessionState>(),
                sessionEntity.CreationDate, sessionEntity.ModifiedDate, user);

            return session;
        }
    }
}
