using FireplaceApi.Application.Extensions;
using FireplaceApi.Application.Tools;
using FireplaceApi.Domain.Enums;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Application.Controllers
{
    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class PostDto
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string AuthorId { get; set; }
        [Required]
        public string AuthorUsername { get; set; }
        [Required]
        public string CommunityId { get; set; }
        [Required]
        public string CommunityName { get; set; }
        [Required]
        public int Vote { get; set; }
        [Required]
        public VoteType RequestingUserVote { get; set; }
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
            [nameof(Id).ToSnakeCase()] = new OpenApiString("iC73r5PwMAP"),
            [nameof(AuthorId).ToSnakeCase()] = UserDto.PureExample1[nameof(UserDto.Id).ToSnakeCase()],
            [nameof(AuthorUsername).ToSnakeCase()] = UserDto.PureExample1[nameof(UserDto.Username).ToSnakeCase()],
            [nameof(CommunityId).ToSnakeCase()] = CommunityDto.PureExample1[nameof(CommunityDto.Id).ToSnakeCase()],
            [nameof(CommunityName).ToSnakeCase()] = CommunityDto.PureExample1[nameof(CommunityDto.Name).ToSnakeCase()],
            [nameof(Vote).ToSnakeCase()] = new OpenApiInteger(53),
            [nameof(RequestingUserVote).ToSnakeCase()] = new OpenApiString(VoteType.UPVOTE.ToString()),
            [nameof(Content).ToSnakeCase()] = new OpenApiString("Hello guys.\nThis is my content!"),
            [nameof(CreationDate).ToSnakeCase()] = new OpenApiDateTime(Utils.GetYesterdayDate()),
            [nameof(ModifiedDate).ToSnakeCase()] = new OpenApiNull(),
            [nameof(Author).ToSnakeCase()] = new OpenApiNull(),
            [nameof(Community).ToSnakeCase()] = new OpenApiNull(),
        };
        public static OpenApiObject PureExample2 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = new OpenApiString("7p5gjJaRKAw"),
            [nameof(AuthorId).ToSnakeCase()] = UserDto.PureExample2[nameof(UserDto.Id).ToSnakeCase()],
            [nameof(AuthorUsername).ToSnakeCase()] = UserDto.PureExample2[nameof(UserDto.Username).ToSnakeCase()],
            [nameof(CommunityId).ToSnakeCase()] = CommunityDto.PureExample2[nameof(CommunityDto.Id).ToSnakeCase()],
            [nameof(CommunityName).ToSnakeCase()] = CommunityDto.PureExample2[nameof(CommunityDto.Name).ToSnakeCase()],
            [nameof(Vote).ToSnakeCase()] = new OpenApiInteger(4),
            [nameof(RequestingUserVote).ToSnakeCase()] = new OpenApiString(VoteType.NEUTRAL.ToString()),
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
            [nameof(RequestingUserVote).ToSnakeCase()] = PureExample1[nameof(RequestingUserVote).ToSnakeCase()],
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
            [nameof(RequestingUserVote).ToSnakeCase()] = PureExample2[nameof(RequestingUserVote).ToSnakeCase()],
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
        public static OpenApiObject QueryResultExample1 { get; } = new OpenApiObject
        {
            [nameof(QueryResultDto<PostDto>.Items).ToSnakeCase()] = PureListExample1,
            [nameof(QueryResultDto<PostDto>.MoreItemIds).ToSnakeCase()] = QueryResultDto<PostDto>.MoreItemIdsExample1
        };
        public static OpenApiObject QueryResultExample2 { get; } = new OpenApiObject
        {
            [nameof(QueryResultDto<PostDto>.Items).ToSnakeCase()] = PureListExample1,
            [nameof(QueryResultDto<PostDto>.MoreItemIds).ToSnakeCase()] = new OpenApiNull(),
        };

        public static IOpenApiAny Example { get; } = Example1;
        public static Dictionary<string, IOpenApiAny> ActionExamples { get; } = new Dictionary<string, IOpenApiAny>
        {
            [nameof(PostController.ListCommunityPostsAsync)] = QueryResultExample1,
            [nameof(PostController.ListPostsAsync)] = QueryResultExample1,
            [nameof(PostController.ListSelfPostsAsync)] = QueryResultExample1,
            [nameof(PostController.GetPostByIdAsync)] = PureExample1,
            [nameof(PostController.CreatePostAsync)] = PureExample1,
            [nameof(PostController.VotePostAsync)] = PureExample1,
            [nameof(PostController.ToggleVoteForPostAsync)] = PureExample1,
            [nameof(PostController.DeleteVoteForPostAsync)] = new OpenApiNull(),
            [nameof(PostController.PatchPostByIdAsync)] = PureExample1,
            [nameof(PostController.DeletePostByIdAsync)] = new OpenApiNull(),
        };

        static PostDto()
        {

        }

        public PostDto(string id, string authorId, string authorUsername, string communityId,
            string communityName, int vote, VoteType requestingUserVote, string content,
            DateTime creationDate, DateTime? modifiedDate, UserDto author = null,
            CommunityDto community = null)
        {
            Id = id;
            AuthorId = authorId;
            AuthorUsername = authorUsername;
            CommunityId = communityId;
            CommunityName = communityName;
            Vote = vote;
            RequestingUserVote = requestingUserVote;
            Content = content;
            CreationDate = creationDate;
            ModifiedDate = modifiedDate;
            Author = author;
            Community = community;
        }
    }
}
