using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Presentation.Auth;

public static class CookieDependencyInjection
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
}
