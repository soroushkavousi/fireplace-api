using FireplaceApi.Application.Errors;
using System.Runtime.CompilerServices;

namespace FireplaceApi.Infrastructure.Errors;

public class InfrastructureErrorType : ApplicationErrorType
{
    protected InfrastructureErrorType([CallerMemberName] string name = null) : base(name) { }
}
