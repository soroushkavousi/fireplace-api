using FireplaceApi.Presentation.Attributes;
using FireplaceApi.Presentation.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace FireplaceApi.Presentation.Controllers;

public static class DependencyInjection
{
    public static void AddControllers(this IServiceCollection services)
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
            options.JsonSerializerOptions.AddApplicationOptions();
        });
    }
}
