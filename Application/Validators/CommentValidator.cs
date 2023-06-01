namespace FireplaceApi.Application.Validators;

public class CommentValidator : ApplicationValidator
{
    public Domain.Validators.CommentValidator DomainValidator { get; set; }

    public CommentValidator(Domain.Validators.CommentValidator domainValidator)
    {
        DomainValidator = domainValidator;
    }
}
