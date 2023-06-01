namespace FireplaceApi.Application.Validators;

public class SessionValidator : ApplicationValidator
{
    public Domain.Validators.SessionValidator DomainValidator { get; set; }

    public SessionValidator(Domain.Validators.SessionValidator domainValidator)
    {
        DomainValidator = domainValidator;
    }
}
