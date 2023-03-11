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
    public class CommunityDto
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string CreatorId { get; set; }
        [Required]
        public string CreatorUsername { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }

        public static OpenApiObject PureExample1 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = new OpenApiString("jNQCWv7kQZc"),
            [nameof(Name).ToSnakeCase()] = new OpenApiString("backend-developers"),
            [nameof(CreatorId).ToSnakeCase()] = UserDto.PureExample1[nameof(UserDto.Id).ToSnakeCase()],
            [nameof(CreatorUsername).ToSnakeCase()] = UserDto.PureExample1[nameof(UserDto.Username).ToSnakeCase()],
            [nameof(CreationDate).ToSnakeCase()] = new OpenApiDateTime(Utils.GetYesterdayDate()),
        };
        public static OpenApiObject PureExample2 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = new OpenApiString("TF8xV2pM5k5"),
            [nameof(Name).ToSnakeCase()] = new OpenApiString("android-developers"),
            [nameof(CreatorId).ToSnakeCase()] = UserDto.PureExample2[nameof(Id).ToSnakeCase()],
            [nameof(CreatorUsername).ToSnakeCase()] = UserDto.PureExample2[nameof(UserDto.Username).ToSnakeCase()],
            [nameof(CreationDate).ToSnakeCase()] = new OpenApiDateTime(Utils.GetYesterdayDate()),
        };

        public static OpenApiArray PureListExample1 { get; } = new OpenApiArray
        {
            PureExample1, PureExample2
        };

        public static OpenApiObject Example1 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = PureExample1[nameof(Id).ToSnakeCase()],
            [nameof(Name).ToSnakeCase()] = PureExample1[nameof(Name).ToSnakeCase()],
            [nameof(CreatorId).ToSnakeCase()] = PureExample1[nameof(CreatorId).ToSnakeCase()],
            [nameof(CreatorUsername).ToSnakeCase()] = PureExample1[nameof(CreatorUsername).ToSnakeCase()],
            [nameof(CreationDate).ToSnakeCase()] = PureExample1[nameof(CreationDate).ToSnakeCase()],
        };
        public static OpenApiObject Example2 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = PureExample2[nameof(Id).ToSnakeCase()],
            [nameof(Name).ToSnakeCase()] = PureExample2[nameof(Name).ToSnakeCase()],
            [nameof(CreatorId).ToSnakeCase()] = PureExample2[nameof(CreatorId).ToSnakeCase()],
            [nameof(CreatorUsername).ToSnakeCase()] = PureExample2[nameof(CreatorUsername).ToSnakeCase()],
            [nameof(CreationDate).ToSnakeCase()] = PureExample2[nameof(CreationDate).ToSnakeCase()],
        };
        public static OpenApiArray ListExample1 { get; } = new OpenApiArray
        {
            Example1, Example2
        };
        public static OpenApiObject QueryResultExample1 { get; } = new OpenApiObject
        {
            [nameof(QueryResultDto<CommunityDto>.Items).ToSnakeCase()] = PureListExample1,
            [nameof(QueryResultDto<CommunityDto>.MoreItemIds).ToSnakeCase()] = QueryResultDto<CommunityDto>.MoreItemIdsExample1
        };

        public static IOpenApiAny Example { get; } = Example1;
        public static Dictionary<string, IOpenApiAny> ActionExamples { get; } = new Dictionary<string, IOpenApiAny>
        {
            [nameof(CommunityController.ListCommunitiesAsync)] = QueryResultExample1,
            [nameof(CommunityController.GetCommunityByIdOrNameAsync)] = Example1,
            [nameof(CommunityController.CreateCommunityAsync)] = Example1,
            [nameof(CommunityController.PatchCommunityByEncodedIdOrNameAsync)] = Example1,
            [nameof(CommunityController.DeleteCommunityByIdOrNameAsync)] = new OpenApiNull(),
        };

        static CommunityDto()
        {

        }

        public CommunityDto(string id, string name, string creatorId,
            string creatorUsername, DateTime creationDate)
        {
            Id = id;
            Name = name;
            CreatorId = creatorId;
            CreatorUsername = creatorUsername;
            CreationDate = creationDate;
        }
    }
}
