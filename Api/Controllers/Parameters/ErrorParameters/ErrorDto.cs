using Microsoft.OpenApi.Any;
using GamingCommunityApi.Api.Extensions;
using GamingCommunityApi.Api.Interfaces;
using GamingCommunityApi.Api.Tools.Swagger;
using GamingCommunityApi.Api.Tools.Swagger.SchemaFilters;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GamingCommunityApi.Core.Enums;

namespace GamingCommunityApi.Api.Controllers.Parameters.ErrorParameters
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
