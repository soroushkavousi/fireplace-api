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
    public class CommentDto
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public long AuthorId { get; set; }
        [Required]
        public string AuthorUsername { get; set; }
        [Required]
        public long PostId { get; set; }
        [Required]
        public int Vote { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        [Required]
        public DateTime? ModifiedDate { get; set; }
        [Required]
        public List<long> ParentCommentIds { get; set; }
        public UserDto Author { get; set; }
        public PostDto Post { get; set; }
        public List<CommentDto> ChildComments { get; set; }

        public static OpenApiObject PureExample1 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = new OpenApiInteger(10001),
            [nameof(AuthorId).ToSnakeCase()] = UserDto.PureExample1[nameof(UserDto.Id).ToSnakeCase()],
            [nameof(AuthorUsername).ToSnakeCase()] = UserDto.PureExample1[nameof(UserDto.Username).ToSnakeCase()],
            [nameof(PostId).ToSnakeCase()] = PostDto.PureExample1[nameof(PostDto.Id).ToSnakeCase()],
            [nameof(Vote).ToSnakeCase()] = new OpenApiInteger(53),
            [nameof(Content).ToSnakeCase()] = new OpenApiString("It's ok."),
            [nameof(CreationDate).ToSnakeCase()] = new OpenApiDateTime(Utils.GetYesterdayDate()),
            [nameof(ModifiedDate).ToSnakeCase()] = new OpenApiNull(),
            [nameof(ParentCommentIds).ToSnakeCase()] = new OpenApiArray(),
            [nameof(Author).ToSnakeCase()] = new OpenApiNull(),
            [nameof(Post).ToSnakeCase()] = new OpenApiNull(),
            [nameof(ChildComments).ToSnakeCase()] = new OpenApiNull(),
        };
        public static OpenApiObject PureExample2 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = new OpenApiInteger(20001),
            [nameof(AuthorId).ToSnakeCase()] = UserDto.PureExample2[nameof(UserDto.Id).ToSnakeCase()],
            [nameof(AuthorUsername).ToSnakeCase()] = UserDto.PureExample2[nameof(UserDto.Username).ToSnakeCase()],
            [nameof(PostId).ToSnakeCase()] = PostDto.PureExample2[nameof(PostDto.Id).ToSnakeCase()],
            [nameof(Vote).ToSnakeCase()] = new OpenApiInteger(4),
            [nameof(Content).ToSnakeCase()] = new OpenApiString("It's not good!"),
            [nameof(CreationDate).ToSnakeCase()] = new OpenApiDateTime(Utils.GetYesterdayDate()),
            [nameof(ModifiedDate).ToSnakeCase()] = new OpenApiDateTime(Utils.GetLastHourDate()),
            [nameof(ParentCommentIds).ToSnakeCase()] = new OpenApiArray(),
            [nameof(Author).ToSnakeCase()] = new OpenApiNull(),
            [nameof(Post).ToSnakeCase()] = new OpenApiNull(),
            [nameof(ChildComments).ToSnakeCase()] = new OpenApiNull(),
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
            [nameof(Vote).ToSnakeCase()] = PureExample1[nameof(Vote).ToSnakeCase()],
            [nameof(Content).ToSnakeCase()] = PureExample1[nameof(Content).ToSnakeCase()],
            [nameof(CreationDate).ToSnakeCase()] = PureExample1[nameof(CreationDate).ToSnakeCase()],
            [nameof(ModifiedDate).ToSnakeCase()] = PureExample1[nameof(ModifiedDate).ToSnakeCase()],
            [nameof(ParentCommentIds).ToSnakeCase()] = PureExample1[nameof(ParentCommentIds).ToSnakeCase()],
            [nameof(Author).ToSnakeCase()] = UserDto.PureExample1,
            [nameof(Post).ToSnakeCase()] = PostDto.PureExample1,
            [nameof(ChildComments).ToSnakeCase()] = new OpenApiNull(),
        };
        public static OpenApiObject Example2 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = PureExample2[nameof(Id).ToSnakeCase()],
            [nameof(AuthorId).ToSnakeCase()] = PureExample2[nameof(AuthorId).ToSnakeCase()],
            [nameof(AuthorUsername).ToSnakeCase()] = PureExample2[nameof(AuthorUsername).ToSnakeCase()],
            [nameof(PostId).ToSnakeCase()] = PureExample2[nameof(PostId).ToSnakeCase()],
            [nameof(Vote).ToSnakeCase()] = PureExample2[nameof(Vote).ToSnakeCase()],
            [nameof(Content).ToSnakeCase()] = PureExample2[nameof(Content).ToSnakeCase()],
            [nameof(CreationDate).ToSnakeCase()] = PureExample2[nameof(CreationDate).ToSnakeCase()],
            [nameof(ModifiedDate).ToSnakeCase()] = PureExample2[nameof(ModifiedDate).ToSnakeCase()],
            [nameof(ParentCommentIds).ToSnakeCase()] = PureExample2[nameof(ParentCommentIds).ToSnakeCase()],
            [nameof(Author).ToSnakeCase()] = UserDto.PureExample2,
            [nameof(Post).ToSnakeCase()] = PostDto.PureExample2,
            [nameof(ChildComments).ToSnakeCase()] = new OpenApiNull(),
        };
        public static OpenApiArray ListExample1 { get; } = new OpenApiArray
        {
            Example1, Example2
        };
        public static OpenApiObject PageExample1 { get; } = new OpenApiObject
        {
            [nameof(PageDto<CommentDto>.Pagination).ToSnakeCase()] = PaginationDto.PureExample1,
            [nameof(PageDto<CommentDto>.Items).ToSnakeCase()] = PureListExample1
        };


        public static IOpenApiAny Example { get; } = Example1;
        public static Dictionary<string, IOpenApiAny> ActionExamples { get; } = new Dictionary<string, IOpenApiAny>
        {
            [nameof(CommentController.ListSelfCommentsAsync)] = ListExample1,
            [nameof(CommentController.ListPostCommentsAsync)] = ListExample1,
            [nameof(CommentController.ListChildCommentsAsync)] = ListExample1,
            [nameof(CommentController.GetCommentByIdAsync)] = PureExample1,
            [nameof(CommentController.ReplyToPostAsync)] = PureExample1,
            [nameof(CommentController.ReplyToCommentAsync)] = PureExample1,
            [nameof(CommentController.VoteCommentAsync)] = PureExample1,
            [nameof(CommentController.ToggleVoteForCommentAsync)] = PureExample1,
            [nameof(CommentController.DeleteVoteForCommentAsync)] = PureExample1,
            [nameof(CommentController.PatchCommentByIdAsync)] = PureExample1,
            [nameof(CommentController.DeleteCommentByIdAsync)] = PureExample1,
        };

        static CommentDto()
        {

        }

        public CommentDto(long id, long authorId, string authorUsername,
            long postId, int vote, string content, DateTime creationDate,
            DateTime? modifiedDate = null, List<long> parentCommentIds = null,
            UserDto author = null, PostDto post = null,
            List<CommentDto> childComments = null)
        {
            Id = id;
            AuthorId = authorId;
            AuthorUsername = authorUsername;
            PostId = postId;
            Vote = vote;
            Content = content;
            CreationDate = creationDate;
            ModifiedDate = modifiedDate;
            ParentCommentIds = parentCommentIds;
            Author = author;
            Post = post;
            ChildComments = childComments;
        }
    }
}
