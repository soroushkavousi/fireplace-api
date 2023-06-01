namespace FireplaceApi.Application.Validators;

public class ErrorValidator : ApplicationValidator
{
    public Domain.Validators.ErrorValidator DomainValidator { get; set; }

    public ErrorValidator(Domain.Validators.ErrorValidator domainValidator)
    {
        DomainValidator = domainValidator;
    }
}
