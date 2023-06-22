using FireplaceApi.Domain.Common;
using FireplaceApi.Domain.Errors;
using FireplaceApi.Domain.Users;

namespace FireplaceApi.Domain.Communities;

public abstract class CommunityMembershipIdentifier : Identifier
{
    public static CommunityMembershipIdIdentifier OfId(ulong id)
        => new(id);

    public static CommunityMembershipUserAndCommunityIdentifier OfUserAndCommunity(
        UserIdentifier userIdentifier, CommunityIdentifier communityIdentifier)
        => new(userIdentifier, communityIdentifier);
}

public class CommunityMembershipIdIdentifier : CommunityMembershipIdentifier, IIdIdentifier
{
    public override FieldName TargetField => FieldName.COMMUNITY_MEMBERSHIP_ID;
    public ulong Id { get; set; }

    internal CommunityMembershipIdIdentifier(ulong id)
    {
        Id = id;
    }
}

public class CommunityMembershipUserAndCommunityIdentifier : CommunityMembershipIdentifier
{
    public override FieldName TargetField => FieldName.COMMUNITY_MEMBERSHIP;
    public UserIdentifier UserIdentifier { get; set; }
    public CommunityIdentifier CommunityIdentifier { get; set; }

    internal CommunityMembershipUserAndCommunityIdentifier(
        UserIdentifier userIdentifier, CommunityIdentifier communityIdentifier)
    {
        UserIdentifier = userIdentifier;
        CommunityIdentifier = communityIdentifier;
    }
}
