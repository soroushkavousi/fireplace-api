using FireplaceApi.Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Auth;

public static class CookieExtensions
{
    public static AuthenticationBuilder AddApiCookieAuthentication(this AuthenticationBuilder builder)
    {
        return builder.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
        {
            o.Cookie.Name = AuthConstants.AccessTokenCookieKey;
            o.SlidingExpiration = true;
            o.ExpireTimeSpan = TimeSpan.FromDays(Configs.Current.Api.CookieMaxAgeInDays);
            o.Events.OnRedirectToLogin = redirectContext => { return Task.CompletedTask; };
            o.Events.OnRedirectToAccessDenied = redirectContext => { return Task.CompletedTask; };
        });
    }

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
