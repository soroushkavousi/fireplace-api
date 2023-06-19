using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Models;
using FireplaceApi.Infrastructure.Entities;

namespace FireplaceApi.Infrastructure.Converters;

public static class SessionConverter
{
    public static SessionEntity ToEntity(this Session session)
    {
        if (session == null)
            return null;

        UserEntity userEntity = null;
        if (session.User != null)
            userEntity = session.User.PureCopy().ToEntity();

        var sessionEntity = new SessionEntity(session.Id, session.UserId,
            session.IpAddress.ToString(), session.State.ToString(),
            session.RefreshToken, session.CreationDate,
            session.ModifiedDate, userEntity);

        return sessionEntity;
    }

    public static Session ToModel(this SessionEntity sessionEntity)
    {
        if (sessionEntity == null)
            return null;

        User user = null;
        if (sessionEntity.UserEntity != null)
            user = sessionEntity.UserEntity.PureCopy().ToModel();

        var session = new Session(sessionEntity.Id, sessionEntity.UserEntityId,
            sessionEntity.IpAddress.ToIPAddress(), sessionEntity.State.ToEnum<SessionState>(),
            sessionEntity.RefreshToken, sessionEntity.CreationDate,
            sessionEntity.ModifiedDate, user);

        return session;
    }
}
