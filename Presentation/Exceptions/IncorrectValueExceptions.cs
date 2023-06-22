using FireplaceApi.Application.Errors;
using FireplaceApi.Domain.Errors;
using FireplaceApi.Presentation.Enums;

namespace FireplaceApi.Presentation.Exceptions;

public class CommunitySortIncorrectValueException : ApiException
{
    public CommunitySortIncorrectValueException(string sortString)
        : base(
            errorType: ApplicationErrorType.INCORRECT_VALUE,
            errorField: PresentationFieldName.COMMUNITY_SORT,
            errorServerMessage: "The community sort is not correct!",
            parameters: new { sortString },
            systemException: null
        )
    { }
}

public class PostSortIncorrectValueException : ApiException
{
    public PostSortIncorrectValueException(string sortString)
        : base(
            errorType: ApplicationErrorType.INCORRECT_VALUE,
            errorField: PresentationFieldName.POST_SORT,
            errorServerMessage: "The post sort is not correct!",
            parameters: new { sortString },
            systemException: null
        )
    { }
}

public class CommentSortIncorrectValueException : ApiException
{
    public CommentSortIncorrectValueException(string sortString)
        : base(
            errorType: ApplicationErrorType.INCORRECT_VALUE,
            errorField: PresentationFieldName.COMMENT_SORT,
            errorServerMessage: "The comment sort is not correct!",
            parameters: new { sortString },
            systemException: null
        )
    { }
}

public class RequestContentTypeIncorrectValueException : ApiException
{
    public RequestContentTypeIncorrectValueException(string requestContentType)
        : base(
            errorType: ApplicationErrorType.INCORRECT_VALUE,
            errorField: PresentationFieldName.REQUEST_CONTENT_TYPE,
            errorServerMessage: "The request content type is not correct!",
            parameters: new { requestContentType },
            systemException: null
        )
    { }
}
