using FireplaceApi.Domain.Communities;
using FireplaceApi.Domain.Users;
using FireplaceApi.Infrastructure.Entities;

namespace FireplaceApi.Infrastructure.Converters;

public static class CommunityMembershipConverter
{
    public static CommunityMembershipEntity ToEntity(this CommunityMembership communityMembership)
    {
        if (communityMembership == null)
            return null;

        UserEntity userEntity = null;
        if (communityMembership.User != null)
            userEntity = communityMembership.User.PureCopy().ToEntity();

        CommunityEntity communityEntity = null;
        if (communityMembership.Community != null)
            communityEntity = communityMembership.Community.PureCopy().ToEntity();

        var communityMembershipEntity = new CommunityMembershipEntity(
            communityMembership.Id, communityMembership.UserId,
            communityMembership.Username, communityMembership.CommunityId,
            communityMembership.CommunityName, communityMembership.CreationDate,
            communityMembership.ModifiedDate, userEntity, communityEntity);

        return communityMembershipEntity;
    }

    public static CommunityMembership ToModel(this CommunityMembershipEntity communityMembershipEntity)
    {
        if (communityMembershipEntity == null)
            return null;

        User user = null;
        if (communityMembershipEntity.UserEntity != null)
            user = communityMembershipEntity.UserEntity.PureCopy().ToModel();

        Community community = null;
        if (communityMembershipEntity.CommunityEntity != null)
            community = communityMembershipEntity.CommunityEntity.PureCopy().ToModel();

        var communityMembership = new CommunityMembership(communityMembershipEntity.Id,
            communityMembershipEntity.UserEntityId, communityMembershipEntity.UserEntityUsername,
            communityMembershipEntity.CommunityEntityId, communityMembershipEntity.CommunityEntityName,
            communityMembershipEntity.CreationDate, communityMembershipEntity.ModifiedDate, user,
            community);

        return communityMembership;
    }
}
