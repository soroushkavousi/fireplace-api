using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Any;
using FireplaceApi.Api.Extensions;
using FireplaceApi.Api.Tools;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FireplaceApi.Core.Extensions;

namespace FireplaceApi.Api.Controllers
{
    public class ControllerPatchCommunityMembershipByIdInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "id")]
        public long? Id { get; set; }
    }

    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ControllerPatchCommunityMembershipInputBodyParameters
    {
        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            ["nothing"] = new OpenApiString("nothing"),
        };
    }
}
