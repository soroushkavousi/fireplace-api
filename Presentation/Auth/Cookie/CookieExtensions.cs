using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FireplaceApi.Presentation.Auth;

public static class CookieExtensions
{
    public static async Task SignInWithCookieAsync(this HttpContext httpContext,
        RequestingUser futureRequestingUser)
    {
        var claimsIdentity = new ClaimsIdentity(futureRequestingUser.ToClaims(),
            CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
        };

        await httpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity), authProperties);
    }
}
