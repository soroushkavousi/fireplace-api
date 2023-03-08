using FireplaceApi.Application.Enums;
using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Exceptions;

namespace FireplaceApi.Application.Exceptions
{
    public class CommunityEncodedIdOrNameInvalidFormatException : ApiException
    {
        public CommunityEncodedIdOrNameInvalidFormatException(string encodedIdOrName)
            : base(
                errorType: ErrorType.INVALID_FORMAT,
                errorField: ApplicationFieldName.COMMUNITY_ID_OR_NAME,
                errorServerMessage: "The community encoded id or name is not valid!!",
                parameters: new { encodedIdOrName },
                systemException: null
            )
        { }
    }

    public class PostEncodedIdInvalidFormatException : ApiException
    {
        public PostEncodedIdInvalidFormatException(string postEncodedId)
            : base(
                errorType: ErrorType.INVALID_FORMAT,
                errorField: ApplicationFieldName.POST_ID,
                errorServerMessage: "The encoded post id is not valid!!",
                parameters: new { postEncodedId },
                systemException: null
            )
        { }
    }

    public class CommentEncodedIdInvalidFormatException : ApiException
    {
        public CommentEncodedIdInvalidFormatException(string commentEncodedId)
            : base(
                errorType: ErrorType.INVALID_FORMAT,
                errorField: ApplicationFieldName.COMMENT_ID,
                errorServerMessage: "The encoded comment id is not valid!!",
                parameters: new { commentEncodedId },
                systemException: null
            )
        { }
    }

    public class UserEncodedIdOrUsernameInvalidFormatException : ApiException
    {
        public UserEncodedIdOrUsernameInvalidFormatException(string encodedIdOrUsername)
            : base(
                errorType: ErrorType.INVALID_FORMAT,
                errorField: ApplicationFieldName.USER_ID_OR_USERNAME,
                errorServerMessage: "The user encoded id or username is not valid!!",
                parameters: new { encodedIdOrUsername },
                systemException: null
            )
        { }
    }

    public class RequestBodyInvalidFormatException : ApiException
    {
        public RequestBodyInvalidFormatException()
            : base(
                errorType: ErrorType.INVALID_FORMAT,
                errorField: ApplicationFieldName.REQUEST_BODY,
                errorServerMessage: "Input request body is not json!",
                parameters: null,
                systemException: null
            )
        { }
    }
}
