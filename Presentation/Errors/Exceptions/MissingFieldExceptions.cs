using FireplaceApi.Domain.Errors;

namespace FireplaceApi.Presentation.Errors;

public class RequestBodyMissingFieldException : MissingFieldException
{
    public RequestBodyMissingFieldException()
        : base(
            errorField: PresentationFieldName.REQUEST_BODY
        )
    { }
}
