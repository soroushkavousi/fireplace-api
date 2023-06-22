using FireplaceApi.Application.Enums;

namespace FireplaceApi.Application.Identifiers;

public abstract class Identifier
{
    public abstract FieldName TargetField { get; }
}
