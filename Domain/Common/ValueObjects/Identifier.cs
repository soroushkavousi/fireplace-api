using FireplaceApi.Domain.Errors;

namespace FireplaceApi.Domain.Common;

public abstract class Identifier
{
    public abstract FieldName TargetField { get; }
}
