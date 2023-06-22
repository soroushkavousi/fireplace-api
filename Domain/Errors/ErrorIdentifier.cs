using FireplaceApi.Domain.Common;

namespace FireplaceApi.Domain.Errors;

public abstract class ErrorIdentifier : Identifier
{
    public abstract string Key { get; }

    public static ErrorIdIdentifier OfId(ulong id)
         => new(id);

    public static ErrorCodeIdentifier OfCode(int code)
        => new(code);

    public static ErrorTypeAndFieldIdentifier OfTypeAndField(ErrorType type, FieldName field)
        => new(type, field);
}

public class ErrorIdIdentifier : ErrorIdentifier, IIdIdentifier
{
    public override FieldName TargetField => FieldName.ERROR_ID;
    public override string Key => $"{Id}";
    public ulong Id { get; set; }

    internal ErrorIdIdentifier(ulong id)
    {
        Id = id;
    }
}

public class ErrorCodeIdentifier : ErrorIdentifier
{
    public override FieldName TargetField => FieldName.ERROR_CODE;
    public override string Key => $"{Code}";
    public int Code { get; set; }

    internal ErrorCodeIdentifier(int code)
    {
        Code = code;
    }
}

public class ErrorTypeAndFieldIdentifier : ErrorIdentifier
{
    public override FieldName TargetField => FieldName.ERROR;
    public override string Key => $"{Type}_{Field}";
    public ErrorType Type { get; set; }
    public FieldName Field { get; set; }

    internal ErrorTypeAndFieldIdentifier(ErrorType type, FieldName field)
    {
        Type = type;
        Field = field;
    }
}
