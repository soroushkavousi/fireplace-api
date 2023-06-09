using FireplaceApi.Application.Converters;
using FireplaceApi.Application.Dtos;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Services;
using FireplaceApi.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Controllers;

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
    [HttpGet("/v{version:apiVersion}/communities/{id-or-name}/posts")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(QueryResultDto<PostDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<QueryResultDto<PostDto>>> ListCommunityPostsAsync(
        [BindNever][FromHeader] User requestingUser,
        [FromRoute] ListCommunityPostsInputRouteDto inputRouteDto,
        [FromQuery] ListCommunityPostsInputQueryDto inputQueryDto)
    {
        var queryResult = await _postService.ListCommunityPostsAsync(
            inputRouteDto.CommunityIdentifier, inputQueryDto.Sort, requestingUser);
        var queryResultDto = queryResult.ToDto();
        return queryResultDto;
    }


    /// <summary>
    /// Search for posts.
    /// </summary>
    /// <returns>List of posts</returns>
    /// <response code="200">All posts was successfully retrieved.</response>
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(QueryResultDto<PostDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<QueryResultDto<PostDto>>> ListPostsAsync(
        [BindNever][FromHeader] User requestingUser,
        [FromQuery] ListPostsInputQueryDto inputQueryDto)
    {
        var queryResult = new QueryResult<Post>(null, null);
        if (!inputQueryDto.Ids.IsNullOrEmpty())
        {
            queryResult.Items = await _postService.ListPostsByIdsAsync(
                inputQueryDto.Ids, requestingUser);
        }
        else
        {
            queryResult = await _postService.ListPostsAsync(inputQueryDto.Search,
                inputQueryDto.Sort, requestingUser);
        }
        var queryResultDto = queryResult.ToDto();
        return queryResultDto;
    }

    /// <summary>
    /// List self posts.
    /// </summary>
    /// <returns>List of posts</returns>
    /// <response code="200">The posts was successfully retrieved.</response>
    [HttpGet("me")]
    [ProducesResponseType(typeof(QueryResultDto<PostDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<QueryResultDto<PostDto>>> ListSelfPostsAsync(
        [BindNever][FromHeader] User requestingUser,
        [FromQuery] ListSelfPostsInputQueryDto inputQueryDto)
    {
        var queryResult = await _postService.ListSelfPostsAsync(requestingUser,
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
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PostDto>> GetPostByIdAsync(
        [BindNever][FromHeader] User requestingUser,
        [FromRoute] GetPostByIdInputRouteDto inputRouteDto,
        [FromQuery] GetPostByIdInputQueryDto inputQueryDto)
    {
        var post = await _postService.GetPostByIdAsync(inputRouteDto.Id,
            inputQueryDto.IncludeAuthor, inputQueryDto.IncludeCommunity,
            requestingUser);
        var postDto = post.ToDto();
        return postDto;
    }

    /// <summary>
    /// Create a post.
    /// </summary>
    /// <returns>Created post</returns>
    /// <response code="200">Returns the newly created item</response>
    [HttpPost("/v{version:apiVersion}/communities/{id-or-name}/posts")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PostDto>> CreatePostAsync(
        [BindNever][FromHeader] User requestingUser,
        [FromRoute] CreatePostInputRouteDto inputRouteDto,
        [FromBody] CreatePostInputBodyDto inputBodyDto)
    {
        var post = await _postService.CreatePostAsync(requestingUser,
            inputRouteDto.CommunityIdentifier, inputBodyDto.Content);
        var postDto = post.ToDto();
        return postDto;
    }

    /// <summary>
    /// Create a vote for the post.
    /// </summary>
    /// <returns>Voted post</returns>
    /// <response code="200">Returns the Voted post</response>
    [HttpPost("{id}/votes")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PostDto>> VotePostAsync(
        [BindNever][FromHeader] User requestingUser,
        [FromRoute] VotePostInputRouteDto inputRouteDto,
        [FromBody] VotePostInputBodyDto inputBodyDto)
    {
        var post = await _postService.VotePostAsync(
            requestingUser, inputRouteDto.Id, inputBodyDto.IsUpvote.Value);
        var postDto = post.ToDto();
        return postDto;
    }

    /// <summary>
    /// Toggle your vote for the post.
    /// </summary>
    /// <returns>The post</returns>
    /// <response code="200">Returns the post</response>
    [HttpPatch("{id}/votes/me")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PostDto>> ToggleVoteForPostAsync(
        [BindNever][FromHeader] User requestingUser,
        [FromRoute] ToggleVoteForPostInputRouteDto inputRouteDto)
    {
        var post = await _postService.ToggleVoteForPostAsync(
            requestingUser, inputRouteDto.Id);
        var postDto = post.ToDto();
        return postDto;
    }

    /// <summary>
    /// Delete your vote for the post.
    /// </summary>
    /// <returns>The post</returns>
    /// <response code="200">Returns the post</response>
    [HttpDelete("{id}/votes/me")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PostDto>> DeleteVoteForPostAsync(
        [BindNever][FromHeader] User requestingUser,
        [FromRoute] DeleteVoteForPostInputRouteDto inputRouteDto)
    {
        var post = await _postService.DeleteVoteForPostAsync(
            requestingUser, inputRouteDto.Id);
        var postDto = post.ToDto();
        return postDto;
    }

    /// <summary>
    /// Update a single post by id.
    /// </summary>
    /// <returns>Updated post</returns>
    /// <response code="200">The post was successfully updated.</response>
    [HttpPatch("{id}")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PostDto>> PatchPostByIdAsync(
        [BindNever][FromHeader] User requestingUser,
        [FromRoute] PatchPostByIdInputRouteDto inputRouteDto,
        [FromBody] PatchPostInputBodyDto inputBodyDto)
    {
        var post = await _postService.PatchPostByIdAsync(requestingUser,
            inputRouteDto.Id, inputBodyDto.Content);
        var postDto = post.ToDto();
        return postDto;
    }

    /// <summary>
    /// Delete a single post by id.
    /// </summary>
    /// <returns>No content</returns>
    /// <response code="200">The post was successfully deleted.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeletePostByIdAsync(
        [BindNever][FromHeader] User requestingUser,
        [FromRoute] DeletePostByIdInputRouteDto inputRouteDto)
    {
        await _postService.DeletePostByIdAsync(requestingUser,
            inputRouteDto.Id);
        return Ok();
    }
}
