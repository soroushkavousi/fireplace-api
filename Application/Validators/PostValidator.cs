namespace FireplaceApi.Application.Validators
{
    public class PostValidator : ApplicationValidator
    {
        public Domain.Validators.PostValidator DomainValidator { get; set; }

        public PostValidator(Domain.Validators.PostValidator domainValidator)
        {
            DomainValidator = domainValidator;
        }
    }
}
