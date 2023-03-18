using FireplaceApi.Application.Extensions;
using FireplaceApi.Application.Tools;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Application.Controllers
{
    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class CommunityMembershipDto
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string CommunityId { get; set; }
        [Required]
        public string CommunityName { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }

        public static OpenApiObject PureExample1 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = new OpenApiString("CKox49N2J5v"),
            [nameof(UserId).ToSnakeCase()] = UserDto.PureExample1[nameof(UserDto.Id).ToSnakeCase()],
            [nameof(Username).ToSnakeCase()] = UserDto.PureExample1[nameof(UserDto.Username).ToSnakeCase()],
            [nameof(CommunityId).ToSnakeCase()] = CommunityDto.PureExample1[nameof(CommunityDto.Id).ToSnakeCase()],
            [nameof(CommunityName).ToSnakeCase()] = CommunityDto.PureExample1[nameof(CommunityDto.Name).ToSnakeCase()],
            [nameof(CreationDate).ToSnakeCase()] = new OpenApiDateTime(Utils.GetYesterdayDate()),
        };
        public static OpenApiObject PureExample2 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = new OpenApiString("hAzZUu5exKE"),
            [nameof(UserId).ToSnakeCase()] = UserDto.PureExample2[nameof(UserDto.Id).ToSnakeCase()],
            [nameof(Username).ToSnakeCase()] = UserDto.PureExample2[nameof(UserDto.Username).ToSnakeCase()],
            [nameof(CommunityId).ToSnakeCase()] = CommunityDto.PureExample2[nameof(CommunityDto.Id).ToSnakeCase()],
            [nameof(CommunityName).ToSnakeCase()] = CommunityDto.PureExample2[nameof(CommunityDto.Name).ToSnakeCase()],
            [nameof(CreationDate).ToSnakeCase()] = new OpenApiDateTime(Utils.GetYesterdayDate()),
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
        };
        public static OpenApiObject Example2 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = PureExample2[nameof(Id).ToSnakeCase()],
            [nameof(UserId).ToSnakeCase()] = PureExample2[nameof(UserId).ToSnakeCase()],
            [nameof(Username).ToSnakeCase()] = PureExample2[nameof(Username).ToSnakeCase()],
            [nameof(CommunityId).ToSnakeCase()] = PureExample2[nameof(CommunityId).ToSnakeCase()],
            [nameof(CommunityName).ToSnakeCase()] = PureExample2[nameof(CommunityName).ToSnakeCase()],
            [nameof(CreationDate).ToSnakeCase()] = PureExample2[nameof(CreationDate).ToSnakeCase()],
        };
        public static OpenApiArray ListExample1 { get; } = new OpenApiArray
        {
            Example1, Example2
        };
        public static OpenApiObject QueryResultExample1 { get; } = new OpenApiObject
        {
            [nameof(QueryResultDto<CommunityMembershipDto>.Items).ToSnakeCase()] = PureListExample1,
            [nameof(QueryResultDto<CommunityMembershipDto>.MoreItemIds).ToSnakeCase()] = QueryResultDto<CommunityMembershipDto>.MoreItemIdsExample1
        };


        public static IOpenApiAny Example { get; } = Example1;
        public static Dictionary<string, IOpenApiAny> ActionExamples { get; } = new Dictionary<string, IOpenApiAny>
        {
            [nameof(CommunityMembershipController.CreateCommunityMembershipAsync)] = Example1,
            [nameof(CommunityMembershipController.DeleteCommunityMembershipByCommunityIdentifierAsync)] = new OpenApiNull(),
        };

        static CommunityMembershipDto()
        {

        }

        public CommunityMembershipDto(string id, string userId, string username,
            string communityId, string communityName, DateTime creationDate)
        {
            Id = id;
            UserId = userId;
            Username = username;
            CommunityId = communityId;
            CommunityName = communityName;
            CreationDate = creationDate;
        }
    }
}
