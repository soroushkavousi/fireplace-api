﻿using Microsoft.OpenApi.Any;
using GamingCommunityApi.Enums;
using GamingCommunityApi.Extensions;
using GamingCommunityApi.Interfaces;
using GamingCommunityApi.Tools.Swagger;
using GamingCommunityApi.Tools.Swagger.SchemaFilters;
using GamingCommunityApi.Tools.TextJsonSerializer;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GamingCommunityApi.Controllers.Parameters.ErrorParameters
{
    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ErrorDto
    {
        [Required]
        public int? Code { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public int? HttpStatusCode { get; set; }

        public static OpenApiObject PureInternalServerErrorExample { get; } = new OpenApiObject
        {
            ["name"] = new OpenApiString(ErrorName.INTERNAL_SERVER.ToString()),
        };

        public static OpenApiObject PurePasswordMinimumLengthErrorExample { get; } = new OpenApiObject
        {
            ["name"] = new OpenApiString(ErrorName.PASSWORD_MIN_LENGTH.ToString()),
        };

        public static OpenApiArray ListOfPureErrorsExample1 { get; } = new OpenApiArray
        {
            PureInternalServerErrorExample, PurePasswordMinimumLengthErrorExample
        };

        public static OpenApiObject Example { get; } = PureInternalServerErrorExample;
        public static Dictionary<string, IOpenApiAny> ActionExamples { get; } = new Dictionary<string, IOpenApiAny>
        {
            [nameof(ErrorController.ListErrorsAsync)] = ListOfPureErrorsExample1,
            [nameof(ErrorController.GetErrorByCodeAsync)] = PurePasswordMinimumLengthErrorExample,
            [nameof(ErrorController.PatchErrorAsync)] = PurePasswordMinimumLengthErrorExample,
        };

        static ErrorDto()
        {

        }

        public ErrorDto(int? code, string message, int? httpStatusCode)
        {
            Code = code;
            Message = message;
            HttpStatusCode = httpStatusCode;
        }
    }
}
