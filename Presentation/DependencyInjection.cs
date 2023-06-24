using FireplaceApi.Presentation.Auth;
using FireplaceApi.Presentation.Tools;
using FireplaceApi.Presentation.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace FireplaceApi.Presentation;

public static class DependencyInjection
{
    public static void AddPresentation(this IServiceCollection services)
    {
        services.AddUtilities();
        services.AddControllers();
        services.AddGraphQL();
        services.AddHostedServices();
        services.AddAuth();
        services.AddApiSwagger();
        services.AddValidators();
        services.AddVersioning();
    }

    private static void AddUtilities(this IServiceCollection services)
    {
        // Give access to HttpContext
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        // Make url lowercase
        services.AddRouting(options => options.LowercaseUrls = true);

        // Add ulong support
        services.AddRouting(options =>
        {
            options.ConstraintMap.Add(UlongRouteConstraint.Name, typeof(UlongRouteConstraint));
        });
    }

    private static void AddHostedServices(this IServiceCollection services)
    {
        // Add a hosted service to check the API readiness
        services.AddHostedService<ReadinessCheckerService>();
    }

    private static void AddVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
        });

        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
    }
}
