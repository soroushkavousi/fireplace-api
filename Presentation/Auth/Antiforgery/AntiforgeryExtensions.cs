using FireplaceApi.Domain.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FireplaceApi.Presentation.Auth;

public static class AntiforgeryExtensions
{
    public static IServiceCollection AddApiAntiforgery(this IServiceCollection services)
    {
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

        return services;
    }
}
