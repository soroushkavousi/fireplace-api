using FireplaceApi.Presentation.Controllers;
using FireplaceApi.Presentation.Extensions;
using FireplaceApi.Presentation.Tools;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Presentation.Dtos;

[SwaggerSchemaFilter(typeof(TypeExampleProvider))]
public class CommentDto
{
    [Required]
    public string Id { get; set; }
    [Required]
    public string AuthorId { get; set; }
    [Required]
    public string AuthorUsername { get; set; }
    [Required]
    public string PostId { get; set; }
    [Required]
    public string ParentCommentId { get; set; }
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
    public ProfileDto Author { get; set; }
    public PostDto Post { get; set; }
    public List<CommentDto> ChildComments { get; set; }
    public List<string> MoreChildCommentIds { get; set; }

    public static OpenApiObject PureExample1 { get; } = new OpenApiObject
    {
        [nameof(Id).ToSnakeCase()] = new OpenApiString("Q8BWziw4hhV"),
        [nameof(AuthorId).ToSnakeCase()] = UserDto.PureExample1[nameof(UserDto.Id).ToSnakeCase()],
        [nameof(AuthorUsername).ToSnakeCase()] = UserDto.PureExample1[nameof(UserDto.Username).ToSnakeCase()],
        [nameof(PostId).ToSnakeCase()] = PostDto.PureExample1[nameof(PostDto.Id).ToSnakeCase()],
        [nameof(ParentCommentId).ToSnakeCase()] = new OpenApiString("S2KwZBd671V"),
        [nameof(Vote).ToSnakeCase()] = new OpenApiInteger(53),
        [nameof(RequestingUserVote).ToSnakeCase()] = new OpenApiString(VoteType.UPVOTE.ToString()),
        [nameof(Content).ToSnakeCase()] = new OpenApiString("It's ok."),
        [nameof(CreationDate).ToSnakeCase()] = new OpenApiDateTime(Tools.Utils.GetYesterdayDate()),
        [nameof(ModifiedDate).ToSnakeCase()] = new OpenApiNull(),
        [nameof(Author).ToSnakeCase()] = new OpenApiNull(),
        [nameof(Post).ToSnakeCase()] = new OpenApiNull(),
        [nameof(ChildComments).ToSnakeCase()] = new OpenApiNull(),
        [nameof(MoreChildCommentIds).ToSnakeCase()] = new OpenApiNull(),
    };
    public static OpenApiObject PureExample2 { get; } = new OpenApiObject
    {
        [nameof(Id).ToSnakeCase()] = new OpenApiString("5kMUWjptzUQ"),
        [nameof(AuthorId).ToSnakeCase()] = UserDto.PureExample2[nameof(UserDto.Id).ToSnakeCase()],
        [nameof(AuthorUsername).ToSnakeCase()] = UserDto.PureExample2[nameof(UserDto.Username).ToSnakeCase()],
        [nameof(PostId).ToSnakeCase()] = PostDto.PureExample2[nameof(PostDto.Id).ToSnakeCase()],
        [nameof(ParentCommentId).ToSnakeCase()] = new OpenApiString("S2KwZBd671V"),
        [nameof(Vote).ToSnakeCase()] = new OpenApiInteger(4),
        [nameof(RequestingUserVote).ToSnakeCase()] = new OpenApiString(VoteType.NEUTRAL.ToString()),
        [nameof(Content).ToSnakeCase()] = new OpenApiString("It's not good!"),
        [nameof(CreationDate).ToSnakeCase()] = new OpenApiDateTime(Tools.Utils.GetYesterdayDate()),
        [nameof(ModifiedDate).ToSnakeCase()] = new OpenApiDateTime(Tools.Utils.GetLastHourDate()),
        [nameof(Author).ToSnakeCase()] = new OpenApiNull(),
        [nameof(Post).ToSnakeCase()] = new OpenApiNull(),
        [nameof(ChildComments).ToSnakeCase()] = new OpenApiNull(),
        [nameof(MoreChildCommentIds).ToSnakeCase()] = new OpenApiNull(),
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
        [nameof(PostId).ToSnakeCase()] = PureExample1[nameof(PostId).ToSnakeCase()],
        [nameof(ParentCommentId).ToSnakeCase()] = PureExample1[nameof(ParentCommentId).ToSnakeCase()],
        [nameof(Vote).ToSnakeCase()] = PureExample1[nameof(Vote).ToSnakeCase()],
        [nameof(RequestingUserVote).ToSnakeCase()] = PureExample1[nameof(RequestingUserVote).ToSnakeCase()],
        [nameof(Content).ToSnakeCase()] = PureExample1[nameof(Content).ToSnakeCase()],
        [nameof(CreationDate).ToSnakeCase()] = PureExample1[nameof(CreationDate).ToSnakeCase()],
        [nameof(ModifiedDate).ToSnakeCase()] = PureExample1[nameof(ModifiedDate).ToSnakeCase()],
        [nameof(Author).ToSnakeCase()] = UserDto.PureExample1,
        [nameof(Post).ToSnakeCase()] = PostDto.PureExample1,
        [nameof(ChildComments).ToSnakeCase()] = new OpenApiNull(),
        [nameof(MoreChildCommentIds).ToSnakeCase()] = new OpenApiNull(),
    };
    public static OpenApiObject Example2 { get; } = new OpenApiObject
    {
        [nameof(Id).ToSnakeCase()] = PureExample2[nameof(Id).ToSnakeCase()],
        [nameof(AuthorId).ToSnakeCase()] = PureExample2[nameof(AuthorId).ToSnakeCase()],
        [nameof(AuthorUsername).ToSnakeCase()] = PureExample2[nameof(AuthorUsername).ToSnakeCase()],
        [nameof(PostId).ToSnakeCase()] = PureExample2[nameof(PostId).ToSnakeCase()],
        [nameof(ParentCommentId).ToSnakeCase()] = PureExample2[nameof(ParentCommentId).ToSnakeCase()],
        [nameof(Vote).ToSnakeCase()] = PureExample2[nameof(Vote).ToSnakeCase()],
        [nameof(RequestingUserVote).ToSnakeCase()] = PureExample2[nameof(RequestingUserVote).ToSnakeCase()],
        [nameof(Content).ToSnakeCase()] = PureExample2[nameof(Content).ToSnakeCase()],
        [nameof(CreationDate).ToSnakeCase()] = PureExample2[nameof(CreationDate).ToSnakeCase()],
        [nameof(ModifiedDate).ToSnakeCase()] = PureExample2[nameof(ModifiedDate).ToSnakeCase()],
        [nameof(Author).ToSnakeCase()] = UserDto.PureExample2,
        [nameof(Post).ToSnakeCase()] = PostDto.PureExample2,
        [nameof(ChildComments).ToSnakeCase()] = new OpenApiNull(),
        [nameof(MoreChildCommentIds).ToSnakeCase()] = new OpenApiNull(),
    };
    public static OpenApiArray ListExample1 { get; } = new OpenApiArray
    {
        Example1, Example2
    };
    public static OpenApiObject QueryResultExample1 { get; } = new OpenApiObject
    {
        [nameof(QueryResultDto<CommentDto>.Items).ToSnakeCase()] = PureListExample1,
        [nameof(QueryResultDto<CommentDto>.MoreItemIds).ToSnakeCase()] = QueryResultDto<CommentDto>.MoreItemIdsExample1
    };
    public static OpenApiObject QueryResultExample2 { get; } = new OpenApiObject
    {
        [nameof(QueryResultDto<CommentDto>.Items).ToSnakeCase()] = PureListExample1,
        [nameof(QueryResultDto<CommentDto>.MoreItemIds).ToSnakeCase()] = new OpenApiNull()
    };

