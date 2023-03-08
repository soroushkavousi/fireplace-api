namespace FireplaceApi.Application.Validators
{
    public class EmailValidator : ApplicationValidator
    {
        public Domain.Validators.EmailValidator DomainValidator { get; set; }

        public EmailValidator(Domain.Validators.EmailValidator domainValidator)
        {
            DomainValidator = domainValidator;
        }
    }
}
