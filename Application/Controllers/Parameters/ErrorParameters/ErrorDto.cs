using FireplaceApi.Application.Extensions;
using FireplaceApi.Application.Tools;
using FireplaceApi.Domain.Enums;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Application.Controllers;

[SwaggerSchemaFilter(typeof(TypeExampleProvider))]
public class ErrorDto
{
    [Required]
    public int Code { get; set; }
    [Required]
    public string Type { get; set; }
    [Required]
    public string Field { get; set; }
    [Required]
    public string Message { get; set; }
    [Required]
    public int HttpStatusCode { get; set; }

    public static OpenApiObject PureInternalServerErrorExample { get; } = new OpenApiObject
    {
        [nameof(Type).ToSnakeCase()] = new OpenApiString(ErrorType.INTERNAL_SERVER.ToString()),
        [nameof(Field).ToSnakeCase()] = new OpenApiString(FieldName.GENERAL.ToString()),
    };

    public static OpenApiObject PurePasswordErrorExample { get; } = new OpenApiObject
    {
        [nameof(Type).ToSnakeCase()] = new OpenApiString(ErrorType.INVALID_FORMAT.ToString()),
        [nameof(Field).ToSnakeCase()] = new OpenApiString(FieldName.PASSWORD.ToString()),
    };

    public static OpenApiArray ListOfPureErrorsExample1 { get; } = new OpenApiArray
    {
        PureInternalServerErrorExample, PurePasswordErrorExample
    };

    public static OpenApiObject Example { get; } = PureInternalServerErrorExample;
    public static Dictionary<string, IOpenApiAny> ActionExamples { get; } = new Dictionary<string, IOpenApiAny>
    {
        //[nameof(ErrorController.ListErrorsAsync)] = ListOfPureErrorsExample1,
        [nameof(ErrorController.GetErrorByCodeAsync)] = PurePasswordErrorExample,
        //[nameof(ErrorController.PatchErrorAsync)] = PurePasswordErrorExample,
    };

    static ErrorDto()
    {

    }

    public ErrorDto(int code, string type, string field, string message,
        int httpStatusCode)
    {
        Code = code;
        Type = type;
        Field = field;
        Message = message;
        HttpStatusCode = httpStatusCode;
    }
}
