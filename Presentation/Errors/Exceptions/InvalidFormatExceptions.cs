using FireplaceApi.Domain.Errors;

namespace FireplaceApi.Presentation.Errors;

public class CommunityEncodedIdOrNameInvalidFormatException : ApiException
{
    public CommunityEncodedIdOrNameInvalidFormatException(string encodedIdOrName)
        : base(
            errorType: ErrorType.INVALID_FORMAT,
            errorField: PresentationFieldName.COMMUNITY_ID_OR_NAME,
            errorServerMessage: "The community encoded id or name is not valid!!",
            parameters: new { encodedIdOrName },
            systemException: null
        )
    { }
}

public class CommunityEncodedIdInvalidFormatException : ApiException
{
    public CommunityEncodedIdInvalidFormatException(string communityEncodedId)
        : base(
            errorType: ErrorType.INVALID_FORMAT,
            errorField: FieldName.COMMUNITY_ID,
            errorServerMessage: "The encoded community id is not valid!!",
            parameters: new { communityEncodedId },
            systemException: null
        )
    { }
}

public class PostEncodedIdInvalidFormatException : ApiException
{
    public PostEncodedIdInvalidFormatException(string postEncodedId)
        : base(
            errorType: ErrorType.INVALID_FORMAT,
            errorField: FieldName.POST_ID,
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
            errorField: FieldName.COMMENT_ID,
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
            errorField: PresentationFieldName.USER_ID_OR_USERNAME,
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
            errorField: PresentationFieldName.REQUEST_BODY,
            errorServerMessage: "Input request body is not json!",
            parameters: null,
            systemException: null
        )
    { }
}
