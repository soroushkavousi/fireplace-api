namespace FireplaceApi.Application.Validators
{
    public class CommunityMembershipValidator : ApplicationValidator
    {
        public Domain.Validators.CommunityMembershipValidator DomainValidator { get; set; }

        public CommunityMembershipValidator(Domain.Validators.CommunityMembershipValidator domainValidator)
        {
            DomainValidator = domainValidator;
        }
    }
}
