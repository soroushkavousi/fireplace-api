using FireplaceApi.Application.Errors;
using System.Runtime.CompilerServices;

namespace FireplaceApi.Presentation.Enums;

public sealed class PresentationErrorType : ApplicationErrorType
{
    public static readonly PresentationErrorType MISSING_FIELD = new();

    private PresentationErrorType([CallerMemberName] string name = null) : base(name) { }
}
