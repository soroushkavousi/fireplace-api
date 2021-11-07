using FireplaceApi.Api.Extensions;
using FireplaceApi.Api.Tools;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FireplaceApi.Api.Controllers
{
    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class PageDto<T>
    {
        [Required]
        [JsonPropertyName("pagination")]
        [JsonPropertyOrder(99)]
        public PaginationDto Pagination { get; set; }
        [Required]
        [JsonPropertyName("items")]
        [JsonPropertyOrder(100)]
        public List<T> Items { get; set; }


        public static OpenApiObject PureExample1 { get; } = new OpenApiObject
        {
            [nameof(Pagination).ToSnakeCase()] = PaginationDto.PureExample1,
            [nameof(Items).ToSnakeCase()] = CommunityDto.PureListExample1
        };
        public static IOpenApiAny Example { get; } = PureExample1;
        //public static Dictionary<string, IOpenApiAny> ActionExamples { get; } = new Dictionary<string, IOpenApiAny>
        //{
        //    [nameof(CommunityController.ListCommunitiesAsync)] = PageOfCommunitiesExample1,
        //};

        public PageDto(List<T> items, PaginationDto paginationDto)
        {
            Pagination = paginationDto;
            Items = items;
        }
    }

    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class PaginationDto
    {
        public string Pointer { get; set; }
        public string NextLink { get; set; }
        public string PerviousLink { get; set; }
        public int? PageNumber { get; set; }
        public int? Start { get; set; }
        public int? End { get; set; }
        public int? Limit { get; set; }
        [Required]
        public int TotalItemsCount { get; set; }
        [Required]
        public int TotalPagesCount { get; set; }

        public static OpenApiObject PureExample1 { get; } = new OpenApiObject
        {
            [nameof(Pointer).ToSnakeCase()] = new OpenApiString("z2vywvh7h6"),
            [nameof(NextLink).ToSnakeCase()] = new OpenApiString("?pointer=z2vywvh7h6&next"),
            [nameof(PerviousLink).ToSnakeCase()] = new OpenApiString("?pointer=z2vywvh7h6&previous"),
            [nameof(PageNumber).ToSnakeCase()] = new OpenApiInteger(2),
            [nameof(Start).ToSnakeCase()] = new OpenApiInteger(20),
            [nameof(End).ToSnakeCase()] = new OpenApiInteger(29),
            [nameof(Limit).ToSnakeCase()] = new OpenApiInteger(10),
            [nameof(TotalItemsCount).ToSnakeCase()] = new OpenApiInteger(300),
            [nameof(TotalPagesCount).ToSnakeCase()] = new OpenApiInteger(30),
        };
        public static IOpenApiAny Example { get; } = PureExample1;

        public PaginationDto(string queryResultPointer, string listPath,
            int? pageNumber, int? start, int? end, int? limit,
            int totalItemsCount, int totalPagesCount)
        {
            Pointer = queryResultPointer;
            if (end.HasValue && end.Value < totalItemsCount - 1)
                NextLink = $"{listPath}?pointer={queryResultPointer}&next";
            if (start.HasValue && start.Value > 0)
                PerviousLink = $"{listPath}?pointer={queryResultPointer}&previous";
            PageNumber = pageNumber;
            Start = start;
            End = end;
            Limit = limit;
            TotalItemsCount = totalItemsCount;
            TotalPagesCount = totalPagesCount;
        }
    }
}
