using FireplaceApi.Api.Extensions;
using FireplaceApi.Api.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    public class PatchErrorInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "code")]
        public int Code { get; set; }
    }

    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class PatchErrorInputBodyParameters
    {
        public string Message { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(Message).ToSnakeCase()] = new OpenApiString("The new error message."),
        };
    }
}
