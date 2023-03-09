using FireplaceApi.Application.Enums;
using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Exceptions;

namespace FireplaceApi.Application.Exceptions
{
    public class CommunitySortIncorrectValueException : ApiException
    {
        public CommunitySortIncorrectValueException(string sortString)
            : base(
                errorType: ErrorType.INCORRECT_VALUE,
                errorField: ApplicationFieldName.COMMUNITY_SORT,
                errorServerMessage: "The community sort is not correct!",
                parameters: new { sortString },
                systemException: null
            )
        { }
    }

    public class SortIncorrectValueException : ApiException
    {
        public SortIncorrectValueException(string sortString)
            : base(
                errorType: ErrorType.INCORRECT_VALUE,
                errorField: ApplicationFieldName.SORT,
                errorServerMessage: "The sort is not correct!",
                parameters: new { sortString },
                systemException: null
            )
        { }
    }

    public class RequestContentTypeIncorrectValueException : ApiException
    {
        public RequestContentTypeIncorrectValueException(string requestContentType)
            : base(
                errorType: ErrorType.INCORRECT_VALUE,
                errorField: ApplicationFieldName.REQUEST_CONTENT_TYPE,
                errorServerMessage: "The request content type is not correct!",
                parameters: new { requestContentType },
                systemException: null
            )
        { }
    }
}
