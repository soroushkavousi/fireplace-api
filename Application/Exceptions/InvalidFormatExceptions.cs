using FireplaceApi.Application.Enums;

namespace FireplaceApi.Application.Exceptions;

public class CommunityNameInvalidFormatException : ApiException
{
    public CommunityNameInvalidFormatException(string communityName, string serverMessage)
        : base(
            errorType: ErrorType.INVALID_FORMAT,
            errorField: FieldName.COMMUNITY_NAME,
            errorServerMessage: serverMessage,
            parameters: new { communityName },
            systemException: null
        )
    { }
}

public class PostContentInvalidFormatException : ApiException
{
    //TODO configs
    private static readonly int _maxLength = 2000;

    public PostContentInvalidFormatException(string postContent)
        : base(
            errorType: ErrorType.INVALID_FORMAT,
            errorField: FieldName.POST_CONTENT,
            errorServerMessage: "The post content doesn't have a valid format!",
            parameters: new { MaxLength = _maxLength, ContentLength = postContent.Length },
            systemException: null
        )
    { }
}

public class CommentContentInvalidException : ApiException
{
    //TODO configs
    private static readonly int _maxLength = 3000;

    public CommentContentInvalidException(string commentContent)
        : base(
            errorType: ErrorType.INVALID_FORMAT,
            errorField: FieldName.COMMENT_CONTENT,
            errorServerMessage: "The comment content doesn't have a valid format!",
            parameters: new { MaxLength = _maxLength, ContentLength = commentContent.Length },
            systemException: null
        )
    { }
}

public class UsernameInvalidFormatException : ApiException
{
    public UsernameInvalidFormatException(string username, string serverMessage)
        : base(
            errorType: ErrorType.INVALID_FORMAT,
            errorField: FieldName.USERNAME,
            errorServerMessage: serverMessage,
            parameters: new { username },
            systemException: null
        )
    { }
}

public class PasswordInvalidFormatException : ApiException
{
    public PasswordInvalidFormatException(string passwordHash, string reason)
        : base(
            errorType: ErrorType.INVALID_FORMAT,
            errorField: FieldName.PASSWORD,
            errorServerMessage: reason,
            parameters: new { passwordHash },
            systemException: null
        )
    { }
}

public class NewPasswordInvalidFormatException : ApiException
{
    public NewPasswordInvalidFormatException(string newPasswordHash, string reason)
        : base(
            errorType: ErrorType.INVALID_FORMAT,
            errorField: FieldName.NEW_PASSWORD,
            errorServerMessage: reason,
            parameters: new { newPasswordHash },
            systemException: null
        )
    { }
}

public class AvatarUrlInvalidFormatException : ApiException
{
    public AvatarUrlInvalidFormatException(string avatarUrl)
        : base(
            errorType: ErrorType.INVALID_FORMAT,
            errorField: FieldName.AVATART_URL,
            errorServerMessage: "The avatar url doesn't have a valid format!",
            parameters: new { avatarUrl },
            systemException: null
        )
    { }
}

public class BannerUrlInvalidFormatException : ApiException
{
    public BannerUrlInvalidFormatException(string bannerUrl)
        : base(
            errorType: ErrorType.INVALID_FORMAT,
            errorField: FieldName.BANNER_URL,
            errorServerMessage: "The banner url doesn't have a valid format!",
            parameters: new { bannerUrl },
            systemException: null
        )
    { }
}

public class DisplayNameInvalidFormatException : ApiException
{
    //TODO configs
    private static readonly int _maxLength = 80;

    public DisplayNameInvalidFormatException(string displayName)
        : base(
            errorType: ErrorType.INVALID_FORMAT,
            errorField: FieldName.DISPLAY_NAME,
            errorServerMessage: "The display name doesn't have a valid format!",
            parameters: new { displayName, MaxLength = _maxLength },
            systemException: null
        )
    { }
}

public class AboutInvalidFormatException : ApiException
{
    //TODO configs
    private static readonly int _maxLength = 2000;

    public AboutInvalidFormatException(string about)
        : base(
            errorType: ErrorType.INVALID_FORMAT,
            errorField: FieldName.ABOUT,
            errorServerMessage: "The about doesn't have a valid format!",
            parameters: new { ContentLength = about.Length, MaxLength = _maxLength },
            systemException: null
        )
    { }
}

public class EmailAddressInvalidFormatException : ApiException
{
    public EmailAddressInvalidFormatException(string emailAddress)
        : base(
            errorType: ErrorType.INVALID_FORMAT,
            errorField: FieldName.EMAIL_ADDRESS,
            errorServerMessage: "The email address doesn't have a valid format!",
            parameters: new { emailAddress },
            systemException: null
        )
    { }
}
