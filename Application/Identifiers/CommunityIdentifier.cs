using FireplaceApi.Application.Enums;
using FireplaceApi.Application.Interfaces;

namespace FireplaceApi.Application.Identifiers;

public abstract class CommunityIdentifier : Identifier
{
    public static CommunityIdIdentifier OfId(ulong id)
        => new(id);

    public static CommunityNameIdentifier OfName(string name)
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
    public string Name { get; set; }

    internal CommunityNameIdentifier(string name)
    {
        Name = name;
    }
}
