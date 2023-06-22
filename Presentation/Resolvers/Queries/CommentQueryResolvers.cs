using FireplaceApi.Application.Comments;
using FireplaceApi.Domain.Comments;
using FireplaceApi.Domain.Errors;
using FireplaceApi.Presentation.Auth;
using FireplaceApi.Presentation.Converters;
using FireplaceApi.Presentation.Dtos;
using FireplaceApi.Presentation.Tools;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace FireplaceApi.Presentation.Resolvers;

[ExtendObjectType(typeof(GraphQLQuery))]
public class CommentQueryResolvers
{
    [AllowAnonymous]
    public async Task<CommentDto> GetCommentAsync(
        [Service(ServiceKind.Resolver)] CommentService commentService,
        [Service(ServiceKind.Resolver)] Validators.CommentValidator commentValidator,
        [User] RequestingUser requestingUser,
        [GraphQLNonNullType] string id)
    {
        var ulongId = commentValidator.ValidateEncodedIdFormat(id, FieldName.COMMENT_ID).Value;
        var comment = await commentService.GetCommentByIdAsync(ulongId, false, true, requestingUser?.Id);
        var commentDto = comment.ToDto();
        return commentDto;
    }
}

[ExtendObjectType(typeof(PostDto))]
public class PostCommentsQueryResolvers
{
    [AllowAnonymous]
    public async Task<QueryResultDto<CommentDto>> GetCommentsAsync(
        [Service(ServiceKind.Resolver)] CommentService commentService,
        [Service(ServiceKind.Resolver)] Validators.CommentValidator commentValidator,
        [User] RequestingUser requestingUser, [Parent] PostDto post,
        CommentSortType? sort = null)
    {
        var queryResult = await commentService.ListPostCommentsAsync(post.Id.IdDecode(), sort, requestingUser?.Id);
        var queryResultDto = queryResult.ToDto();
        return queryResultDto;
    }
}

[ExtendObjectType(typeof(UserDto))]
public class UserCommentsQueryResolvers
{
    [AllowAnonymous]
    public async Task<QueryResultDto<CommentDto>> GetCommentsAsync(
        [Service(ServiceKind.Resolver)] CommentService commentService,
        [Service(ServiceKind.Resolver)] Validators.CommentValidator commentValidator,
        [User] RequestingUser requestingUser, [Parent] UserDto user,
        CommentSortType? sort = null)
    {
        var queryResult = await commentService.ListSelfCommentsAsync(requestingUser.Id.Value, sort);
        var queryResultDto = queryResult.ToDto();
        return queryResultDto;
    }
}
