namespace FireplaceApi.Presentation.Validators;

public class PostValidator : ApplicationValidator
{
    public Application.Posts.PostValidator ApplicationValidator { get; set; }

    public PostValidator(Application.Posts.PostValidator applicationValidator)
    {
        ApplicationValidator = applicationValidator;
    }
}
