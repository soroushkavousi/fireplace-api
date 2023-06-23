using FireplaceApi.Application.Errors;
using System.Runtime.CompilerServices;

namespace FireplaceApi.Infrastructure.Enums;

public sealed class InfrastructureErrorType : ApplicationErrorType
{
    private InfrastructureErrorType([CallerMemberName] string name = null) : base(name) { }
}
