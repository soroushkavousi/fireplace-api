namespace FireplaceApi.Presentation.Validators;

public class SessionValidator : ApplicationValidator
{
    public Application.Sessions.SessionValidator ApplicationValidator { get; set; }

    public SessionValidator(Application.Sessions.SessionValidator applicationValidator)
    {
        ApplicationValidator = applicationValidator;
    }
}
