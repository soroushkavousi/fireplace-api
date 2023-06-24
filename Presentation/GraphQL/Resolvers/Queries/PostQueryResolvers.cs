using FireplaceApi.Application.Posts;
using FireplaceApi.Domain.Communities;
using FireplaceApi.Domain.Errors;
using FireplaceApi.Domain.Posts;
using FireplaceApi.Presentation.Auth;
using FireplaceApi.Presentation.Converters;
using FireplaceApi.Presentation.Dtos;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace FireplaceApi.Presentation.GraphQL;

[ExtendObjectType(typeof(GraphQLQuery))]
public class PostQueryResolvers
{
    [AllowAnonymous]
    public async Task<QueryResultDto<PostDto>> GetPostsAsync(
        [Service(ServiceKind.Resolver)] PostService postService,
        [Service(ServiceKind.Resolver)] Validators.PostValidator postValidator,
        [User] RequestingUser requestingUser, [GraphQLNonNullType] string search,
        PostSortType? sort = null)
    {
        var queryResult = await postService.ListPostsAsync(search, sort, requestingUser?.Id);
        var queryResultDto = queryResult.ToDto();
        return queryResultDto;
    }

    [AllowAnonymous]
    public async Task<PostDto> GetPostAsync(
        [Service(ServiceKind.Resolver)] PostService postService,
        [Service(ServiceKind.Resolver)] Validators.PostValidator postValidator,
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
        [Service(ServiceKind.Resolver)] Validators.PostValidator postValidator,
        [User] RequestingUser requestingUser, [Parent] CommunityDto community,
        PostSortType? sort = null)
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
        [Service(ServiceKind.Resolver)] Validators.PostValidator postValidator,
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
        [Service(ServiceKind.Resolver)] Validators.PostValidator postValidator,
        [User] RequestingUser requestingUser, [Parent] UserDto user,
        PostSortType? sort = null)
    {
        var queryResult = await postService.ListSelfPostsAsync(requestingUser.Id.Value, sort);
        var queryResultDto = queryResult.ToDto();
        return queryResultDto;
    }
}
