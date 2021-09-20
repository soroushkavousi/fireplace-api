using Microsoft.OpenApi.Any;
using FireplaceApi.Api.Extensions;
using FireplaceApi.Api.Interfaces;
using FireplaceApi.Api.Tools;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FireplaceApi.Core.Extensions;

namespace FireplaceApi.Api.Controllers
{
    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class CommunityDto
    {
        [Required]
        public long? Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public long CreatorId { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        public UserDto Creator { get; set; }

        public static OpenApiObject PureCommunityExample1 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = new OpenApiInteger(10001),
            [nameof(Name).ToSnakeCase()] = new OpenApiString("backend-developers"),
            [nameof(CreatorId).ToSnakeCase()] = UserDto.PureUserExample1[nameof(Id).ToSnakeCase()],
            [nameof(CreationDate).ToSnakeCase()] = new OpenApiDateTime(Utils.GetYesterdayDate()),
            [nameof(Creator).ToSnakeCase()] = new OpenApiNull(),
        };
        public static OpenApiObject PureCommunityExample2 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = new OpenApiInteger(20002),
            [nameof(Name).ToSnakeCase()] = new OpenApiString("android-developers"),
            [nameof(CreatorId).ToSnakeCase()] = UserDto.PureUserExample2[nameof(Id).ToSnakeCase()],
            [nameof(CreationDate).ToSnakeCase()] = new OpenApiDateTime(Utils.GetYesterdayDate()),
            [nameof(Creator).ToSnakeCase()] = new OpenApiNull(),
        };

        public static OpenApiArray ListOfPureCommunitiesExample1 { get; } = new OpenApiArray
        {
            PureCommunityExample1, PureCommunityExample2
        };

        public static OpenApiObject CommunityExample1 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = PureCommunityExample1[nameof(Id).ToSnakeCase()],
            [nameof(Name).ToSnakeCase()] = PureCommunityExample1[nameof(Name).ToSnakeCase()],
            [nameof(CreatorId).ToSnakeCase()] = PureCommunityExample1[nameof(CreatorId).ToSnakeCase()],
            [nameof(CreationDate).ToSnakeCase()] = PureCommunityExample1[nameof(CreationDate).ToSnakeCase()],
            [nameof(Creator).ToSnakeCase()] = PureCommunityExample1[nameof(Creator).ToSnakeCase()],
        };
        public static OpenApiObject CommunityExample2 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = PureCommunityExample2[nameof(Id).ToSnakeCase()],
            [nameof(Name).ToSnakeCase()] = PureCommunityExample2[nameof(Name).ToSnakeCase()],
            [nameof(CreatorId).ToSnakeCase()] = PureCommunityExample2[nameof(CreatorId).ToSnakeCase()],
            [nameof(CreationDate).ToSnakeCase()] = PureCommunityExample2[nameof(CreationDate).ToSnakeCase()],
            [nameof(Creator).ToSnakeCase()] = PureCommunityExample2[nameof(Creator).ToSnakeCase()],
        };
        public static OpenApiArray ListOfCommunitiesExample1 { get; } = new OpenApiArray
        {
            CommunityExample1, CommunityExample2
        };
        public static OpenApiObject PageOfCommunitiesExample1 { get; } = new OpenApiObject
        {
            [nameof(PageDto<CommunityDto>.TotalItemsCount).ToSnakeCase()] = new OpenApiInteger(200),
            [nameof(PageDto<CommunityDto>.Pagination).ToSnakeCase()] = PaginationDto.PurePaginationExample1,
            [nameof(PageDto<CommunityDto>.Items).ToSnakeCase()] = ListOfPureCommunitiesExample1
        };


        public static IOpenApiAny Example { get; } = CommunityExample1;
        public static Dictionary<string, IOpenApiAny> ActionExamples { get; } = new Dictionary<string, IOpenApiAny>
        {
            [nameof(CommunityController.ListCommunitiesAsync)] = PageOfCommunitiesExample1,
            [nameof(CommunityController.GetCommunityByIdAsync)] = CommunityExample1,
            [nameof(CommunityController.GetCommunityByNameAsync)] = CommunityExample1,
            [nameof(CommunityController.CreateCommunityAsync)] = CommunityExample1,
            [nameof(CommunityController.PatchCommunityAsync)] = CommunityExample1,
            [nameof(CommunityController.DeleteCommunityAsync)] = CommunityExample1,
        };

        static CommunityDto()
        {

        }

        public CommunityDto(long? id, string name, long creatorId, 
            DateTime creationDate, UserDto creator = null)
        {
            Id = id;
            Name = name;
            CreatorId = creatorId;
            CreationDate = creationDate;
            Creator = creator;
        }
    }
}
