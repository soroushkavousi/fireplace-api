using FireplaceApi.Domain.ValueObjects;
using System.Runtime.CompilerServices;

namespace FireplaceApi.Domain.Enums;

public class ErrorType : Enumeration<ErrorType>
{
    public static readonly ErrorType AUTHENTICATION_FAILED = new();
    public static readonly ErrorType NOT_EXIST_OR_ACCESS_DENIED = new();
    public static readonly ErrorType ALREADY_EXISTS = new();
    public static readonly ErrorType INVALID_FORMAT = new();
    public static readonly ErrorType INTERNAL_SERVER = new();
    public static readonly ErrorType INCORRECT_VALUE = new();
    public static readonly ErrorType ALREADY_ACTIVATED = new();
    public static readonly ErrorType LIMITATION = new();

    protected ErrorType([CallerMemberName] string name = null) : base(name) { }
}
