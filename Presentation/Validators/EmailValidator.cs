namespace FireplaceApi.Presentation.Validators;

public class EmailValidator : ApplicationValidator
{
    public Application.Emails.EmailValidator ApplicationValidator { get; set; }

    public EmailValidator(Application.Emails.EmailValidator applicationValidator)
    {
        ApplicationValidator = applicationValidator;
    }
}
