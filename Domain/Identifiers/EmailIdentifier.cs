using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Interfaces;

namespace FireplaceApi.Domain.Identifiers;

public abstract class EmailIdentifier : Identifier
{
    public static EmailIdIdentifier OfId(ulong id)
         => new(id);

    public static EmailAddressIdentifier OfAddress(string address)
        => new(address);

    public static EmailUserIdIdentifier OfUserId(ulong userId)
        => new(userId);
}

public class EmailIdIdentifier : EmailIdentifier, IIdIdentifier
{
    public override FieldName TargetField => FieldName.EMAIL_ID;
    public ulong Id { get; set; }

    internal EmailIdIdentifier(ulong id)
    {
        Id = id;
    }
}

public class EmailAddressIdentifier : EmailIdentifier
{
    public override FieldName TargetField => FieldName.EMAIL_ADDRESS;
    public string Address { get; set; }

    internal EmailAddressIdentifier(string address)
    {
        Address = address;
    }
}

public class EmailUserIdIdentifier : EmailIdentifier
{
    public override FieldName TargetField => FieldName.EMAIL;
    public ulong UserId { get; set; }

    internal EmailUserIdIdentifier(ulong userId)
    {
        UserId = userId;
    }
}
