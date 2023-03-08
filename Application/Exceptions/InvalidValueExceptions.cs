using FireplaceApi.Application.Enums;
using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Exceptions;

namespace FireplaceApi.Application.Exceptions
{
    public class CommunityEncodedIdOrNameInvalidValueException : ApiException
    {
        public CommunityEncodedIdOrNameInvalidValueException(string encodedIdOrName)
            : base(
                errorType: ErrorType.INVALID_VALUE,
                errorField: ApplicationFieldName.COMMUNITY_ID_OR_NAME,
                errorServerMessage: "The community encoded id or name is not valid!!",
                parameters: new { encodedIdOrName },
                systemException: null
            )
        { }
    }

    public class CommunitySortInvalidValueException : ApiException
    {
        public CommunitySortInvalidValueException(string sortString)
            : base(
                errorType: ErrorType.INVALID_VALUE,
                errorField: ApplicationFieldName.COMMUNITY_SORT,
                errorServerMessage: "The community sort is not valid!!",
                parameters: new { sortString },
                systemException: null
            )
        { }
    }

    public class PostEncodedIdInvalidValueException : ApiException
    {
        public PostEncodedIdInvalidValueException(string postEncodedId)
            : base(
                errorType: ErrorType.INVALID_VALUE,
                errorField: ApplicationFieldName.POST_ID,
                errorServerMessage: "The encoded post id is not valid!!",
                parameters: new { postEncodedId },
                systemException: null
            )
        { }
    }

    public class CommentEncodedIdInvalidValueException : ApiException
    {
        public CommentEncodedIdInvalidValueException(string commentEncodedId)
            : base(
                errorType: ErrorType.INVALID_VALUE,
                errorField: ApplicationFieldName.COMMENT_ID,
                errorServerMessage: "The encoded comment id is not valid!!",
                parameters: new { commentEncodedId },
                systemException: null
            )
        { }
    }

    public class UserEncodedIdOrUsernameInvalidValueException : ApiException
    {
        public UserEncodedIdOrUsernameInvalidValueException(string encodedIdOrUsername)
            : base(
                errorType: ErrorType.INVALID_VALUE,
                errorField: ApplicationFieldName.USER_ID_OR_USERNAME,
                errorServerMessage: "The user encoded id or username is not valid!!",
                parameters: new { encodedIdOrUsername },
                systemException: null
            )
        { }
    }

    public class SortInvalidValueException : ApiException
    {
        public SortInvalidValueException(string sortString)
            : base(
                errorType: ErrorType.INVALID_VALUE,
                errorField: ApplicationFieldName.SORT,
                errorServerMessage: "The sort is not valid!!",
                parameters: new { sortString },
                systemException: null
            )
        { }
    }

    public class RequestBodyInvalidValueException : ApiException
    {
        public RequestBodyInvalidValueException()
            : base(
                errorType: ErrorType.INVALID_VALUE,
                errorField: ApplicationFieldName.REQUEST_BODY,
                errorServerMessage: "Input request body is not json!",
                parameters: null,
                systemException: null
            )
        { }
    }

    public class RequestContentTypeInvalidValueException : ApiException
    {
        public RequestContentTypeInvalidValueException(string requestContentType)
            : base(
                errorType: ErrorType.INVALID_VALUE,
                errorField: ApplicationFieldName.REQUEST_CONTENT_TYPE,
                errorServerMessage: "The request content type is not valid!",
                parameters: new { requestContentType },
                systemException: null
            )
        { }
    }
}
