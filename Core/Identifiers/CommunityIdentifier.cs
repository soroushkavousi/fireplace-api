using FireplaceApi.Core.Interfaces;

namespace FireplaceApi.Core.Identifiers
{
    public abstract class CommunityIdentifier
    {
        public static CommunityIdIdentifier OfId(ulong id)
        {
            return new CommunityIdIdentifier(id);
        }

        public static CommunityNameIdentifier OfName(string name)
        {
            return new CommunityNameIdentifier(name);
        }
    }

    public class CommunityIdIdentifier : CommunityIdentifier, IIdIdentifier
    {
        public ulong Id { get; set; }

        internal CommunityIdIdentifier(ulong id)
        {
            Id = id;
        }
    }

    public class CommunityNameIdentifier : CommunityIdentifier
    {
        public string Name { get; set; }

        internal CommunityNameIdentifier(string name)
        {
            Name = name;
        }
    }
}
