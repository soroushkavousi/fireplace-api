using FireplaceApi.Domain.Users;
using FireplaceApi.Presentation.Enums;
using FireplaceApi.Presentation.Exceptions;

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

        if (ApplicationValidator.ValidateUsernameFormat(encodedIdOrUsername, throwException: false))
        {
            identifier = UserIdentifier.OfUsername(encodedIdOrUsername);
            return identifier;
        }

        throw new UserEncodedIdOrUsernameInvalidFormatException(encodedIdOrUsername);
    }
}
