using Microsoft.AspNetCore.Http;

namespace FireplaceApi.Presentation.Auth;

public static class AuthExtensions
{
    public static RequestingUser GetRequestingUser(this HttpContext httpContext)
    {
        var hasUser = httpContext.Items.TryGetValue(AuthConstants.RequestingUserKey, out var userObject);
        if (hasUser)
            return userObject.To<RequestingUser>();

        var requestingUser = httpContext.User?.ToRequestingUser();
        httpContext.Items[AuthConstants.RequestingUserKey] = requestingUser;
        return requestingUser;
    }
}
