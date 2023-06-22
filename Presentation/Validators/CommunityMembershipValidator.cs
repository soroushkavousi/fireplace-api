namespace FireplaceApi.Presentation.Validators;

public class CommunityMembershipValidator : ApplicationValidator
{
    public Application.Validators.CommunityMembershipValidator ApplicationValidator { get; set; }

    public CommunityMembershipValidator(Application.Validators.CommunityMembershipValidator applicationValidator)
    {
        ApplicationValidator = applicationValidator;
    }
}
