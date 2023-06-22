using FireplaceApi.Presentation.Controllers;
using FireplaceApi.Presentation.Extensions;
using FireplaceApi.Presentation.Tools;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Presentation.Dtos;

[SwaggerSchemaFilter(typeof(TypeExampleProvider))]
public class FileDto
{
    [Required]
    public string Id { get; set; }
    [Required]
    public string Url { get; set; }
    [Required]
    public DateTime CreationDate { get; set; }

    public static OpenApiObject PureExample1 { get; } = new OpenApiObject
    {
        [nameof(Id).ToSnakeCase()] = new OpenApiString("DzqWK7MRS1H"),
        [nameof(Url).ToSnakeCase()] = new OpenApiString("https://files.social-media.bitiano.com/xww03krwo1e3.jpg"),
        [nameof(CreationDate).ToSnakeCase()] = new OpenApiDateTime(Utils.GetYesterdayDate()),
    };
    public static OpenApiObject Example1 { get; } = new OpenApiObject
    {
        [nameof(Id).ToSnakeCase()] = PureExample1[nameof(Id).ToSnakeCase()],
        [nameof(Url).ToSnakeCase()] = PureExample1[nameof(Url).ToSnakeCase()],
        [nameof(CreationDate).ToSnakeCase()] = new OpenApiDateTime(Utils.GetYesterdayDate()),
    };

    public static Dictionary<string, IOpenApiAny> ActionExamples { get; } = new Dictionary<string, IOpenApiAny>
    {
        [nameof(FileController.PostFileAsync)] = Example1,
    };

    public static OpenApiObject Example { get; } = Example1;

    static FileDto()
    {

    }

    public FileDto(string id, string url, DateTime creationDate)
    {
        Id = id;
        Url = url;
        CreationDate = creationDate;
    }
}
