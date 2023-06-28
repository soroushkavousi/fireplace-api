using FireplaceApi.Application.Errors;
using FireplaceApi.Domain.Errors;

namespace FireplaceApi.Infrastructure.Errors;

public class CommunitySortIncorrectValueException : ApiException
{
    public CommunitySortIncorrectValueException(string sortString)
        : base(
            errorType: ApplicationErrorType.INCORRECT_VALUE,
            errorField: FieldName.COMMUNITY_SORT,
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
            errorField: FieldName.POST_SORT,
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
            errorField: FieldName.COMMENT_SORT,
            errorServerMessage: "The comment sort is not correct!",
            parameters: new { sortString },
            systemException: null
        )
    { }
}