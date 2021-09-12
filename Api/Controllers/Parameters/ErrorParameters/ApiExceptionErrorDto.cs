using Microsoft.OpenApi.Any;
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
    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ApiExceptionErrorDto
    {
        [Required]
        public int? Code { get; set; }
        [Required]
        public string Message { get; set; }

        public static IOpenApiAny ApiExceptionErrorExample1 { get; } = new OpenApiObject
        {
            [nameof(Code).ToSnakeCase()] = new OpenApiInteger(0),
            [nameof(Message).ToSnakeCase()] = new OpenApiString("The error message."),
        };

        public static IOpenApiAny Example { get; } = ApiExceptionErrorExample1;

        //public static Dictionary<string, IOpenApiAny> ActionExamples { get; } = new Dictionary<string, IOpenApiAny>
        //{
        //    [nameof(FileController.PostFileAsync)] = ApiExceptionErrorExample1,
        //};

        public ApiExceptionErrorDto(int? code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
