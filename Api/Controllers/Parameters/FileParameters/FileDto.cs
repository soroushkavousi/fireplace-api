using Microsoft.OpenApi.Any;
using FireplaceApi.Api.Extensions;
using FireplaceApi.Api.Interfaces;
using FireplaceApi.Api.Tools.Swagger;
using FireplaceApi.Api.Tools.Swagger.SchemaFilters;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FireplaceApi.Core.Extensions;

namespace FireplaceApi.Api.Controllers.Parameters.FileParameters
{
    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class FileDto
    {
        [Required]
        public long? Id { get; set; }
        [Required]
        public string Url { get; set; }

        public static OpenApiObject PureFileExample1 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = new OpenApiInteger(1000000),
            [nameof(Url).ToSnakeCase()] = new OpenApiString("https://files.social-media.bitiano.com/xww03krwo1e3.jpg"),
        };
        public static OpenApiObject FileExample1 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = PureFileExample1[nameof(Id).ToSnakeCase()],
            [nameof(Url).ToSnakeCase()] = PureFileExample1[nameof(Url).ToSnakeCase()],
        };

        public static Dictionary<string, IOpenApiAny> ActionExamples { get; } = new Dictionary<string, IOpenApiAny>
        {
            [nameof(FileController.PostFileAsync)] = FileExample1,
        };

        public static OpenApiObject Example { get; } = FileExample1;

        static FileDto()
        {

        }

        public FileDto(long? id, string url)
        {
            Id = id;
            Url = url;
        }
    }
}
