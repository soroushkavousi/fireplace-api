using FireplaceApi.Api.Extensions;
using FireplaceApi.Api.Tools;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ActivateRequestingUserEmailInputBodyParameters
    {
        [Required]
        public int? ActivationCode { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(ActivationCode).ToSnakeCase()] = new OpenApiLong(11111),
        };
    }

    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ResendActivationCodeAsyncInputBodyParameters
    {
        public static IOpenApiAny Example { get; } = new OpenApiObject
        {

        };
    }
}
