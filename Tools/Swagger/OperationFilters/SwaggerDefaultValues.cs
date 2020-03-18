using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using GamingCommunityApi.Controllers.Parameters.ErrorParameters;
using GamingCommunityApi.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;

namespace GamingCommunityApi.Tools.Swagger.OperationFilters
{
    /// <summary>
    /// Represents the Swagger/Swashbuckle operation filter used to document the implicit API version parameter.
    /// </summary>
    /// <remarks>This <see cref="IOperationFilter"/> is only required due to bugs in the <see cref="SwaggerGenerator"/>.
    /// Once they are fixed and published, this class can be removed.</remarks>
    public class SwaggerDefaultValues : IOperationFilter
    {
        public static OpenApiSecurityRequirement SecurityRequirement { get; } = new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer", //The name of the previously defined security scheme.
                        Type = ReferenceType.SecurityScheme,
                    },
                },
                new List<string>()
            }
        };
        /// <summary>
        /// Applies the filter to the specified operation using the given context.
        /// </summary>
        /// <param name="operation">The operation to apply the filter to.</param>
        /// <param name="context">The current operation filter context.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            AddDefaultBadRequestExample(operation, context);
            AddDefaultExamples(operation, context);
            AddSecuritySign(operation, context);
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

        public void AddSecuritySign(OpenApiOperation operation, OperationFilterContext context)
        {
            context.ApiDescription.TryGetMethodInfo(out var methodInfo);

            if (methodInfo == null)
                return;

            if (methodInfo.MemberType != MemberTypes.Method)
                return;

            var hasAllowAnonymousAttribute = methodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();
            var actionName = methodInfo.Name;

            if (hasAllowAnonymousAttribute)
                return;

            operation.Security = new List<OpenApiSecurityRequirement> { SecurityRequirement };

        //var x = new OpenApiSecurityRequirement
        //    {
        //        {
        //            new OpenApiSecurityScheme
        //            {
        //                Reference = new OpenApiReference
        //                {
        //                    Id = "Bearer", //The name of the previously defined security scheme.
        //                    Type = ReferenceType.SecurityScheme,
        //                },
        //            },
        //            new List<string>()
        //        }
        //    };
        //    options.AddSecurityRequirement(new OpenApiSecurityRequirement
        //    {
        //        {
        //            new OpenApiSecurityScheme
        //            {
        //                Reference = new OpenApiReference
        //                {
        //                    Id = "Bearer", //The name of the previously defined security scheme.
        //                    Type = ReferenceType.SecurityScheme,
        //                },
        //            },
        //            new List<string>()
        //        }
        //    });
            //operation.Security = new List<IDictionary<string, IEnumerable<string>>>();
            //operation.Security.Add(new Dictionary<string, IEnumerable<string>>
            //{
            //    { "Bearer", new string[] { } }
            //});
        }


        // Just for knowledge

        //var openApiObject = new OpenApiObject();
        //foreach (var example in ApiExceptionErrorOutputBodyParameters.Examples)
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
        //    ["message"] = new OpenApiString("پیام خطا"),
        //    ["field"] = new OpenApiString("the_field"),
        //},
        //Required = new SortedSet<string> { "code", "message"}
    }
}