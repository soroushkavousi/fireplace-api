using FireplaceApi.Application.Enums;
using System.Runtime.CompilerServices;

namespace FireplaceApi.Infrastructure.Enums;

public sealed class InfrastructureErrorType : ErrorType
{
    public static readonly InfrastructureErrorType MISSING_FIELD = new();

    private InfrastructureErrorType([CallerMemberName] string name = null) : base(name) { }
}
