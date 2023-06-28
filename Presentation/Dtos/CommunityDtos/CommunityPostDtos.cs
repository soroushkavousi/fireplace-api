using FireplaceApi.Domain.Communities;
using FireplaceApi.Infrastructure.Serializers;
using FireplaceApi.Presentation.Swagger;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Presentation.Dtos;

[SwaggerSchemaFilter(typeof(TypeExampleProvider))]
public class CreateCommunityInputBodyDto
{
    [Required]
    public CommunityName Name { get; set; }

    public static IOpenApiAny Example { get; } = new OpenApiObject
    {
        [nameof(Name).ToSnakeCase()] = CommunityDto.PureExample1[nameof(CommunityDto.Name).ToSnakeCase()],
    };
}
