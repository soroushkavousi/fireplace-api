using FireplaceApi.Domain.Errors;

namespace FireplaceApi.Application.Errors;

public class EmailAlreadyActivatedException : ApiException
{
    public EmailAlreadyActivatedException(string emailAddress)
        : base(
            errorType: ApplicationErrorType.ALREADY_ACTIVATED,
            errorField: FieldName.EMAIL,
            errorServerMessage: "The email is already activated!",
            parameters: emailAddress,
            systemException: null
        )
    { }
}
