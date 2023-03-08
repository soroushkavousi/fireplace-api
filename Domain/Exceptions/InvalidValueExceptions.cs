using FireplaceApi.Domain.Enums;

namespace FireplaceApi.Domain.Exceptions
{
    public class CommunityNameInvalidValueException : ApiException
    {
        public CommunityNameInvalidValueException(string communityName, string serverMessage)
            : base(
                errorType: ErrorType.INVALID_VALUE,
                errorField: FieldName.COMMUNITY_NAME,
                errorServerMessage: serverMessage,
                parameters: new { communityName },
                systemException: null
            )
        { }
    }

    public class PostContentInvalidValueException : ApiException
    {
        //TODO configs
        private static readonly int _maxLength = 2000;

        public PostContentInvalidValueException(string postContent)
            : base(
                errorType: ErrorType.INVALID_VALUE,
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
                errorType: ErrorType.INVALID_VALUE,
                errorField: FieldName.COMMENT_CONTENT,
                errorServerMessage: "The comment content doesn't have a valid format!",
                parameters: new { MaxLength = _maxLength, ContentLength = commentContent.Length },
                systemException: null
            )
        { }
    }

    public class UsernameInvalidValueException : ApiException
    {
        public UsernameInvalidValueException(string username, string serverMessage)
            : base(
                errorType: ErrorType.INVALID_VALUE,
                errorField: FieldName.USERNAME,
                errorServerMessage: serverMessage,
                parameters: new { username },
                systemException: null
            )
        { }
    }

    public class PasswordInvalidValueException : ApiException
    {
        public PasswordInvalidValueException(string passwordHash, string reason)
            : base(
                errorType: ErrorType.INVALID_VALUE,
                errorField: FieldName.PASSWORD,
                errorServerMessage: reason,
                parameters: new { passwordHash },
                systemException: null
            )
        { }
    }

    public class NewPasswordInvalidValueException : ApiException
    {
        public NewPasswordInvalidValueException(string newPasswordHash, string reason)
            : base(
                errorType: ErrorType.INVALID_VALUE,
                errorField: FieldName.NEW_PASSWORD,
                errorServerMessage: reason,
                parameters: new { newPasswordHash },
                systemException: null
            )
        { }
    }

    public class ResetPasswordCodeInvalidValueException : ApiException
    {
        public ResetPasswordCodeInvalidValueException(string resetPasswordCode)
            : base(
                errorType: ErrorType.INVALID_VALUE,
                errorField: FieldName.RESET_PASSWORD_CODE,
                errorServerMessage: "The reset password code is not correct!",
                parameters: new { resetPasswordCode },
                systemException: null
            )
        { }
    }

    public class AvatarUrlInvalidValueException : ApiException
    {
        public AvatarUrlInvalidValueException(string avatarUrl)
            : base(
                errorType: ErrorType.INVALID_VALUE,
                errorField: FieldName.AVATART_URL,
                errorServerMessage: "The avatar url doesn't have a valid format!",
                parameters: new { avatarUrl },
                systemException: null
            )
        { }
    }

    public class BannerUrlInvalidValueException : ApiException
    {
        public BannerUrlInvalidValueException(string bannerUrl)
            : base(
                errorType: ErrorType.INVALID_VALUE,
                errorField: FieldName.BANNER_URL,
                errorServerMessage: "The banner url doesn't have a valid format!",
                parameters: new { bannerUrl },
                systemException: null
            )
        { }
    }

    public class DisplayNameInvalidValueException : ApiException
    {
        public DisplayNameInvalidValueException(string displayName)
            : base(
                errorType: ErrorType.INVALID_VALUE,
                errorField: FieldName.DISPLAY_NAME,
                errorServerMessage: "The display name doesn't have a valid format!",
                parameters: new { displayName },
                systemException: null
            )
        { }
    }

    public class AboutInvalidValueException : ApiException
    {
        //TODO configs
        private static readonly int _maxLength = 2000;

        public AboutInvalidValueException(string about)
            : base(
                errorType: ErrorType.INVALID_VALUE,
                errorField: FieldName.ABOUT,
                errorServerMessage: "The about doesn't have a valid format!",
                parameters: new { MaxLength = _maxLength, ContentLength = about.Length },
                systemException: null
            )
        { }
    }

    public class EmailAddressInvalidValueException : ApiException
    {
        public EmailAddressInvalidValueException(string emailAddress)
            : base(
                errorType: ErrorType.INVALID_VALUE,
                errorField: FieldName.EMAIL_ADDRESS,
                errorServerMessage: "The email address doesn't have a valid format!",
                parameters: new { emailAddress },
                systemException: null
            )
        { }
    }

    public class ActivationCodeInvalidValueException : ApiException
    {
        public ActivationCodeInvalidValueException(int activationCode)
            : base(
                errorType: ErrorType.INVALID_VALUE,
                errorField: FieldName.ACTIVATION_CODE,
                errorServerMessage: "The activation code doesn't have a valid format!",
                parameters: new { activationCode },
                systemException: null
            )
        { }
    }

    public class AcessTokenInvalidValueException : ApiException
    {
        public AcessTokenInvalidValueException(string accessToken)
            : base(
                errorType: ErrorType.INVALID_VALUE,
                errorField: FieldName.ACCESS_TOKEN,
                errorServerMessage: "The access token doesn't have a valid format!",
                parameters: new { accessToken },
                systemException: null
            )
        { }
    }

    public class ErrorClientMessageInvalidValueException : ApiException
    {
        public ErrorClientMessageInvalidValueException(string errorClientMessage)
            : base(
                errorType: ErrorType.INVALID_VALUE,
                errorField: FieldName.ERROR_MESSAGE,
                errorServerMessage: "The error client message doesn't have a valid format!",
                parameters: new { errorClientMessage },
                systemException: null
            )
        { }
    }
}
