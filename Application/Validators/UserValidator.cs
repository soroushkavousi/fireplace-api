using FireplaceApi.Application.Enums;
using FireplaceApi.Application.Exceptions;
using FireplaceApi.Domain.Identifiers;

namespace FireplaceApi.Application.Validators
{
    public class UserValidator : ApplicationValidator
    {
        public Domain.Validators.UserValidator DomainValidator { get; set; }

        public UserValidator(Domain.Validators.UserValidator domainValidator)
        {
            DomainValidator = domainValidator;
        }

        public UserIdentifier ValidateEncodedIdOrUsername(string encodedIdOrUsername)
        {
            UserIdentifier identifier;
            var id = ValidateEncodedIdFormat(encodedIdOrUsername, ApplicationFieldName.USER_ID_OR_USERNAME, throwException: false);
            if (id.HasValue)
            {
                identifier = UserIdentifier.OfId(id.Value);
                return identifier;
            }

            if (DomainValidator.ValidateUsernameFormat(encodedIdOrUsername, throwException: false))
            {
                identifier = UserIdentifier.OfUsername(encodedIdOrUsername);
                return identifier;
            }

            throw new UserEncodedIdOrUsernameInvalidFormatException(encodedIdOrUsername);
        }
    }
}
