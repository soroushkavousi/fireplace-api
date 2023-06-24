using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FireplaceApi.Presentation.Swagger;

public class CustomModelDocumentFilter<T> : IDocumentFilter where T : class
{
    private readonly ILogger<CustomModelDocumentFilter<T>> _logger;

    public CustomModelDocumentFilter(ILogger<CustomModelDocumentFilter<T>> logger)
    {
        _logger = logger;
    }

    public void Apply(OpenApiDocument openapiDoc, DocumentFilterContext context)
    {
        context.SchemaGenerator.GenerateSchema(typeof(T), context.SchemaRepository);
    }
}
