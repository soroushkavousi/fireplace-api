using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace FireplaceApi.Presentation.Swagger;

public class SwaggerEnumDescriptionProvider : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        context.ApiDescription.ParameterDescriptions
            .Where(d => d.Source.Id == "Query").ToList()
            .ForEach(param =>
            {
                var swaggerEnumAttributeObject = param.CustomAttributes()
                    .FirstOrDefault(attr => attr.GetType() == typeof(SwaggerEnumAttribute));

                if (swaggerEnumAttributeObject == null)
                    return;

                var swaggerEnumAttribute = (SwaggerEnumAttribute)swaggerEnumAttributeObject;

                var enumType = swaggerEnumAttribute.Type;
                var description = string.Join(", ", Enum.GetNames(enumType));
                description = $"<i>Enum Values:</i> {description}";
                operation.Parameters.First(p => p.Name == param.Name).Description = description;
            });
    }
}
