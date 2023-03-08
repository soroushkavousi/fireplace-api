using FireplaceApi.Domain.Enums;

namespace FireplaceApi.Domain.Exceptions
{
    public class EmailAlreadyActivatedException : ApiException
    {
        public EmailAlreadyActivatedException(string emailAddress)
            : base(
                errorType: ErrorType.ALREADY_ACTIVATED,
                errorField: FieldName.EMAIL,
                errorServerMessage: "The email is already activated!",
                parameters: emailAddress,
                systemException: null
            )
        { }
    }
}
