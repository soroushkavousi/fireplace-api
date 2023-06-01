using FireplaceApi.Application.Controllers;
using FireplaceApi.Application.Enums;
using FireplaceApi.Application.Extensions;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Identifiers;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Operators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Tools;

public class TypeExampleProvider : ISchemaFilter
{
    private readonly ILogger<TypeExampleProvider> _logger;
    private readonly IServiceProvider _serviceProvider;

    public TypeExampleProvider(ILogger<TypeExampleProvider> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var example = context.Type.ExtractExample();

        if (context.Type.Name.Contains("Error") && example.ContainsKey("message") == false)
            example = FillErrorExample(example, _serviceProvider, _logger).GetAwaiter().GetResult();

        schema.Example = example;
        schema.Default = example;
    }

    public static async Task<OpenApiObject> FillErrorExample<T>(OpenApiObject errorExample,
        IServiceProvider serviceProvider, ILogger<T> logger)
    {
        var type = ApplicationErrorType.FromName(errorExample[nameof(ApiExceptionErrorDto.Type).ToSnakeCase()].To<OpenApiString>().Value);
        var field = ApplicationFieldName.FromName(errorExample[nameof(ApiExceptionErrorDto.Field).ToSnakeCase()].To<OpenApiString>().Value);
        var errorIdentifier = ErrorIdentifier.OfTypeAndField(type, field);
        Error error = null;
        using (var scope = serviceProvider.CreateScope())
        {
            try
            {
                var errorOperator = scope.ServiceProvider.GetService<ErrorOperator>();
                error = await errorOperator.GetErrorAsync(errorIdentifier);
            }
            catch (Exception ex)
            {
                logger.LogAppInformation($"Exception occurred when trying to get the error details for examples.",
                    parameters: new { type, field, ErrorMessage = ex.Message });
            }
            finally
            {
                if (error == null)
                {
                    logger.LogAppInformation($"Error seems does not exists yet or an exception occurred.",
                        parameters: new { type, field });
                    error = Error.InternalServerError;
                }
            }
        }
        errorExample["code"] = new OpenApiInteger(error.Code);
        errorExample["message"] = new OpenApiString(error.ClientMessage);
        return errorExample;
    }
}
