using FireplaceApi.Application.Posts;
using FireplaceApi.Domain.Common;
using FireplaceApi.Domain.Posts;
using FireplaceApi.Domain.Users;
using FireplaceApi.Presentation.Auth;
using FireplaceApi.Presentation.Converters;
using FireplaceApi.Presentation.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace FireplaceApi.Presentation.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/posts")]
[Produces("application/json")]
public class PostController : ApiController
{
    private readonly PostService _postService;

    public PostController(PostService postService)
    {
        _postService = postService;
    }

    /// <summary>
    /// List community posts.
    /// </summary>
    /// <returns>List of community posts</returns>
    /// <response code="200">Community posts was successfully retrieved.</response>
    [AllowAnonymous]
    [ProducesResponseType(typeof(QueryResultDto<PostDto>), StatusCodes.Status200OK)]
    [HttpGet("/v{version:apiVersion}/communities/{id-or-name}/posts")]
    public async Task<ActionResult<QueryResultDto<PostDto>>> ListCommunityPostsAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromRoute] ListCommunityPostsInputRouteDto inputRouteDto,
        [FromQuery] ListCommunityPostsInputQueryDto inputQueryDto)
    {
        var queryResult = await _postService.ListCommunityPostsAsync(
            inputRouteDto.CommunityIdentifier, inputQueryDto.Sort, requestingUser?.Id);
        var queryResultDto = queryResult.ToDto();
        return queryResultDto;
    }


    /// <summary>
    /// Search for posts.
    /// </summary>
    /// <returns>List of posts</returns>
    /// <response code="200">All posts was successfully retrieved.</response>
    [AllowAnonymous]
    [ProducesResponseType(typeof(QueryResultDto<PostDto>), StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<QueryResultDto<PostDto>>> ListPostsAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromQuery] ListPostsInputQueryDto inputQueryDto)
    {
        var queryResult = new QueryResult<Post>(null, null);
        if (!inputQueryDto.Ids.IsNullOrEmpty())
        {
            queryResult.Items = await _postService.ListPostsByIdsAsync(
                inputQueryDto.Ids, requestingUser?.Id);
        }
        else
        {
            queryResult = await _postService.ListPostsAsync(inputQueryDto.Search,
                inputQueryDto.Sort, requestingUser?.Id);
        }
        var queryResultDto = queryResult.ToDto();
        return queryResultDto;
    }

    /// <summary>
    /// List self posts.
    /// </summary>
    /// <returns>List of posts</returns>
    /// <response code="200">The posts was successfully retrieved.</response>
    [Authorize(Policy = AuthConstants.UserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(typeof(QueryResultDto<PostDto>), StatusCodes.Status200OK)]
    [HttpGet("me")]
    public async Task<ActionResult<QueryResultDto<PostDto>>> ListSelfPostsAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromQuery] ListSelfPostsInputQueryDto inputQueryDto)
    {
        var queryResult = await _postService.ListSelfPostsAsync(requestingUser.Id.Value,
            inputQueryDto.Sort);
        var queryResultDto = queryResult.ToDto();
        return queryResultDto;
    }

    /// <summary>
    /// Get a single post by id.
    /// </summary>
    /// <returns>Requested post</returns>
    /// <response code="200">The post was successfully retrieved.</response>
    [AllowAnonymous]
    [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
    [HttpGet("{id}")]
    public async Task<ActionResult<PostDto>> GetPostByIdAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromRoute] GetPostByIdInputRouteDto inputRouteDto,
        [FromQuery] GetPostByIdInputQueryDto inputQueryDto)
    {
        var post = await _postService.GetPostByIdAsync(inputRouteDto.Id,
            inputQueryDto.IncludeAuthor, inputQueryDto.IncludeCommunity,
            requestingUser?.Id);
        var postDto = post.ToDto();
        return postDto;
    }

    /// <summary>
    /// Create a post.
    /// </summary>
    /// <returns>Created post</returns>
    /// <response code="200">Returns the newly created item</response>
    [Authorize(Policy = AuthConstants.UserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
    [Consumes(Tools.Constants.JsonContentTypeName)]
    [HttpPost("/v{version:apiVersion}/communities/{id-or-name}/posts")]
    public async Task<ActionResult<PostDto>> CreatePostAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromRoute] CreatePostInputRouteDto inputRouteDto,
        [FromBody] CreatePostInputBodyDto inputBodyDto)
    {
        var post = await _postService.CreatePostAsync(requestingUser.Id.Value,
            inputRouteDto.CommunityIdentifier, inputBodyDto.Content);
        var postDto = post.ToDto();
        return postDto;
    }

    /// <summary>
    /// Create a vote for the post.
    /// </summary>
    /// <returns>Voted post</returns>
    /// <response code="200">Returns the Voted post</response>
    [Authorize(Policy = AuthConstants.UserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
    [Consumes(Tools.Constants.JsonContentTypeName)]
    [HttpPost("{id}/votes")]
    public async Task<ActionResult<PostDto>> VotePostAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromRoute] VotePostInputRouteDto inputRouteDto,
        [FromBody] VotePostInputBodyDto inputBodyDto)
    {
        var post = await _postService.VotePostAsync(
            requestingUser.Id.Value, inputRouteDto.Id, inputBodyDto.IsUpvote.Value);
        var postDto = post.ToDto();
        return postDto;
    }

    /// <summary>
    /// Toggle your vote for the post.
    /// </summary>
    /// <returns>The post</returns>
    /// <response code="200">Returns the post</response>
    [Authorize(Policy = AuthConstants.UserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
    [Consumes(Tools.Constants.JsonContentTypeName)]
    [HttpPatch("{id}/votes/me")]
    public async Task<ActionResult<PostDto>> ToggleVoteForPostAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromRoute] ToggleVoteForPostInputRouteDto inputRouteDto)
    {
        var post = await _postService.ToggleVoteForPostAsync(
            requestingUser.Id.Value, inputRouteDto.Id);
        var postDto = post.ToDto();
        return postDto;
    }

    /// <summary>
    /// Delete your vote for the post.
    /// </summary>
    /// <returns>The post</returns>
    /// <response code="200">Returns the post</response>
    [Authorize(Policy = AuthConstants.UserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
    [Consumes(Tools.Constants.JsonContentTypeName)]
    [HttpDelete("{id}/votes/me")]
    public async Task<ActionResult<PostDto>> DeleteVoteForPostAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromRoute] DeleteVoteForPostInputRouteDto inputRouteDto)
    {
        var post = await _postService.DeleteVoteForPostAsync(
            requestingUser.Id.Value, inputRouteDto.Id);
        var postDto = post.ToDto();
        return postDto;
    }

    /// <summary>
    /// Update a single post by id.
    /// </summary>
    /// <returns>Updated post</returns>
    /// <response code="200">The post was successfully updated.</response>
    [Authorize(Policy = AuthConstants.UserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
    [Consumes(Tools.Constants.JsonContentTypeName)]
    [HttpPatch("{id}")]
    public async Task<ActionResult<PostDto>> PatchPostByIdAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromRoute] PatchPostByIdInputRouteDto inputRouteDto,
        [FromBody] PatchPostInputBodyDto inputBodyDto)
    {
        var post = await _postService.PatchPostByIdAsync(requestingUser.Id.Value,
            inputRouteDto.Id, inputBodyDto.Content);
        var postDto = post.ToDto();
        return postDto;
    }

    /// <summary>
    /// Delete a single post by id.
    /// </summary>
    /// <returns>No content</returns>
    /// <response code="200">The post was successfully deleted.</response>
    [Authorize(Policy = AuthConstants.UserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePostByIdAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromRoute] DeletePostByIdInputRouteDto inputRouteDto)
    {
        await _postService.DeletePostByIdAsync(requestingUser.Id.Value,
            inputRouteDto.Id);
        return Ok();
    }
}
