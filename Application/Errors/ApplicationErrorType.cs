using FireplaceApi.Domain.Errors;
using System.Runtime.CompilerServices;

namespace FireplaceApi.Application.Errors;

public class ApplicationErrorType : ErrorType
{
    public static readonly ApplicationErrorType AUTHENTICATION_FAILED = new();
    public static readonly ApplicationErrorType NOT_EXIST_OR_ACCESS_DENIED = new();
    public static readonly ApplicationErrorType ALREADY_EXISTS = new();
    public static readonly ApplicationErrorType INCORRECT_VALUE = new();
    public static readonly ApplicationErrorType ALREADY_ACTIVATED = new();
    public static readonly ApplicationErrorType LIMITATION = new();

    protected ApplicationErrorType([CallerMemberName] string name = null) : base(name) { }
}
