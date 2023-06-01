using FireplaceApi.Application.Extensions;
using FireplaceApi.Application.Tools;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FireplaceApi.Application.Controllers;

[SwaggerSchemaFilter(typeof(TypeExampleProvider))]
public class QueryResultDto<T>
{
    [Required]
    [JsonPropertyName("items")]
    [JsonPropertyOrder(99)]
    public List<T> Items { get; set; }
    [Required]
    [JsonPropertyName("more_item_ids")]
    [JsonPropertyOrder(100)]
    public List<string> MoreItemIds { get; set; }


    public static OpenApiObject PureExample1 { get; } = new OpenApiObject
    {
        [nameof(MoreItemIds).ToSnakeCase()] = MoreItemIdsExample1,
        [nameof(Items).ToSnakeCase()] = CommunityDto.PureListExample1
    };
    public static IOpenApiAny Example { get; } = PureExample1;
    public static OpenApiArray MoreItemIdsExample1 { get; } = new OpenApiArray
    {
        new OpenApiString("RGsCJ27lPMx"),
        new OpenApiString("YdBEnPyyupH"),
        new OpenApiString("A8zQSZZQDmv"),
        new OpenApiString("wtjAGlpzqaS"),
        new OpenApiString("mmXYIPXxbNB"),
    };

    public QueryResultDto(List<T> items, List<string> moreItemIds)
    {
        MoreItemIds = moreItemIds;
        Items = items;
    }
}
