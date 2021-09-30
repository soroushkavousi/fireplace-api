using Microsoft.AspNetCore.Http;
using FireplaceApi.Api.Extensions;
using FireplaceApi.Api.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using FireplaceApi.Core.Tools;
using FireplaceApi.Core.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.OpenApi.Any;

namespace FireplaceApi.Api.Controllers
{
    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class PageDto<T>
    {
        [JsonPropertyName("pagination")]
        [JsonPropertyOrder(99)]
        public PaginationDto Pagination { get; set; }
        [Required]
        [JsonPropertyName("items")]
        [JsonPropertyOrder(100)]
        public List<T> Items { get; set; }


        public static OpenApiObject PurePageExample1 { get; } = new OpenApiObject
        {
            [nameof(Pagination).ToSnakeCase()] = PaginationDto.PurePaginationExample1,
            [nameof(Items).ToSnakeCase()] = CommunityDto.ListOfPureCommunitiesExample1
        };
        public static IOpenApiAny Example { get; } = PurePageExample1;
        //public static Dictionary<string, IOpenApiAny> ActionExamples { get; } = new Dictionary<string, IOpenApiAny>
        //{
        //    [nameof(CommunityController.ListCommunitiesAsync)] = PageOfCommunitiesExample1,
        //};

        public PageDto(List<T> items, PaginationDto paginationDto = null)
        {
            Pagination = paginationDto;
            Items = items;
        }
    }

    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class PaginationDto
    {
        [Required]
        public string Pointer { get; set; }
        [Required]
        public string NextLink { get; set; }
        [Required]
        public string PerviousLink { get; set; }
        public int? PageNumber { get; set; }
        [Required]
        public int Start { get; set; }
        [Required]
        public int End { get; set; }
        [Required]
        public int Limit { get; set; }
        [Required]
        public int TotalItemsCount { get; set; }
        [Required]
        public int TotalPagesCount { get; set; }
        
        public static OpenApiObject PurePaginationExample1 { get; } = new OpenApiObject
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
        public static IOpenApiAny Example { get; } = PurePaginationExample1;

        public PaginationDto(string queryResultPointer, string listPath,
            int? pageNumber, int start, int end, int limit, 
            int totalItemsCount, int totalPagesCount)
        {
            Pointer = queryResultPointer;
            NextLink = $"{listPath}?pointer={queryResultPointer}&next";
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
