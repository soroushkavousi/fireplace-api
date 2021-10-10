//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using FireplaceApi.Api.Converters;
//using Microsoft.AspNetCore.Mvc.ModelBinding;
//using FireplaceApi.Core.Services;
//using FireplaceApi.Core.Models;

//namespace FireplaceApi.Api.Controllers
//{
//    [ApiController]
//    [ApiVersion("0.1")]
//    [Route("v{version:apiVersion}/community-memberships")]
//    [Produces("application/json")]
//    public class PostController : ApiController
//    {
//        private readonly ILogger<PostController> _logger;
//        private readonly PostConverter _PostConverter;
//        private readonly PostService _PostService;

//        public PostController(ILogger<PostController> logger,
//            PostConverter PostConverter, 
//            PostService PostService)
//        {
//            _logger = logger;
//            _PostConverter = PostConverter;
//            _PostService = PostService;
//        }

//        /// <summary>
//        /// List all community memberships.
//        /// </summary>
//        /// <returns>List of community memberships</returns>
//        /// <response code="200">All community memberships was successfully retrieved.</response>
//        [HttpGet]
//        [ProducesResponseType(typeof(PageDto<PostDto>), StatusCodes.Status200OK)]
//        public async Task<ActionResult<PageDto<PostDto>>> ListPostsAsync(
//            [BindNever][FromHeader] User requesterUser,
//            [FromQuery] ControllerListPostsInputQueryParameters inputQueryParameters)
//        {
//            //var accessTokenValue = FindAccessTokenValue(inputHeaderParameters, inputCookieParameters);
//            var paginationInputParameters = PageConverter.ConvertToModel(inputQueryParameters);
//            var page = await _PostService.ListPostsAsync(requesterUser, 
//                paginationInputParameters);
//            var pageDto = _PostConverter.ConvertToDto(page, "/community-memberships");
//            //SetOutputHeaderParameters(PostDtos.HeaderParameters);
//            return pageDto;
//        }

//        /// <summary>
//        /// Get a single community membership by id.
//        /// </summary>
//        /// <returns>Requested community membership</returns>
//        /// <response code="200">The community membership was successfully retrieved.</response>
//        [HttpGet("{id:long}")]
//        [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
//        public async Task<ActionResult<PostDto>> GetPostByIdAsync(
//            [BindNever][FromHeader] User requesterUser,
//            [FromRoute] ControllerGetPostByIdInputRouteParameters inputRouteParameters,
//            [FromQuery] ControllerGetPostInputQueryParameters inputQueryParameters)
//        {
//            var Post = await _PostService
//                .GetPostByIdAsync(requesterUser, inputRouteParameters.Id, 
//                inputQueryParameters.IncludeCreator, inputQueryParameters.IncludeCommunity);
//            var PostDto = _PostConverter.ConvertToDto(Post);
//            return PostDto;
//        }

//        /// <summary>
//        /// Create a community membership.
//        /// </summary>
//        /// <returns>Created community membership</returns>
//        /// <response code="200">Returns the newly created item</response>
//        [HttpPost]
//        [Consumes("application/json")]
//        [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
//        public async Task<ActionResult<PostDto>> CreatePostAsync(
//            [BindNever][FromHeader] User requesterUser,
//            [FromBody] ControllerCreatePostInputBodyParameters inputBodyParameters)
//        {
//            var Post = await _PostService.CreatePostAsync(
//                requesterUser, inputBodyParameters.CommunityId, inputBodyParameters.CommunityName);
//            var PostDto = _PostConverter.ConvertToDto(Post);
//            return PostDto;
//        }

//        /// <summary>
//        /// Update a single community membership by id.
//        /// </summary>
//        /// <returns>Updated community membership</returns>
//        /// <response code="200">The community membership was successfully updated.</response>
//        [HttpPatch("{id:long}")]
//        [Consumes("application/merge-patch+json")]
//        [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
//        public async Task<ActionResult<PostDto>> PatchPostByIdAsync(
//            [BindNever][FromHeader] User requesterUser,
//            [FromRoute] ControllerPatchPostByIdInputRouteParameters inputRouteParameters,
//            [FromBody] ControllerPatchPostInputBodyParameters inputBodyParameters)
//        {
//            var Post = await _PostService.PatchPostByIdAsync(requesterUser, 
//                inputRouteParameters.Id);
//            var PostDto = _PostConverter.ConvertToDto(Post);
//            return PostDto;
//        }
       
//        /// <summary>
//        /// Delete a single community membership by id.
//        /// </summary>
//        /// <returns>No content</returns>
//        /// <response code="200">The community membership was successfully deleted.</response>
//        [HttpDelete("{id:long}")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        public async Task<IActionResult> DeletePostByIdAsync(
//            [BindNever][FromHeader] User requesterUser,
//            [FromRoute] ControllerDeletePostByIdInputRouteParameters inputRouteParameters)
//        {
//            await _PostService.DeletePostByIdAsync(requesterUser, 
//                inputRouteParameters.Id);
//            return Ok();
//        }
//    }
//}
