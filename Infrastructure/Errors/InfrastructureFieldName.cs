using FireplaceApi.Application.Errors;
using System.Runtime.CompilerServices;

namespace FireplaceApi.Infrastructure.Errors;

public class InfrastructureFieldName : ApplicationFieldName
{
    public static readonly InfrastructureFieldName REQUEST_CONTENT_TYPE = new();
    public static readonly InfrastructureFieldName REQUEST_BODY = new();

    public static readonly InfrastructureFieldName ENCODED_ID = new();
    public static readonly InfrastructureFieldName USER_ID_OR_USERNAME = new();

    protected InfrastructureFieldName([CallerMemberName] string name = null) : base(name) { }
}
