namespace FireplaceApi.Presentation.Validators;

public class FileValidator : ApplicationValidator
{
    public Application.Files.FileValidator ApplicationValidator { get; set; }

    public FileValidator(Application.Files.FileValidator applicationValidator)
    {
        ApplicationValidator = applicationValidator;
    }
}
