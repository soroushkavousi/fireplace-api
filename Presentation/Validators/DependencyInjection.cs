using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace FireplaceApi.Presentation.Validators;

public static class DependencyInjection
{
    public static void AddValidators(this IServiceCollection services)
    {
        services.AddScoped<CommentValidator>();
        services.AddScoped<EmailValidator>();
        services.AddScoped<ErrorValidator>();
        services.AddScoped<FileValidator>();
        services.AddScoped<PostValidator>();
        services.AddScoped<SessionValidator>();
        services.AddScoped<UserValidator>();

        // Suppress default validation
        services.AddSingleton<IObjectModelValidator, NullObjectModelValidator>();
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
            options.SuppressMapClientErrors = true;
        });
    }
}
