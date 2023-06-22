namespace FireplaceApi.Presentation.Validators;

public class ErrorValidator : ApplicationValidator
{
    public Application.Errors.ErrorValidator ApplicationValidator { get; set; }

    public ErrorValidator(Application.Errors.ErrorValidator applicationValidator)
    {
        ApplicationValidator = applicationValidator;
    }
}
