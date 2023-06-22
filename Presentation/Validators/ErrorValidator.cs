namespace FireplaceApi.Presentation.Validators;

public class ErrorValidator : ApplicationValidator
{
    public Application.Validators.ErrorValidator ApplicationValidator { get; set; }

    public ErrorValidator(Application.Validators.ErrorValidator applicationValidator)
    {
        ApplicationValidator = applicationValidator;
    }
}
