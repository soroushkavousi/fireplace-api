using FireplaceApi.Domain.Enums;

namespace FireplaceApi.Domain.Identifiers
{
    public abstract class Identifier
    {
        public abstract FieldName TargetField { get; }
    }
}
