using FireplaceApi.Application.Identifiers;
using FireplaceApi.Presentation.Enums;
using FireplaceApi.Presentation.Exceptions;

namespace FireplaceApi.Presentation.Validators;

public class CommunityValidator : ApplicationValidator
{
    public Application.Validators.CommunityValidator ApplicationValidator { get; set; }

    public CommunityValidator(Application.Validators.CommunityValidator applicationValidator)
    {
        ApplicationValidator = applicationValidator;
    }

    public CommunityIdentifier ValidateEncodedIdOrName(string encodedIdOrName)
    {
        CommunityIdentifier identifier;
        var id = ValidateEncodedIdFormat(encodedIdOrName, ApplicationFieldName.COMMUNITY_ID_OR_NAME, throwException: false);
        if (id.HasValue)
        {
            identifier = CommunityIdentifier.OfId(id.Value);
            return identifier;
        }

        if (ApplicationValidator.ValidateCommunityNameFormat(encodedIdOrName, throwException: false))
        {
            identifier = CommunityIdentifier.OfName(encodedIdOrName);
            return identifier;
        }

        throw new CommunityEncodedIdOrNameInvalidFormatException(encodedIdOrName);
    }
}
