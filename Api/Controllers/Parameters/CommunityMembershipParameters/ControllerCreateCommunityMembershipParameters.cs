using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Any;
using FireplaceApi.Api.Extensions;
using FireplaceApi.Api.Interfaces;
using FireplaceApi.Api.Tools;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FireplaceApi.Core.Extensions;

namespace FireplaceApi.Api.Controllers
{
    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ControllerCreateCommunityMembershipInputBodyParameters
    {
        public long? CommunityId { get; set; }
        public string CommunityName { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(CommunityName).ToSnakeCase()] = CommunityDto.PureExample1[nameof(CommunityDto.Name).ToSnakeCase()],
        };
    }
}
