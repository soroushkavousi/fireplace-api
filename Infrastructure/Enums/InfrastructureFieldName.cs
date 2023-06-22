using FireplaceApi.Application.Enums;
using System.Runtime.CompilerServices;

namespace FireplaceApi.Infrastructure.Enums;

public class InfrastructureFieldName : FieldName
{
    public static readonly InfrastructureFieldName REQUEST_CONTENT_TYPE = new();
    public static readonly InfrastructureFieldName REQUEST_BODY = new();

    public static readonly InfrastructureFieldName ENCODED_ID = new();
    public static readonly InfrastructureFieldName LIST_OF_IDS = new();
    public static readonly InfrastructureFieldName USER_ID_OR_USERNAME = new();
    public static readonly InfrastructureFieldName COMMUNITY_ID_OR_NAME = new();

    private InfrastructureFieldName([CallerMemberName] string name = null) : base(name) { }
}
