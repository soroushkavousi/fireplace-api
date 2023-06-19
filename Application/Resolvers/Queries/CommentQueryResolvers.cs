using FireplaceApi.Application.Auth;
using FireplaceApi.Application.Converters;
using FireplaceApi.Application.Dtos;
using FireplaceApi.Application.Tools;
using FireplaceApi.Application.Validators;
using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Services;
using FireplaceApi.Domain.Tools;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Resolvers;

[ExtendObjectType(typeof(GraphQLQuery))]
public class CommentQueryResolvers
{
    [AllowAnonymous]
    public async Task<CommentDto> GetCommentAsync(
        [Service(ServiceKind.Resolver)] CommentService commentService,
        [Service(ServiceKind.Resolver)] CommentValidator commentValidator,
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
        [Service(ServiceKind.Resolver)] CommentValidator commentValidator,
        [User] RequestingUser requestingUser, [Parent] PostDto post,
        SortType? sort = null)
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
        [Service(ServiceKind.Resolver)] CommentValidator commentValidator,
        [User] RequestingUser requestingUser, [Parent] UserDto user,
        SortType? sort = null)
    {
        var queryResult = await commentService.ListSelfCommentsAsync(requestingUser.Id.Value, sort);
        var queryResultDto = queryResult.ToDto();
        return queryResultDto;
    }
}
