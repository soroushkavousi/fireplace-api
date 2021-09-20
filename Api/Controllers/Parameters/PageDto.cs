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
        [Required]
        [JsonPropertyOrder(-1)]
        public int TotalItemsCount { get; set; }
        [JsonPropertyOrder(99)]
        public PaginationDto Pagination { get; set; }
        [Required]
        [JsonPropertyOrder(100)]
        public List<T> Items { get; set; }


        public static OpenApiObject PurePageExample1 { get; } = new OpenApiObject
        {
            [nameof(TotalItemsCount).ToSnakeCase()] = new OpenApiInteger(200),
            [nameof(Pagination).ToSnakeCase()] = PaginationDto.PurePaginationExample1,
            [nameof(Items).ToSnakeCase()] = CommunityDto.ListOfPureCommunitiesExample1
        };
        public static IOpenApiAny Example { get; } = PurePageExample1;
        //public static Dictionary<string, IOpenApiAny> ActionExamples { get; } = new Dictionary<string, IOpenApiAny>
        //{
        //    [nameof(CommunityController.ListCommunitiesAsync)] = PageOfCommunitiesExample1,
        //};

        public PageDto(int totalItemsCount, List<T> items, PaginationDto paginationDto = null)
        {
            TotalItemsCount = totalItemsCount;
            Pagination = paginationDto;
            Items = items;
        }
    }

    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class PaginationDto
    {
        [Required]
        public int TotalPagesCount { get; set; }
        [Required]
        public string NextLink { get; set; }
        [Required]
        public string PerviousLink { get; set; }
        public string Cursor { get; set; }

        public static OpenApiObject PurePaginationExample1 { get; } = new OpenApiObject
        {
            [nameof(TotalPagesCount).ToSnakeCase()] = new OpenApiInteger(10),
            [nameof(NextLink).ToSnakeCase()] = new OpenApiString("next-link"),
            [nameof(PerviousLink).ToSnakeCase()] = new OpenApiString("previous-link"),
            [nameof(Cursor).ToSnakeCase()] = new OpenApiString("CURSER"),
        };
        public static IOpenApiAny Example { get; } = PurePaginationExample1;

        public PaginationDto(int totalPagesCount, string nextLink, 
            string perviousLink, string cursor = null)
        {
            TotalPagesCount = totalPagesCount;
            NextLink = nextLink;
            PerviousLink = perviousLink;
            Cursor = cursor;
        }
    }
}
