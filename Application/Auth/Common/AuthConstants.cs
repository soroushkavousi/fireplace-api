namespace FireplaceApi.Application.Auth;

public static class AuthConstants
{
    public const string AuthorizationHeaderKey = "Authorization";
    public const string AccessTokenHeaderKey = "X-ACCESS-TOKEN";
    public const string AccessTokenCookieKey = "X-ACCESS-TOKEN";
    public const string RequestingUserKey = "User";
    public const string SessionIdClaimKey = "SessionId";
    public const string UserStateClaimKey = "State";
    public const string UnverifiedUserPolicyKey = "UnverifiedUser";
    public const string UserPolicyKey = "User";
}
