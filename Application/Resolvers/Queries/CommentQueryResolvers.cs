using FireplaceApi.Application.Controllers;
using FireplaceApi.Application.Converters;
using FireplaceApi.Application.Tool;
using FireplaceApi.Application.Validators;
using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Services;
using FireplaceApi.Domain.Tools;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Resolvers
{
    [ExtendObjectType(typeof(GraphQLQuery))]
    public class CommentQueryResolvers
    {
        [AllowAnonymous]
        public async Task<CommentDto> GetCommentAsync(
            [Service(ServiceKind.Resolver)] CommentService commentService,
            [Service(ServiceKind.Resolver)] CommentValidator commentValidator,
            [Service] CommentConverter commentConverter,
            [User] User requestingUser,
            [GraphQLNonNullType] string id)
        {
            var ulongId = commentValidator.ValidateEncodedIdFormat(id, FieldName.COMMENT_ID).Value;
            var comment = await commentService.GetCommentByIdAsync(ulongId, false, true, requestingUser);
            var commentDto = commentConverter.ConvertToDto(comment);
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
            [Service] CommentConverter commentConverter,
            [User] User requestingUser, [Parent] PostDto post,
            SortType? sort = null)
        {
            var queryResult = await commentService.ListPostCommentsAsync(post.Id.IdDecode(), sort, requestingUser);
            var queryResultDto = commentConverter.ConvertToDto(queryResult);
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
            [Service] CommentConverter commentConverter,
            [User] User requestingUser, [Parent] UserDto user,
            SortType? sort = null)
        {
            var queryResult = await commentService.ListSelfCommentsAsync(requestingUser, sort);
            var queryResultDto = commentConverter.ConvertToDto(queryResult);
            return queryResultDto;
        }
    }
}
