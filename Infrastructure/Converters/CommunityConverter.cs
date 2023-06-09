using FireplaceApi.Domain.Models;
using FireplaceApi.Infrastructure.Entities;

namespace FireplaceApi.Infrastructure.Converters;

public static class CommunityConverter
{
    public static CommunityEntity ToEntity(this Community community)
    {
        if (community == null)
            return null;

        UserEntity creatorEntity = null;
        if (community.Creator != null)
            creatorEntity = community.Creator.PureCopy().ToEntity();

        var communityEntity = new CommunityEntity(community.Id, community.Name,
            community.CreatorId, community.CreatorUsername, community.CreationDate,
            community.ModifiedDate, creatorEntity);

        return communityEntity;
    }

    public static Community ToModel(this CommunityEntity communityEntity)
    {
        if (communityEntity == null)
            return null;

        User creator = null;
        if (communityEntity.CreatorEntity != null)
            creator = communityEntity.CreatorEntity.PureCopy().ToModel();

        var community = new Community(communityEntity.Id, communityEntity.Name,
            communityEntity.CreatorEntityId, communityEntity.CreatorEntityUsername,
            communityEntity.CreationDate, communityEntity.ModifiedDate, creator);

        return community;
    }
}
