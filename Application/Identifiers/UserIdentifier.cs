using FireplaceApi.Application.Enums;
using FireplaceApi.Application.Interfaces;

namespace FireplaceApi.Application.Identifiers;

public abstract class UserIdentifier : Identifier
{
    public static UserIdIdentifier OfId(ulong id)
        => new(id);

    public static UserUsernameIdentifier OfUsername(string username)
        => new(username);
}

public class UserIdIdentifier : UserIdentifier, IIdIdentifier
{
    public override FieldName TargetField => FieldName.USER_ID;
    public ulong Id { get; set; }

    internal UserIdIdentifier(ulong id)
    {
        Id = id;
    }
}

public class UserUsernameIdentifier : UserIdentifier
{
    public override FieldName TargetField => FieldName.USERNAME;
    public string Username { get; set; }

    internal UserUsernameIdentifier(string username)
    {
        Username = username;
    }
}
