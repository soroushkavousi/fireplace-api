using FireplaceApi.Application.Errors;
using System.Runtime.CompilerServices;

namespace FireplaceApi.Presentation.Enums;

public sealed class PresentationErrorType : ApplicationErrorType
{
    private PresentationErrorType([CallerMemberName] string name = null) : base(name) { }
}
