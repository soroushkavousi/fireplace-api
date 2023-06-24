using FireplaceApi.Presentation.Tools;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace FireplaceApi.Presentation.Swagger;

public class SwaggerDefaultValueProvider : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        AddDefaultBadRequestExample(operation, context);
        AddDefaultExamples(operation, context);
    }

    public void AddDefaultBadRequestExample(OpenApiOperation operation, OperationFilterContext context)
    {
        var openApiMediaType = new OpenApiMediaType
        {
            Schema = new OpenApiSchema
            {
                Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = Constants.ApiExceptionErrorDtoClassName },
            }
        };
        operation.Responses["400"] = new OpenApiResponse
        {
            Description = "Bad Request",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = openApiMediaType
            }
        };
    }

    public void AddDefaultExamples(OpenApiOperation operation, OperationFilterContext context)
    {
        var apiDescription = context.ApiDescription;
        operation.Deprecated |= apiDescription.IsDeprecated();

        if (operation.Parameters == null)
        {
            return;
        }

        // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/412
        // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/pull/413
        foreach (var parameter in operation.Parameters)
        {
            var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

            if (parameter.Description == null)
            {
                parameter.Description = description.ModelMetadata?.Description;
            }

            if (parameter.Schema.Default == null && description.DefaultValue != null)
            {
                parameter.Schema.Default = new OpenApiString(description.DefaultValue.ToString());
            }
            parameter.Required |= description.IsRequired;
        }
    }

    // Just for knowledge

    //var openApiObject = new OpenApiObject();
    //foreach (var example in ApiExceptionErrorOutputBodyDto.Examples)
    //{
    //    openApiObject[example.Key.ToSnakeCase()] = example.Value;
    //}

    //Type = "object",
    //Properties = new Dictionary<string, OpenApiSchema>
    //{
    //    ["code"] = new OpenApiSchema { Format = "int32", Type = "integer"},
    //    ["message"] = new OpenApiSchema { Type = "string" },
    //    ["field"] = new OpenApiSchema { Type = "string" },
    //},
    //Example = new OpenApiObject
    //{
    //    ["code"] = new OpenApiInteger(0),
    //    ["message"] = new OpenApiString("error message"),
    //    ["field"] = new OpenApiString("the_field"),
    //},
    //Required = new SortedSet<string> { "code", "message"}
}