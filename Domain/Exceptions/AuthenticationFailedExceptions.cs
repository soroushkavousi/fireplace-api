using FireplaceApi.Domain.Enums;

namespace FireplaceApi.Domain.Exceptions;

public class UsernameAndPasswordAuthenticationFailedException : ApiException
{
    public UsernameAndPasswordAuthenticationFailedException(string username, string passwordHash)
        : base(
            errorType: ErrorType.AUTHENTICATION_FAILED,
            errorField: FieldName.USERNAME_AND_PASSWORD,
            errorServerMessage: "Username doesn't match with the password!",
            parameters: new { username, passwordHash },
            systemException: null
        )
    { }
}

public class EmailAndPasswordAuthenticationFailedException : ApiException
{
    public EmailAndPasswordAuthenticationFailedException(string emailAddress, string passwordHash)
        : base(
            errorType: ErrorType.AUTHENTICATION_FAILED,
            errorField: FieldName.EMAIL_AND_PASSWORD,
            errorServerMessage: "Email doesn't match with the password",
            parameters: new { emailAddress, passwordHash },
            systemException: null
        )
    { }
}

public class AccessTokenAuthenticationFailedException : ApiException
{
    public AccessTokenAuthenticationFailedException(string serverMessage = "Access token not found!")
        : base(
            errorType: ErrorType.AUTHENTICATION_FAILED,
            errorField: FieldName.ACCESS_TOKEN,
            errorServerMessage: serverMessage,
            parameters: null,
            systemException: null
        )
    { }
}
