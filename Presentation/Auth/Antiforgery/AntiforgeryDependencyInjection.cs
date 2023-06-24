using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FireplaceApi.Presentation.Auth;

public static class AntiforgeryDependencyInjection
{
    public static void AddApiAntiforgery(this IServiceCollection services)
    {
        services.AddScoped<IAuthorizationHandler, AntiforgeryAuthorizationHandler>();
        services.AddAntiforgery(options =>
        {
            options.Cookie = new CookieBuilder
            {
                Name = AntiforgeryConstants.CsrfTokenKey,
                MaxAge = new TimeSpan(Configs.Current.Api.CookieMaxAgeInDays, 0, 0, 0),
                IsEssential = true,
            };
            options.HeaderName = AntiforgeryConstants.CsrfTokenKey;
            options.Cookie.Name = AntiforgeryConstants.CsrfTokenKey;
        });
    }
}
