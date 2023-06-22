using FireplaceApi.Domain.Errors;
using System.Runtime.CompilerServices;

namespace FireplaceApi.Presentation.Enums;

public class PresentationFieldName : FieldName
{
    public static readonly PresentationFieldName REQUEST_CONTENT_TYPE = new();
    public static readonly PresentationFieldName REQUEST_BODY = new();

    public static readonly PresentationFieldName ENCODED_ID = new();
    public static readonly PresentationFieldName LIST_OF_IDS = new();
    public static readonly PresentationFieldName USER_ID_OR_USERNAME = new();
    public static readonly PresentationFieldName COMMUNITY_ID_OR_NAME = new();

    private PresentationFieldName([CallerMemberName] string name = null) : base(name) { }
}
