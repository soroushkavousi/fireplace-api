using FireplaceApi.Presentation.Enums;
using FireplaceApi.Presentation.Exceptions;
using FireplaceApi.Application.Enums;
using FireplaceApi.Application.Exceptions;
using FireplaceApi.Application.Tools;
using System;
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

    public TEnum? ValidateInputEnum<TEnum>(string inputString) where TEnum : struct
    {
        if (string.IsNullOrWhiteSpace(inputString))
            return default;

        if (!Enum.TryParse(inputString, true, out TEnum result))
        {
            throw typeof(TEnum).Name switch
            {
                nameof(SortType) => new SortIncorrectValueException(inputString),
                nameof(CommunitySortType) => new CommunitySortIncorrectValueException(inputString),
                _ => new InternalServerException("Not known enum type!"),
            };
        }

        return result;
    }

    public ulong? ValidateEncodedIdFormat(string encodedId, FieldName field, bool throwException = true)
    {
        if (IdGenerator.IsEncodedIdFormatValid(encodedId))
            return encodedId.IdDecode();

        if (throwException)
        {
            throw field.Name switch
            {
                nameof(FieldName.POST_ID) => new PostEncodedIdInvalidFormatException(encodedId),
                nameof(FieldName.COMMENT_ID) => new CommentEncodedIdInvalidFormatException(encodedId),
                _ => new InternalServerException("Not known encoded id field!"),
            };
        }
        return default;
    }

    public List<ulong> ValidateIdsFormat(string stringOfEncodedIds)
    {
        if (string.IsNullOrWhiteSpace(stringOfEncodedIds))
            return null;

        List<ulong> ids = new();
        var encodedIds = stringOfEncodedIds.Split(',');
        foreach (var encodedId in encodedIds)
        {
            ids.Add(ValidateEncodedIdFormat(encodedId, ApplicationFieldName.COMMUNITY_ID_OR_NAME).Value);
        }
        return ids;
    }
}
