using FireplaceApi.Domain.Models;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace FireplaceApi.Infrastructure.Converters;

public class GoogleUserConverter
{
    private readonly ILogger<GoogleUserConverter> _logger;
    private readonly IServiceProvider _serviceProvider;

    public GoogleUserConverter(ILogger<GoogleUserConverter> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    // Entity

    public GoogleUserEntity ConvertToEntity(GoogleUser googleUser)
    {
        if (googleUser == null)
            return null;

        UserEntity userEntity = null;
        if (googleUser.User != null)
            userEntity = _serviceProvider.GetService<UserConverter>()
                .ConvertToEntity(googleUser.User.PureCopy());

        var googleUserEntity = new GoogleUserEntity(googleUser.Id, googleUser.UserId, googleUser.Code,
            googleUser.AccessToken, googleUser.TokenType, googleUser.AccessTokenExpiresInSeconds,
            googleUser.RefreshToken, googleUser.Scope, googleUser.IdToken,
            googleUser.AccessTokenIssuedTime, googleUser.GmailAddress, googleUser.GmailVerified,
            googleUser.GmailIssuedTimeInSeconds, googleUser.FullName, googleUser.FirstName,
            googleUser.LastName, googleUser.Locale, googleUser.PictureUrl, googleUser.State,
            googleUser.AuthUser, googleUser.Prompt, googleUser.RedirectToUserUrl,
            googleUser.CreationDate, googleUser.ModifiedDate, userEntity);

        return googleUserEntity;
    }

    public GoogleUser ConvertToModel(GoogleUserEntity googleUserEntity)
    {
        if (googleUserEntity == null)
            return null;

        User user = null;
        if (googleUserEntity.UserEntity != null)
            user = _serviceProvider.GetService<UserConverter>()
                .ConvertToModel(googleUserEntity.UserEntity.PureCopy());

        var googleUser = new GoogleUser(googleUserEntity.Id, googleUserEntity.UserEntityId,
            googleUserEntity.Code, googleUserEntity.AccessToken, googleUserEntity.TokenType,
            googleUserEntity.AccessTokenExpiresInSeconds, googleUserEntity.RefreshToken,
            googleUserEntity.Scope, googleUserEntity.IdToken, googleUserEntity.AccessTokenIssuedTime,
            googleUserEntity.GmailAddress, googleUserEntity.GmailVerified,
            googleUserEntity.GmailIssuedTimeInSeconds, googleUserEntity.FullName,
            googleUserEntity.FirstName, googleUserEntity.LastName, googleUserEntity.Locale,
            googleUserEntity.PictureUrl, googleUserEntity.State, googleUserEntity.AuthUser,
            googleUserEntity.Prompt, googleUserEntity.RedirectToUserUrl,
            googleUserEntity.CreationDate, googleUserEntity.ModifiedDate,
            user);

        return googleUser;
    }
}
