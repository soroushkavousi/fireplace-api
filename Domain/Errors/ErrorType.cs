using System.Runtime.CompilerServices;

namespace FireplaceApi.Domain.Errors;

public class ErrorType : Enumeration<ErrorType>
{
    public static readonly ErrorType INTERNAL_SERVER = new();
    public static readonly ErrorType MISSING_FIELD = new();
    public static readonly ErrorType INVALID_FORMAT = new();

    protected ErrorType([CallerMemberName] string name = null) : base(name) { }
}
