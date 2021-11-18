using FireplaceApi.Core.Interfaces;

namespace FireplaceApi.Core.Identifiers
{
    public abstract class CommunityMembershipIdentifier
    {
        public static CommunityMembershipIdIdentifier OfId(ulong id)
            => new CommunityMembershipIdIdentifier(id);

        public static CommunityMembershipUserAndCommunityIdentifier OfUserAndCommunity(
            UserIdentifier userIdentifier, CommunityIdentifier communityIdentifier)
            => new CommunityMembershipUserAndCommunityIdentifier(
                userIdentifier, communityIdentifier);
    }

    public class CommunityMembershipIdIdentifier : CommunityMembershipIdentifier, IIdIdentifier
    {
        public ulong Id { get; set; }

        internal CommunityMembershipIdIdentifier(ulong id)
        {
            Id = id;
        }
    }

    public class CommunityMembershipUserAndCommunityIdentifier : CommunityMembershipIdentifier
    {
        public UserIdentifier UserIdentifier { get; set; }
        public CommunityIdentifier CommunityIdentifier { get; set; }

        internal CommunityMembershipUserAndCommunityIdentifier(
            UserIdentifier userIdentifier, CommunityIdentifier communityIdentifier)
        {
            UserIdentifier = userIdentifier;
            CommunityIdentifier = communityIdentifier;
        }
    }
}
