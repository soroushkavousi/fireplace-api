using FireplaceApi.Presentation.Dtos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Reflection;

namespace FireplaceApi.Presentation.Tools;

public static class SwaggerDependencyInjection
{
    public static void AddApiSwagger(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen(options =>
        {
            options.EnableAnnotations(false, true);
            options.OperationFilter<SwaggerDefaultValueProvider>();
            options.OperationFilter<ActionResponseExampleProvider>();
            options.OperationFilter<SwaggerEnumDescriptionProvider>();
            options.DocumentFilter<CustomModelDocumentFilter<ApiExceptionErrorDto>>();
            options.DocumentFilter<OrderDocumentFilter>();
            options.AddValueObjectMappers();

            // Enable xml
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);

            options.AddSecurityDefinition("Bearer",
                new OpenApiSecurityScheme
                {
                    Description = "Authorization header using the Bearer scheme." +
                        " <br/> <br/> Just put your **access token** as the value. <br/>",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    In = ParameterLocation.Header,
                });
            options.OperationFilter<SwaggerSecurityConfigurator>();
        });
    }
}
