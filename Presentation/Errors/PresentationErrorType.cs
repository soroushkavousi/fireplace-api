using FireplaceApi.Infrastructure.Errors;
using System.Runtime.CompilerServices;

namespace FireplaceApi.Presentation.Errors;

public sealed class PresentationErrorType : InfrastructureErrorType
{
    private PresentationErrorType([CallerMemberName] string name = null) : base(name) { }
}
