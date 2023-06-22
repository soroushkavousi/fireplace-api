namespace FireplaceApi.Presentation.Validators;

public class CommentValidator : ApplicationValidator
{
    public Application.Comments.CommentValidator ApplicationValidator { get; set; }

    public CommentValidator(Application.Comments.CommentValidator applicationValidator)
    {
        ApplicationValidator = applicationValidator;
    }
}
