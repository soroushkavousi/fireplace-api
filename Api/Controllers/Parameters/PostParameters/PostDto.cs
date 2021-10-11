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
    public class PostDto
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public long AuthorId { get; set; }
        [Required]
        public string AuthorUsername { get; set; }
        [Required]
        public long CommunityId { get; set; }
        [Required]
        public string CommunityName { get; set; }
        [Required]
        public int Vote { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        [Required]
        public DateTime? ModifiedDate { get; set; }
        public UserDto Author { get; set; }
        public CommunityDto Community { get; set; }

        public static OpenApiObject PureExample1 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = new OpenApiInteger(10001),
            [nameof(AuthorId).ToSnakeCase()] = UserDto.PureExample1[nameof(UserDto.Id).ToSnakeCase()],
            [nameof(AuthorUsername).ToSnakeCase()] = UserDto.PureExample1[nameof(UserDto.Username).ToSnakeCase()],
            [nameof(CommunityId).ToSnakeCase()] = CommunityDto.PureExample1[nameof(CommunityDto.Id).ToSnakeCase()],
            [nameof(CommunityName).ToSnakeCase()] = CommunityDto.PureExample1[nameof(CommunityDto.Name).ToSnakeCase()],
            [nameof(Vote).ToSnakeCase()] = new OpenApiInteger(53),
            [nameof(Content).ToSnakeCase()] = new OpenApiString("Hello guys.\nThis is my content!"),
            [nameof(CreationDate).ToSnakeCase()] = new OpenApiDateTime(Utils.GetYesterdayDate()),
            [nameof(ModifiedDate).ToSnakeCase()] = new OpenApiNull(),
            [nameof(Author).ToSnakeCase()] = new OpenApiNull(),
            [nameof(Community).ToSnakeCase()] = new OpenApiNull(),
        };
        public static OpenApiObject PureExample2 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = new OpenApiInteger(10001),
            [nameof(AuthorId).ToSnakeCase()] = UserDto.PureExample2[nameof(UserDto.Id).ToSnakeCase()],
            [nameof(AuthorUsername).ToSnakeCase()] = UserDto.PureExample2[nameof(UserDto.Username).ToSnakeCase()],
            [nameof(CommunityId).ToSnakeCase()] = CommunityDto.PureExample2[nameof(CommunityDto.Id).ToSnakeCase()],
            [nameof(CommunityName).ToSnakeCase()] = CommunityDto.PureExample2[nameof(CommunityDto.Name).ToSnakeCase()],
            [nameof(Vote).ToSnakeCase()] = new OpenApiInteger(4),
            [nameof(Content).ToSnakeCase()] = new OpenApiString("What is the best way to ...?"),
            [nameof(CreationDate).ToSnakeCase()] = new OpenApiDateTime(Utils.GetYesterdayDate()),
            [nameof(ModifiedDate).ToSnakeCase()] = new OpenApiDateTime(Utils.GetLastHourDate()),
            [nameof(Author).ToSnakeCase()] = new OpenApiNull(),
            [nameof(Community).ToSnakeCase()] = new OpenApiNull(),
        };

        public static OpenApiArray PureListExample1 { get; } = new OpenApiArray
        {
            PureExample1, PureExample2
        };

        public static OpenApiObject Example1 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = PureExample1[nameof(Id).ToSnakeCase()],
            [nameof(AuthorId).ToSnakeCase()] = PureExample1[nameof(AuthorId).ToSnakeCase()],
            [nameof(AuthorUsername).ToSnakeCase()] = PureExample1[nameof(AuthorUsername).ToSnakeCase()],
            [nameof(CommunityId).ToSnakeCase()] = PureExample1[nameof(CommunityId).ToSnakeCase()],
            [nameof(CommunityName).ToSnakeCase()] = PureExample1[nameof(CommunityName).ToSnakeCase()],
            [nameof(Vote).ToSnakeCase()] = PureExample1[nameof(Vote).ToSnakeCase()],
            [nameof(Content).ToSnakeCase()] = PureExample1[nameof(Content).ToSnakeCase()],
            [nameof(CreationDate).ToSnakeCase()] = PureExample1[nameof(CreationDate).ToSnakeCase()],
            [nameof(ModifiedDate).ToSnakeCase()] = PureExample1[nameof(ModifiedDate).ToSnakeCase()],
            [nameof(Author).ToSnakeCase()] = UserDto.PureExample1,
            [nameof(Community).ToSnakeCase()] = CommunityDto.PureExample1
        };
        public static OpenApiObject Example2 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = PureExample2[nameof(Id).ToSnakeCase()],
            [nameof(AuthorId).ToSnakeCase()] = PureExample2[nameof(AuthorId).ToSnakeCase()],
            [nameof(AuthorUsername).ToSnakeCase()] = PureExample2[nameof(AuthorUsername).ToSnakeCase()],
            [nameof(CommunityId).ToSnakeCase()] = PureExample2[nameof(CommunityId).ToSnakeCase()],
            [nameof(CommunityName).ToSnakeCase()] = PureExample2[nameof(CommunityName).ToSnakeCase()],
            [nameof(Vote).ToSnakeCase()] = PureExample2[nameof(Vote).ToSnakeCase()],
            [nameof(Content).ToSnakeCase()] = PureExample2[nameof(Content).ToSnakeCase()],
            [nameof(CreationDate).ToSnakeCase()] = PureExample2[nameof(CreationDate).ToSnakeCase()],
            [nameof(ModifiedDate).ToSnakeCase()] = PureExample2[nameof(ModifiedDate).ToSnakeCase()],
            [nameof(Author).ToSnakeCase()] = UserDto.PureExample2,
            [nameof(Community).ToSnakeCase()] = CommunityDto.PureExample2
        };
        public static OpenApiArray ListExample1 { get; } = new OpenApiArray
        {
            Example1, Example2
        };
        public static OpenApiObject PageExample1 { get; } = new OpenApiObject
        {
            [nameof(PageDto<PostDto>.Pagination).ToSnakeCase()] = PaginationDto.PureExample1,
            [nameof(PageDto<PostDto>.Items).ToSnakeCase()] = PureListExample1
        };


        public static IOpenApiAny Example { get; } = Example1;
        public static Dictionary<string, IOpenApiAny> ActionExamples { get; } = new Dictionary<string, IOpenApiAny>
        {
            [nameof(PostController.ListPostsAsync)] = PageExample1,
            [nameof(PostController.GetPostByIdAsync)] = PureExample1,
            [nameof(PostController.CreatePostAsync)] = PureExample1,
            [nameof(PostController.PatchPostByIdAsync)] = PureExample1,
            [nameof(PostController.DeletePostByIdAsync)] = PureExample1,
        };

        static PostDto()
        {

        }

        public PostDto(long id, long authorId, string authorUsername, long communityId,
            string communityName, int vote, string content, DateTime creationDate,
            DateTime? modifiedDate, UserDto author = null, CommunityDto community = null)
        {
            Id = id;
            AuthorId = authorId;
            AuthorUsername = authorUsername;
            CommunityId = communityId;
            CommunityName = communityName;
            Vote = vote;
            Content = content;
            CreationDate = creationDate;
            ModifiedDate = modifiedDate;
            Author = author;
            Community = community;
        }
    }
}
