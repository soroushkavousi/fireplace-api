namespace FireplaceApi.Presentation.Validators;

public class PostValidator : ApplicationValidator
{
    public Application.Validators.PostValidator ApplicationValidator { get; set; }

    public PostValidator(Application.Validators.PostValidator applicationValidator)
    {
        ApplicationValidator = applicationValidator;
    }
}
