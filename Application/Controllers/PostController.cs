using FireplaceApi.Application.Converters;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Services;
using FireplaceApi.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/posts")]
    [Produces("application/json")]
    public class PostController : ApiController
    {
        private readonly ILogger<PostController> _logger;
        private readonly PostConverter _postConverter;
        private readonly PostService _postService;

        public PostController(ILogger<PostController> logger,
            PostConverter postConverter,
            PostService postService)
        {
            _logger = logger;
            _postConverter = postConverter;
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
            [FromRoute] ListCommunityPostsInputRouteParameters inputRouteParameters,
            [FromQuery] ListCommunityPostsInputQueryParameters inputQueryParameters)
        {
            var queryResult = await _postService.ListCommunityPostsAsync(
                inputRouteParameters.CommunityIdentifier, inputQueryParameters.Sort, requestingUser);
            var queryResultDto = _postConverter.ConvertToDto(queryResult);
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
            [FromQuery] ListPostsInputQueryParameters inputQueryParameters)
        {
            var queryResult = new QueryResult<Post>(null, null);
            if (!inputQueryParameters.Ids.IsNullOrEmpty())
            {
                queryResult.Items = await _postService.ListPostsByIdsAsync(
                    inputQueryParameters.Ids, requestingUser);
            }
            else
            {
                queryResult = await _postService.ListPostsAsync(inputQueryParameters.Search,
                    inputQueryParameters.Sort, requestingUser);
            }
            var queryResultDto = _postConverter.ConvertToDto(queryResult);
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
            [FromQuery] ListSelfPostsInputQueryParameters inputQueryParameters)
        {
            var queryResult = await _postService.ListSelfPostsAsync(requestingUser,
                inputQueryParameters.Sort);
            var queryResultDto = _postConverter.ConvertToDto(queryResult);
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
            [FromRoute] GetPostByIdInputRouteParameters inputRouteParameters,
            [FromQuery] GetPostByIdInputQueryParameters inputQueryParameters)
        {
            var post = await _postService.GetPostByIdAsync(inputRouteParameters.Id,
                inputQueryParameters.IncludeAuthor, inputQueryParameters.IncludeCommunity,
                requestingUser);
            var postDto = _postConverter.ConvertToDto(post);
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
            [FromRoute] CreatePostInputRouteParameters inputRouteParameters,
            [FromBody] CreatePostInputBodyParameters inputBodyParameters)
        {
            var post = await _postService.CreatePostAsync(requestingUser,
                inputRouteParameters.CommunityIdentifier, inputBodyParameters.Content);
            var postDto = _postConverter.ConvertToDto(post);
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
            [FromRoute] VotePostInputRouteParameters inputRouteParameters,
            [FromBody] VotePostInputBodyParameters inputBodyParameters)
        {
            var post = await _postService.VotePostAsync(
                requestingUser, inputRouteParameters.Id, inputBodyParameters.IsUpvote.Value);
            var postDto = _postConverter.ConvertToDto(post);
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
            [FromRoute] ToggleVoteForPostInputRouteParameters inputRouteParameters)
        {
            var post = await _postService.ToggleVoteForPostAsync(
                requestingUser, inputRouteParameters.Id);
            var postDto = _postConverter.ConvertToDto(post);
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
            [FromRoute] DeleteVoteForPostInputRouteParameters inputRouteParameters)
        {
            var post = await _postService.DeleteVoteForPostAsync(
                requestingUser, inputRouteParameters.Id);
            var postDto = _postConverter.ConvertToDto(post);
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
            [FromRoute] PatchPostByIdInputRouteParameters inputRouteParameters,
            [FromBody] PatchPostInputBodyParameters inputBodyParameters)
        {
            var post = await _postService.PatchPostByIdAsync(requestingUser,
                inputRouteParameters.Id, inputBodyParameters.Content);
            var postDto = _postConverter.ConvertToDto(post);
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
            [FromRoute] DeletePostByIdInputRouteParameters inputRouteParameters)
        {
            await _postService.DeletePostByIdAsync(requestingUser,
                inputRouteParameters.Id);
            return Ok();
        }
    }
}
