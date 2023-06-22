namespace FireplaceApi.Presentation.Validators;

public class SessionValidator : ApplicationValidator
{
    public Application.Validators.SessionValidator ApplicationValidator { get; set; }

    public SessionValidator(Application.Validators.SessionValidator applicationValidator)
    {
        ApplicationValidator = applicationValidator;
    }
}
