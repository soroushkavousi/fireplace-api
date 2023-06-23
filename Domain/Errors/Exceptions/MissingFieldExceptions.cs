namespace FireplaceApi.Domain.Errors;

public abstract class MissingFieldException : ApiException
{
    public MissingFieldException(FieldName errorField)
        : base(
            errorType: ErrorType.MISSING_FIELD,
            errorField: errorField,
            errorServerMessage: $"Field {errorField.Name} is missing!",
            parameters: null,
            systemException: null
        )
    { }
}

public class CommunityNameMissingFieldException : MissingFieldException
{
    public CommunityNameMissingFieldException()
        : base(
              errorField: FieldName.COMMUNITY_NAME
        )
    { }
}

public class PostContentMissingFieldException : MissingFieldException
{
    public PostContentMissingFieldException()
        : base(
            errorField: FieldName.POST_CONTENT
        )
    { }
}

public class CommentContentMissingFieldException : MissingFieldException
{
    public CommentContentMissingFieldException()
        : base(
              errorField: FieldName.COMMENT_CONTENT
        )
    { }
}

public class UsernameMissingFieldException : MissingFieldException
{
    public UsernameMissingFieldException()
        : base(
            errorField: FieldName.USERNAME
        )
    { }
}

public class EmailAddressMissingFieldException : MissingFieldException
{
    public EmailAddressMissingFieldException()
        : base(
            errorField: FieldName.EMAIL_ADDRESS
        )
    { }
}

public class ActivationCodeMissingFieldException : MissingFieldException
{
    public ActivationCodeMissingFieldException()
        : base(
            errorField: FieldName.ACTIVATION_CODE
        )
    { }
}

public class PasswordMissingFieldException : MissingFieldException
{
    public PasswordMissingFieldException()
        : base(
            errorField: FieldName.PASSWORD
        )
    { }
}

public class NewPasswordMissingFieldException : MissingFieldException
{
    public NewPasswordMissingFieldException()
        : base(
            errorField: FieldName.NEW_PASSWORD
        )
    { }
}

public class ResetPasswordCodeMissingFieldException : MissingFieldException
{
    public ResetPasswordCodeMissingFieldException()
        : base(
            errorField: FieldName.RESET_PASSWORD_CODE
        )
    { }
}

public class GoogleCodeMissingFieldException : MissingFieldException
{
    public GoogleCodeMissingFieldException()
        : base(
            errorField: FieldName.GOOGLE_CODE
        )
    { }
}

public class IsUpvoteMissingFieldException : MissingFieldException
{
    public IsUpvoteMissingFieldException()
        : base(
            errorField: FieldName.IS_UPVOTE
        )
    { }
}

public class SearchMissingFieldException : MissingFieldException
{
    public SearchMissingFieldException()
        : base(
            errorField: FieldName.SEARCH
        )
    { }
}
