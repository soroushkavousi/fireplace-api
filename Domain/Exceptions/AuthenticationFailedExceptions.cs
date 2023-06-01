using FireplaceApi.Domain.Enums;
using System.Net;

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
    public AccessTokenAuthenticationFailedException(string accessTokenValue)
        : base(
            errorType: ErrorType.AUTHENTICATION_FAILED,
            errorField: FieldName.ACCESS_TOKEN,
            errorServerMessage: "Access token not found!",
            parameters: accessTokenValue,
            systemException: null
        )
    { }
}

public class SessionClosedAuthenticationFailedException : ApiException
{
    public SessionClosedAuthenticationFailedException(ulong requestingUserId, IPAddress ipAddress)
        : base(
            errorType: ErrorType.AUTHENTICATION_FAILED,
            errorField: FieldName.SESSION,
            errorServerMessage: "The user session was closed!",
            parameters: new { requestingUserId, ipAddress },
            systemException: null
        )
    { }
}

public class CsrfTokenAuthenticationFailedException : ApiException
{
    public CsrfTokenAuthenticationFailedException(string headerCsrfToken, string cookieCsrfToken)
        : base(
            errorType: ErrorType.AUTHENTICATION_FAILED,
            errorField: FieldName.CSRF_TOKEN,
            errorServerMessage: "The csrf token in headers does not match!",
            parameters: new { headerCsrfToken, cookieCsrfToken },
            systemException: null
        )
    { }
}
