using FireplaceApi.Presentation.Auth;
using FireplaceApi.Presentation.Converters;
using FireplaceApi.Presentation.Dtos;
using FireplaceApi.Presentation.Tools;
using FireplaceApi.Presentation.Validators;
using FireplaceApi.Application.Enums;
using FireplaceApi.Application.Identifiers;
using FireplaceApi.Application.Models;
using FireplaceApi.Application.Services;
using FireplaceApi.Application.Tools;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace FireplaceApi.Presentation.Resolvers;

[ExtendObjectType(typeof(GraphQLQuery))]
public class PostQueryResolvers
{
    [AllowAnonymous]
    public async Task<QueryResultDto<PostDto>> GetPostsAsync(
        [Service(ServiceKind.Resolver)] PostService postService,
        [Service(ServiceKind.Resolver)] PostValidator postValidator,
        [User] RequestingUser requestingUser, [GraphQLNonNullType] string search,
        SortType? sort = null)
    {
        var queryResult = await postService.ListPostsAsync(search, sort, requestingUser?.Id);
        var queryResultDto = queryResult.ToDto();
        return queryResultDto;
    }

    [AllowAnonymous]
    public async Task<PostDto> GetPostAsync(
        [Service(ServiceKind.Resolver)] PostService postService,
        [Service(ServiceKind.Resolver)] PostValidator postValidator,
        [User] RequestingUser requestingUser, [GraphQLNonNullType] string id)
    {
        var ulongId = postValidator.ValidateEncodedIdFormat(id, FieldName.POST_ID).Value;
        var post = await postService.GetPostByIdAsync(ulongId, false, false, requestingUser?.Id);
        var postDto = post.ToDto();
        return postDto;
    }
}

[ExtendObjectType(typeof(CommunityDto))]
public class CommunityPostsQueryResolvers
{
    [AllowAnonymous]
    public async Task<QueryResultDto<PostDto>> GetPostsAsync(
        [Service(ServiceKind.Resolver)] PostService postService,
        [Service(ServiceKind.Resolver)] PostValidator postValidator,
        [User] RequestingUser requestingUser, [Parent] CommunityDto community,
        SortType? sort = null)
    {
        var communityIdentifier = CommunityIdentifier.OfId(community.Id.IdDecode());
        var queryResult = await postService.ListCommunityPostsAsync(communityIdentifier,
            sort, requestingUser?.Id);
        var queryResultDto = queryResult.ToDto();
        return queryResultDto;
    }
}

[ExtendObjectType(typeof(CommentDto))]
public class CommentPostQueryResolvers
{
    [AllowAnonymous]
    public async Task<PostDto> GetPostAsync(
        [Service(ServiceKind.Resolver)] PostService postService,
        [Service(ServiceKind.Resolver)] PostValidator postValidator,
        [User] RequestingUser requestingUser, [Parent] CommentDto comment)
    {
        var post = await postService.GetPostByIdAsync(comment.PostId.IdDecode(),
            false, false, requestingUser?.Id);
        var postDto = post.ToDto();
        return postDto;
    }
}

[ExtendObjectType(typeof(UserDto))]
public class UserPostsQueryResolvers
{
    [AllowAnonymous]
    public async Task<QueryResultDto<PostDto>> GetPostsAsync(
        [Service(ServiceKind.Resolver)] PostService postService,
        [Service(ServiceKind.Resolver)] PostValidator postValidator,
        [User] RequestingUser requestingUser, [Parent] UserDto user,
        SortType? sort = null)
    {
        var queryResult = await postService.ListSelfPostsAsync(requestingUser.Id.Value, sort);
        var queryResultDto = queryResult.ToDto();
        return queryResultDto;
    }
}
