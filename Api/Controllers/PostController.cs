using FireplaceApi.Api.Converters;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FireplaceApi.Api.Controllers
{
    [ApiController]
    [ApiVersion("0.1")]
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
        /// List all posts.
        /// </summary>
        /// <returns>List of posts</returns>
        /// <response code="200">All posts was successfully retrieved.</response>
        [HttpGet]
        [ProducesResponseType(typeof(PageDto<PostDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PageDto<PostDto>>> ListPostsAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromQuery] ControllerListPostsInputQueryParameters inputQueryParameters)
        {
            //var accessTokenValue = FindAccessTokenValue(inputHeaderParameters, inputCookieParameters);
            var paginationInputParameters = PageConverter.ConvertToModel(inputQueryParameters);
            var page = await _postService.ListPostsAsync(requesterUser,
                paginationInputParameters, inputQueryParameters.Self,
                inputQueryParameters.Joined, inputQueryParameters.CommunityId,
                inputQueryParameters.CommunityName, inputQueryParameters.Search,
                inputQueryParameters.Sort);
            var requestPath = HttpContext.Request.Path;
            var pageDto = _postConverter.ConvertToDto(page, requestPath);
            //SetOutputHeaderParameters(postDtos.HeaderParameters);
            return pageDto;
        }

        /// <summary>
        /// Get a single post by id.
        /// </summary>
        /// <returns>Requested post</returns>
        /// <response code="200">The post was successfully retrieved.</response>
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<PostDto>> GetPostByIdAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerGetPostByIdInputRouteParameters inputRouteParameters,
            [FromQuery] ControllerGetPostInputQueryParameters inputQueryParameters)
        {
            var post = await _postService
                .GetPostByIdAsync(requesterUser, inputRouteParameters.Id,
                inputQueryParameters.IncludeAuthor, inputQueryParameters.IncludeCommunity);
            var postDto = _postConverter.ConvertToDto(post);
            return postDto;
        }

        /// <summary>
        /// Create a post.
        /// </summary>
        /// <returns>Created post</returns>
        /// <response code="200">Returns the newly created item</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<PostDto>> CreatePostAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromBody] ControllerCreatePostInputBodyParameters inputBodyParameters)
        {
            var post = await _postService.CreatePostAsync(
                requesterUser, inputBodyParameters.CommunityId,
                inputBodyParameters.CommunityName, inputBodyParameters.Content);
            var postDto = _postConverter.ConvertToDto(post);
            return postDto;
        }

        /// <summary>
        /// Update a single post by id.
        /// </summary>
        /// <returns>Updated post</returns>
        /// <response code="200">The post was successfully updated.</response>
        [HttpPatch("{id:long}")]
        [Consumes("application/merge-patch+json")]
        [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<PostDto>> PatchPostByIdAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerPatchPostByIdInputRouteParameters inputRouteParameters,
            [FromBody] ControllerPatchPostInputBodyParameters inputBodyParameters)
        {
            var post = await _postService.PatchPostByIdAsync(requesterUser,
                inputRouteParameters.Id, inputBodyParameters.Content);
            var postDto = _postConverter.ConvertToDto(post);
            return postDto;
        }

        /// <summary>
        /// Delete a single post by id.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="200">The post was successfully deleted.</response>
        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeletePostByIdAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerDeletePostByIdInputRouteParameters inputRouteParameters)
        {
            await _postService.DeletePostByIdAsync(requesterUser,
                inputRouteParameters.Id);
            return Ok();
        }
    }
}
