using FireplaceApi.Domain.Errors;
using FireplaceApi.Presentation.Enums;

namespace FireplaceApi.Presentation.Exceptions;

public class RequestBodyMissingFieldException : MissingFieldException
{
    public RequestBodyMissingFieldException()
        : base(
            errorField: PresentationFieldName.REQUEST_BODY
        )
    { }
}
