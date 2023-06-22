namespace FireplaceApi.Presentation.Validators;

public class EmailValidator : ApplicationValidator
{
    public Application.Validators.EmailValidator ApplicationValidator { get; set; }

    public EmailValidator(Application.Validators.EmailValidator applicationValidator)
    {
        ApplicationValidator = applicationValidator;
    }
}
