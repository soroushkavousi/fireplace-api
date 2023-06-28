using FireplaceApi.Domain.Errors;

namespace FireplaceApi.Domain.Communities;

public abstract class CommunityIdentifier : Identifier
{
    public static CommunityIdIdentifier OfId(ulong id)
        => new(id);

    public static CommunityNameIdentifier OfName(CommunityName name)
        => new(name);
}

public class CommunityIdIdentifier : CommunityIdentifier, IIdIdentifier
{
    public override FieldName TargetField => FieldName.COMMUNITY_ID;
    public ulong Id { get; set; }

    internal CommunityIdIdentifier(ulong id)
    {
        Id = id;
    }
}

public class CommunityNameIdentifier : CommunityIdentifier
{
    public override FieldName TargetField => FieldName.COMMUNITY_NAME;
    public CommunityName Name { get; set; }

    internal CommunityNameIdentifier(CommunityName name)
    {
        Name = name;
    }
}
