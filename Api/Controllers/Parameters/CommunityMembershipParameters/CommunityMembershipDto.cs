using FireplaceApi.Api.Tools;
using FireplaceApi.Core.Extensions;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class CommunityMembershipDto
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public long UserId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public long CommunityId { get; set; }
        [Required]
        public string CommunityName { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        public UserDto User { get; set; }
        public CommunityDto Community { get; set; }

        public static OpenApiObject PureExample1 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = new OpenApiInteger(10001),
            [nameof(UserId).ToSnakeCase()] = UserDto.PureExample1[nameof(UserDto.Id).ToSnakeCase()],
            [nameof(Username).ToSnakeCase()] = UserDto.PureExample1[nameof(UserDto.Username).ToSnakeCase()],
            [nameof(CommunityId).ToSnakeCase()] = CommunityDto.PureExample1[nameof(CommunityDto.Id).ToSnakeCase()],
            [nameof(CommunityName).ToSnakeCase()] = CommunityDto.PureExample1[nameof(CommunityDto.Name).ToSnakeCase()],
            [nameof(CreationDate).ToSnakeCase()] = new OpenApiDateTime(Utils.GetYesterdayDate()),
            [nameof(User).ToSnakeCase()] = new OpenApiNull(),
            [nameof(Community).ToSnakeCase()] = new OpenApiNull(),
        };
        public static OpenApiObject PureExample2 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = new OpenApiInteger(20001),
            [nameof(UserId).ToSnakeCase()] = UserDto.PureExample2[nameof(UserDto.Id).ToSnakeCase()],
            [nameof(Username).ToSnakeCase()] = UserDto.PureExample2[nameof(UserDto.Username).ToSnakeCase()],
            [nameof(CommunityId).ToSnakeCase()] = CommunityDto.PureExample2[nameof(CommunityDto.Id).ToSnakeCase()],
            [nameof(CommunityName).ToSnakeCase()] = CommunityDto.PureExample2[nameof(CommunityDto.Name).ToSnakeCase()],
            [nameof(CreationDate).ToSnakeCase()] = new OpenApiDateTime(Utils.GetYesterdayDate()),
            [nameof(User).ToSnakeCase()] = new OpenApiNull(),
            [nameof(Community).ToSnakeCase()] = new OpenApiNull(),
        };

        public static OpenApiArray PureListExample1 { get; } = new OpenApiArray
        {
            PureExample1, PureExample2
        };

        public static OpenApiObject Example1 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = PureExample1[nameof(Id).ToSnakeCase()],
            [nameof(UserId).ToSnakeCase()] = PureExample1[nameof(UserId).ToSnakeCase()],
            [nameof(Username).ToSnakeCase()] = PureExample1[nameof(Username).ToSnakeCase()],
            [nameof(CommunityId).ToSnakeCase()] = PureExample1[nameof(CommunityId).ToSnakeCase()],
            [nameof(CommunityName).ToSnakeCase()] = PureExample1[nameof(CommunityName).ToSnakeCase()],
            [nameof(CreationDate).ToSnakeCase()] = PureExample1[nameof(CreationDate).ToSnakeCase()],
            [nameof(User).ToSnakeCase()] = UserDto.PureExample1,
            [nameof(Community).ToSnakeCase()] = CommunityDto.PureExample1
        };
        public static OpenApiObject Example2 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = PureExample2[nameof(Id).ToSnakeCase()],
            [nameof(UserId).ToSnakeCase()] = PureExample2[nameof(UserId).ToSnakeCase()],
            [nameof(Username).ToSnakeCase()] = PureExample2[nameof(Username).ToSnakeCase()],
            [nameof(CommunityId).ToSnakeCase()] = PureExample2[nameof(CommunityId).ToSnakeCase()],
            [nameof(CommunityName).ToSnakeCase()] = PureExample2[nameof(CommunityName).ToSnakeCase()],
            [nameof(CreationDate).ToSnakeCase()] = PureExample2[nameof(CreationDate).ToSnakeCase()],
            [nameof(User).ToSnakeCase()] = UserDto.PureExample2,
            [nameof(Community).ToSnakeCase()] = CommunityDto.PureExample2
        };
        public static OpenApiArray ListExample1 { get; } = new OpenApiArray
        {
            Example1, Example2
        };
        public static OpenApiObject PageExample1 { get; } = new OpenApiObject
        {
            [nameof(PageDto<CommunityMembershipDto>.Pagination).ToSnakeCase()] = PaginationDto.PureExample1,
            [nameof(PageDto<CommunityMembershipDto>.Items).ToSnakeCase()] = PureListExample1
        };


        public static IOpenApiAny Example { get; } = Example1;
        public static Dictionary<string, IOpenApiAny> ActionExamples { get; } = new Dictionary<string, IOpenApiAny>
        {
            [nameof(CommunityMembershipController.ListCommunityMembershipsAsync)] = PageExample1,
            [nameof(CommunityMembershipController.GetCommunityMembershipByIdAsync)] = Example1,
            [nameof(CommunityMembershipController.CreateCommunityMembershipAsync)] = Example1,
            [nameof(CommunityMembershipController.PatchCommunityMembershipByIdAsync)] = Example1,
            [nameof(CommunityMembershipController.DeleteCommunityMembershipByIdAsync)] = Example1,
        };

        static CommunityMembershipDto()
        {

        }

        public CommunityMembershipDto(long id, long userId, string username,
            long communityId, string communityName, DateTime creationDate,
            UserDto user = null, CommunityDto community = null)
        {
            Id = id;
            UserId = userId;
            Username = username;
            CommunityId = communityId;
            CommunityName = communityName;
            CreationDate = creationDate;
            User = user;
            Community = community;
        }
    }
}
