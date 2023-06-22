namespace FireplaceApi.Presentation.Validators;

public class FileValidator : ApplicationValidator
{
    public Application.Validators.FileValidator ApplicationValidator { get; set; }

    public FileValidator(Application.Validators.FileValidator applicationValidator)
    {
        ApplicationValidator = applicationValidator;
    }
}
