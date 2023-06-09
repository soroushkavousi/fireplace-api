using FireplaceApi.Domain.Models;
using FireplaceApi.Infrastructure.Entities;

namespace FireplaceApi.Infrastructure.Converters;

public static class AccessTokenConverter
{
    public static AccessTokenEntity ToEntity(this AccessToken accessToken)
    {
        if (accessToken == null)
            return null;

        UserEntity userEntity = null;
        if (accessToken.User != null)
            userEntity = accessToken.User.PureCopy().ToEntity();

        var accessTokenEntity = new AccessTokenEntity(accessToken.Id, accessToken.UserId, accessToken.Value,
            accessToken.CreationDate, accessToken.ModifiedDate, userEntity);

        return accessTokenEntity;
    }

    public static AccessToken ToModel(this AccessTokenEntity accessTokenEntity)
    {
        if (accessTokenEntity == null)
            return null;

        User user = null;
        if (accessTokenEntity.UserEntity != null)
            user = accessTokenEntity.UserEntity.PureCopy().ToModel();

        var accessToken = new AccessToken(accessTokenEntity.Id, accessTokenEntity.UserEntityId,
            accessTokenEntity.Value, accessTokenEntity.CreationDate, accessTokenEntity.ModifiedDate, user);

        return accessToken;
    }
}
