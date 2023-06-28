using FireplaceApi.Domain.Errors;
using FireplaceApi.Presentation.Errors;
using System.Collections.Generic;

namespace FireplaceApi.Presentation.Validators;

public class ApplicationValidator
{
    public void ValidateFieldIsNotMissing(object value, FieldName field)
    {
        var fieldIsMissing = value switch
        {
            string stringValue => string.IsNullOrWhiteSpace(stringValue),
            _ => value == null,
        };
        if (fieldIsMissing)
        {
            throw field.Name switch
            {
                nameof(FieldName.COMMUNITY_NAME) => new CommunityNameMissingFieldException(),
                nameof(FieldName.POST_CONTENT) => new PostContentMissingFieldException(),
                nameof(FieldName.COMMENT_CONTENT) => new CommentContentMissingFieldException(),
                nameof(FieldName.USERNAME) => new UsernameMissingFieldException(),
                nameof(FieldName.EMAIL_ADDRESS) => new EmailAddressMissingFieldException(),
                nameof(FieldName.ACTIVATION_CODE) => new ActivationCodeMissingFieldException(),
                nameof(FieldName.PASSWORD) => new PasswordMissingFieldException(),
                nameof(FieldName.NEW_PASSWORD) => new NewPasswordMissingFieldException(),
                nameof(FieldName.RESET_PASSWORD_CODE) => new ResetPasswordCodeMissingFieldException(),
                nameof(FieldName.GOOGLE_CODE) => new PasswordMissingFieldException(),
                nameof(FieldName.IS_UPVOTE) => new IsUpvoteMissingFieldException(),
                nameof(FieldName.SEARCH) => new SearchMissingFieldException(),
                _ => new InternalServerException("Field not known in missing fields!"),
            };
        }
    }

    public ulong? ValidateEncodedIdFormat(string encodedId, FieldName field, bool throwException = true)
    {
        try
        {
            var id = encodedId.IdDecode();
            return id;
        }
        catch (ApiException)
        {
            if (throwException)
                throw;
            return default;
        }
    }

    public List<ulong> ValidateIdsFormat(string stringOfEncodedIds)
    {
        if (string.IsNullOrWhiteSpace(stringOfEncodedIds))
            return null;

        List<ulong> ids = new();
        var encodedIds = stringOfEncodedIds.Split(',');
        foreach (var encodedId in encodedIds)
        {
            ids.Add(ValidateEncodedIdFormat(encodedId, PresentationFieldName.COMMUNITY_ID_OR_NAME).Value);
        }
        return ids;
    }
}