    public static IOpenApiAny Example { get; } = Example1;
    public static Dictionary<string, IOpenApiAny> ActionExamples { get; } = new Dictionary<string, IOpenApiAny>
    {
        [nameof(CommentController.ListPostCommentsAsync)] = QueryResultExample1,
        [nameof(CommentController.ListCommentsAsync)] = QueryResultExample2,
        [nameof(CommentController.ListSelfCommentsAsync)] = QueryResultExample1,
        [nameof(CommentController.ListChildCommentsAsync)] = QueryResultExample1,
        [nameof(CommentController.GetCommentByIdAsync)] = PureExample1,
        [nameof(CommentController.ReplyToPostAsync)] = PureExample1,
        [nameof(CommentController.ReplyToCommentAsync)] = PureExample1,
        [nameof(CommentController.VoteCommentAsync)] = PureExample1,
        [nameof(CommentController.ToggleVoteForCommentAsync)] = PureExample1,
        [nameof(CommentController.DeleteVoteForCommentAsync)] = new OpenApiNull(),
        [nameof(CommentController.PatchCommentByIdAsync)] = PureExample1,
        [nameof(CommentController.DeleteCommentByIdAsync)] = new OpenApiNull(),
    };

    static CommentDto()
    {

    }

    public CommentDto(string id, string authorId, string authorUsername,
        string postId, int vote, VoteType requestingUserVote,
        string content, DateTime creationDate, string parentCommentId = null,
        DateTime? modifiedDate = null, ProfileDto author = null, PostDto post = null,
        List<CommentDto> childComments = null, List<string> moreChildCommentIds = null)
    {
        Id = id;
        AuthorId = authorId;
        AuthorUsername = authorUsername;
        PostId = postId;
        ParentCommentId = parentCommentId;
        Vote = vote;
        RequestingUserVote = requestingUserVote;
        Content = content;
        CreationDate = creationDate;
        ModifiedDate = modifiedDate;
        Author = author;
        Post = post;
        ChildComments = childComments;
        MoreChildCommentIds = moreChildCommentIds;
    }
}
