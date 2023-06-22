using FireplaceApi.Application.Enums;
using FireplaceApi.Application.Extensions;
using FireplaceApi.Application.Models;
using FireplaceApi.Application.ValueObjects;
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

        var userEntityRoles = user.Roles.Select(r => r.ToString()).ToList();

        EmailEntity emailEntity = null;
        if (user.Email != null)
            emailEntity = user.Email.PureCopy().ToEntity();

        GoogleUserEntity googleUserEntity = null;
        if (user.GoogleUser != null)
            googleUserEntity = user.GoogleUser.PureCopy().ToEntity();

        List<SessionEntity> sessionEntities = null;
        if (user.Sessions != null && user.Sessions.Count != 0)
            sessionEntities = user.Sessions.Select(
                session => session.PureCopy().ToEntity()).ToList();

        var userEntity = new UserEntity(user.Id, user.Username, user.State.ToString(),
            userEntityRoles, user.CreationDate, user.DisplayName, user.About, user.AvatarUrl, user.BannerUrl,
            user.ModifiedDate, user.Password?.Hash, user.ResetPasswordCode, emailEntity, googleUserEntity,
            sessionEntities);

        return userEntity;
    }

    public static User ToModel(this UserEntity userEntity)
    {
        if (userEntity == null)
            return null;

        var userRoles = userEntity.Roles.Select(r => r.ToEnum<UserRole>()).ToList();

        Email email = null;
        if (userEntity.EmailEntity != null)
            email = userEntity.EmailEntity.PureCopy().ToModel();

        GoogleUser googleUser = null;
        if (userEntity.GoogleUserEntity != null)
            googleUser = userEntity.GoogleUserEntity.PureCopy().ToModel();

        List<Session> sessions = null;
        if (userEntity.SessionEntities != null && userEntity.SessionEntities.Count != 0)
            sessions = userEntity.SessionEntities.Select(
                sessionEntity => sessionEntity.PureCopy().ToModel()).ToList();

        var user = new User(userEntity.Id, userEntity.Username, userEntity.State.ToEnum<UserState>(),
            userRoles, userEntity.DisplayName, userEntity.About, userEntity.AvatarUrl,
            userEntity.BannerUrl, userEntity.CreationDate, userEntity.ModifiedDate,
            Password.OfHash(userEntity.PasswordHash), userEntity.ResetPasswordCode,
            email, googleUser, sessions);

        return user;
    }
}
