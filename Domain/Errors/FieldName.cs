using System.Runtime.CompilerServices;

namespace FireplaceApi.Domain.Errors;

public class FieldName : Enumeration<FieldName>
{
    public static readonly FieldName GENERAL = new();
    public static readonly FieldName LIST_OF_IDS = new();
    public static readonly FieldName USER = new();
    public static readonly FieldName USER_ID = new();
    public static readonly FieldName USERNAME = new();
    public static readonly FieldName DISPLAY_NAME = new();
    public static readonly FieldName ABOUT = new();
    public static readonly FieldName ACCESS_TOKEN = new();
    public static readonly FieldName COMMUNITY = new();
    public static readonly FieldName COMMUNITY_ID = new();
    public static readonly FieldName COMMUNITY_NAME = new();
    public static readonly FieldName COMMUNITY_ID_OR_NAME = new();
    public static readonly FieldName COMMUNITY_MEMBERSHIP = new();
    public static readonly FieldName COMMUNITY_MEMBERSHIP_ID = new();
    public static readonly FieldName PASSWORD = new();
    public static readonly FieldName NEW_PASSWORD = new();
    public static readonly FieldName COMMENT = new();
    public static readonly FieldName COMMENT_ID = new();
    public static readonly FieldName COMMENT_CONTENT = new();
    public static readonly FieldName COMMENT_VOTE = new();
    public static readonly FieldName COMMENT_SORT = new();
    public static readonly FieldName IS_UPVOTE = new();
    public static readonly FieldName POST = new();
    public static readonly FieldName POST_ID = new();
    public static readonly FieldName POST_CONTENT = new();
    public static readonly FieldName POST_VOTE = new();
    public static readonly FieldName POST_SORT = new();
    public static readonly FieldName EMAIL = new();
    public static readonly FieldName EMAIL_ID = new();
    public static readonly FieldName EMAIL_ADDRESS = new();
    public static readonly FieldName URL = new();
    public static readonly FieldName AVATART_URL = new();
    public static readonly FieldName BANNER_URL = new();
    public static readonly FieldName COMMUNITY_SORT = new();
    public static readonly FieldName ACTIVATION_CODE = new();
    public static readonly FieldName GOOGLE_CODE = new();
    public static readonly FieldName RESET_PASSWORD_CODE = new();
    public static readonly FieldName ERROR = new();
    public static readonly FieldName ERROR_ID = new();
    public static readonly FieldName ERROR_CODE = new();
    public static readonly FieldName ERROR_MESSAGE = new();
    public static readonly FieldName SESSION = new();
    public static readonly FieldName SESSION_ID = new();
    public static readonly FieldName EXPIRED_SESSION = new();
    public static readonly FieldName REVOKED_SESSION = new();
    public static readonly FieldName IP = new();
    public static readonly FieldName MAX_REQUEST_PER_IP = new();
    public static readonly FieldName CSRF_TOKEN = new();
    public static readonly FieldName USERNAME_AND_PASSWORD = new();
    public static readonly FieldName EMAIL_AND_PASSWORD = new();
    public static readonly FieldName SEARCH = new();
    public static readonly FieldName ROLE = new();
    public static readonly FieldName USER_STATE = new();

    protected FieldName([CallerMemberName] string name = null) : base(name) { }
}
