using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.ValueObjects;
using FireplaceApi.Infrastructure.Entities;
using System.Collections.Generic;
using System.Linq;

namespace FireplaceApi.Infrastructure.Converters;

public static class UserConverter
{
    public static UserEntity ToEntity(this User user)
    {
        if (user == null)
            return null;

        EmailEntity emailEntity = null;
        if (user.Email != null)
            emailEntity = user.Email.PureCopy().ToEntity();

        GoogleUserEntity googleUserEntity = null;
        if (user.GoogleUser != null)
            googleUserEntity = user.GoogleUser.PureCopy().ToEntity();

        List<AccessTokenEntity> accessTokenEntities = null;
        if (user.AccessTokens != null && user.AccessTokens.Count != 0)
            accessTokenEntities = user.AccessTokens.Select(
                accessToken => accessToken.PureCopy().ToEntity()).ToList();

        List<SessionEntity> sessionEntities = null;
        if (user.Sessions != null && user.Sessions.Count != 0)
            sessionEntities = user.Sessions.Select(
                session => session.PureCopy().ToEntity()).ToList();

        var userEntity = new UserEntity(user.Id, user.Username, user.State.ToString(),
            user.CreationDate, user.DisplayName, user.About, user.AvatarUrl, user.BannerUrl,
            user.ModifiedDate, user.Password?.Hash, user.ResetPasswordCode, emailEntity, googleUserEntity,
            accessTokenEntities, sessionEntities);

        return userEntity;
    }

    public static User ToModel(this UserEntity userEntity)
    {
        if (userEntity == null)
            return null;

        Email email = null;
        if (userEntity.EmailEntity != null)
            email = userEntity.EmailEntity.PureCopy().ToModel();

        GoogleUser googleUser = null;
        if (userEntity.GoogleUserEntity != null)
            googleUser = userEntity.GoogleUserEntity.PureCopy().ToModel();

        List<AccessToken> accessTokens = null;
        if (userEntity.AccessTokenEntities != null && userEntity.AccessTokenEntities.Count != 0)
            accessTokens = userEntity.AccessTokenEntities.Select(
                accessTokenEntity => accessTokenEntity.PureCopy().ToModel()).ToList();

        List<Session> sessions = null;
        if (userEntity.SessionEntities != null && userEntity.SessionEntities.Count != 0)
            sessions = userEntity.SessionEntities.Select(
                sessionEntity => sessionEntity.PureCopy().ToModel()).ToList();

        var user = new User(userEntity.Id, userEntity.Username, userEntity.State.ToEnum<UserState>(),
            userEntity.CreationDate, userEntity.DisplayName, userEntity.About, userEntity.AvatarUrl,
            userEntity.BannerUrl, userEntity.ModifiedDate, Password.OfHash(userEntity.PasswordHash),
            userEntity.ResetPasswordCode, email, googleUser, accessTokens, sessions);

        return user;
    }
}
