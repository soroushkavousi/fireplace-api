using FireplaceApi.Domain.Common;
using FireplaceApi.Domain.Errors;

namespace FireplaceApi.Domain.Users;

public abstract class UserIdentifier : Identifier
{
    public static UserIdIdentifier OfId(ulong id)
        => new(id);

    public static UserUsernameIdentifier OfUsername(Username username)
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
    public Username Username { get; set; }

    internal UserUsernameIdentifier(Username username)
    {
        Username = username;
    }
}
