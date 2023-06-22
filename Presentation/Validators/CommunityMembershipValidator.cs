namespace FireplaceApi.Presentation.Validators;

public class CommunityMembershipValidator : ApplicationValidator
{
    public Application.Communities.CommunityMembershipValidator ApplicationValidator { get; set; }

    public CommunityMembershipValidator(Application.Communities.CommunityMembershipValidator applicationValidator)
    {
        ApplicationValidator = applicationValidator;
    }
}
