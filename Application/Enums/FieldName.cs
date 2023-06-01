using FireplaceApi.Domain.Enums;
using System.Runtime.CompilerServices;

namespace FireplaceApi.Application.Enums;

public class ApplicationFieldName : FieldName
{
    public static readonly ApplicationFieldName REQUEST_CONTENT_TYPE = new();
    public static readonly ApplicationFieldName REQUEST_BODY = new();

    public static readonly ApplicationFieldName ENCODED_ID = new();
    public static readonly ApplicationFieldName LIST_OF_IDS = new();
    public static readonly ApplicationFieldName USER_ID_OR_USERNAME = new();
    public static readonly ApplicationFieldName COMMUNITY_ID_OR_NAME = new();

    private ApplicationFieldName([CallerMemberName] string name = null) : base(name) { }
}
