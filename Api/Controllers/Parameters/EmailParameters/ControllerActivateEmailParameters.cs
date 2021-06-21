using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Any;
using GamingCommunityApi.Api.Extensions;
using GamingCommunityApi.Api.Tools.Swagger.SchemaFilters;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using GamingCommunityApi.Core.Extensions;

namespace GamingCommunityApi.Api.Controllers.Parameters.EmailParameters
{
    public class ControllerActivateEmailInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id")]
        public long? Id { get; set; }
    }

    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ControllerActivateEmailInputBodyParameters
    {
        [Required]
        public int? ActivationCode { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(ActivationCode).ToSnakeCase()] = new OpenApiLong(11111),
        };
    }
}
