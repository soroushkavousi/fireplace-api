namespace FireplaceApi.Application.Validators;

public class FileValidator : ApplicationValidator
{
    public Domain.Validators.FileValidator DomainValidator { get; set; }

    public FileValidator(Domain.Validators.FileValidator domainValidator)
    {
        DomainValidator = domainValidator;
    }
}
