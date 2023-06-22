using FireplaceApi.Domain.Errors;

namespace FireplaceApi.Application.Errors;

public class PasswordIncorrectValueException : ApiException
{
    public PasswordIncorrectValueException(string passwordHash)
        : base(
            errorType: ApplicationErrorType.INCORRECT_VALUE,
            errorField: FieldName.PASSWORD,
            errorServerMessage: "The password is not correct!",
            parameters: new { passwordHash },
            systemException: null
        )
    { }
}

public class ResetPasswordCodeIncorrectValueException : ApiException
{
    public ResetPasswordCodeIncorrectValueException(string resetPasswordCode)
        : base(
            errorType: ApplicationErrorType.INCORRECT_VALUE,
            errorField: FieldName.RESET_PASSWORD_CODE,
            errorServerMessage: "The reset password code is not correct!",
            parameters: new { resetPasswordCode },
            systemException: null
        )
    { }
}

public class ActivationCodeIncorrectValueException : ApiException
{
    public ActivationCodeIncorrectValueException(int activationCode)
        : base(
            errorType: ApplicationErrorType.INCORRECT_VALUE,
            errorField: FieldName.ACTIVATION_CODE,
            errorServerMessage: "The activation code is not correct!",
            parameters: new { activationCode },
            systemException: null
        )
    { }
}
