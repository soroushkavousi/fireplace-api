namespace FireplaceApi.Presentation.Validators;

public class CommentValidator : ApplicationValidator
{
    public Application.Validators.CommentValidator ApplicationValidator { get; set; }

    public CommentValidator(Application.Validators.CommentValidator applicationValidator)
    {
        ApplicationValidator = applicationValidator;
    }
}
