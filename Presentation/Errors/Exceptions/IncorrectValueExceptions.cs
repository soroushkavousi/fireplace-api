using FireplaceApi.Application.Errors;
using FireplaceApi.Domain.Errors;

namespace FireplaceApi.Presentation.Errors;

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
