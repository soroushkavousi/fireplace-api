using FireplaceApi.Presentation.Attributes;
using FireplaceApi.Presentation.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace FireplaceApi.Presentation.Controllers;

public static class DependencyInjection
{
    public static void AddApiControllers(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<RequestingUserInjectorAttribute>();
            options.Filters.Add<InputHeaderDtoInjectorAttribute>();
            options.Filters.Add<InputCookieDtoInjectorAttribute>();
            options.Filters.Add<ActionLoggingAttribute>();
            options.Filters.Add<ActionInputValidatorAttribute>();
        }).AddJsonOptions(options =>
        {
            // Config serializers
            options.JsonSerializerOptions.AddCommonOptions();
        });

        // Make url lowercase
        services.AddRouting(options => options.LowercaseUrls = true);

        // Add ulong support
        services.AddRouting(options =>
        {
            options.ConstraintMap.Add(UlongRouteConstraint.Name, typeof(UlongRouteConstraint));
        });
    }
}
