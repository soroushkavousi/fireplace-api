using FireplaceApi.Domain.Users;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FireplaceApi.Presentation.Swagger;

public static class SwaggerMappers
{
    public static void AddValueObjectMappers(this SwaggerGenOptions options)
    {
        options.MapType<Username>(() => new OpenApiSchema { Type = typeof(string).Name });
    }
}
