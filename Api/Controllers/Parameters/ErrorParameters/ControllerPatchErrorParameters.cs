﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Any;
using GamingCommunityApi.Api.Tools.Swagger.SchemaFilters;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using GamingCommunityApi.Core.Extensions;

namespace GamingCommunityApi.Api.Controllers.Parameters.ErrorParameters
{
    public class ControllerPatchErrorInputRouteParameters
    {
        [Required]
        [FromRoute(Name = "code")]
        public int? Code { get; set; }
    }

    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ControllerPatchErrorInputBodyParameters
    {
        public string Message { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(Message).ToSnakeCase()] = new OpenApiString("The new error message."),
        };
    }
}