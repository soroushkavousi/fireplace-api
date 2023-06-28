﻿using FireplaceApi.Domain.Users;
using FireplaceApi.Presentation.Errors;

namespace FireplaceApi.Presentation.Validators;

public class UserValidator : ApplicationValidator
{
    public Application.Users.UserValidator ApplicationValidator { get; set; }

    public UserValidator(Application.Users.UserValidator applicationValidator)
    {
        ApplicationValidator = applicationValidator;
    }

    public UserIdentifier ValidateEncodedIdOrUsername(string encodedIdOrUsername)
    {
        UserIdentifier identifier;
        var id = ValidateEncodedIdFormat(encodedIdOrUsername, PresentationFieldName.USER_ID_OR_USERNAME, throwException: false);
        if (id.HasValue)
        {
            identifier = UserIdentifier.OfId(id.Value);
            return identifier;
        }

        try
        {
            var username = new Username(encodedIdOrUsername);
            identifier = UserIdentifier.OfUsername(username);
            return identifier;
        }
        catch { }

        throw new UserEncodedIdOrUsernameInvalidFormatException(encodedIdOrUsername);
    }
}