using FireplaceApi.Application.Enums;
using FireplaceApi.Application.Exceptions;
using FireplaceApi.Domain.Identifiers;

namespace FireplaceApi.Application.Validators
{
    public class CommunityValidator : ApplicationValidator
    {
        public Domain.Validators.CommunityValidator DomainValidator { get; set; }

        public CommunityValidator(Domain.Validators.CommunityValidator domainValidator)
        {
            DomainValidator = domainValidator;
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

            if (DomainValidator.ValidateCommunityNameFormat(encodedIdOrName, throwException: false))
            {
                identifier = CommunityIdentifier.OfName(encodedIdOrName);
                return identifier;
            }

            throw new CommunityEncodedIdOrNameInvalidFormatException(encodedIdOrName);
        }
    }
}
