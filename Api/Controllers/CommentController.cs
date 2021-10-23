using FireplaceApi.Api.Converters;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireplaceApi.Api.Controllers
{
    [ApiController]
    [ApiVersion("0.1")]
    [Route("v{version:apiVersion}/comments")]
    [Produces("application/json")]
    public class CommentController : ApiController
    {
        private readonly ILogger<CommentController> _logger;
        private readonly CommentConverter _commentConverter;
        private readonly CommentService _commentService;

        public CommentController(ILogger<CommentController> logger,
            CommentConverter commentConverter,
            CommentService commentService)
        {
            _logger = logger;
            _commentConverter = commentConverter;
            _commentService = commentService;
        }

        /// <summary>
        /// List self comments.
        /// </summary>
        /// <returns>List of self comments</returns>
        /// <response code="200">Self comments was successfully retrieved.</response>
        [HttpGet]
        [HttpGet("me")]
        [ProducesResponseType(typeof(PageDto<CommentDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PageDto<CommentDto>>> ListSelfCommentsAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromQuery] ControllerListSelfCommentsInputQueryParameters inputQueryParameters)
        {
            var paginationInputParameters = PageConverter.ConvertToModel(inputQueryParameters);
            var page = await _commentService.ListSelfCommentsAsync(requesterUser,
                paginationInputParameters, inputQueryParameters.Sort);
            var requestPath = HttpContext.Request.Path;
            var pageDto = _commentConverter.ConvertToDto(page, requestPath);
            return pageDto;
        }

        /// <summary>
        /// List post comments.
        /// </summary>
        /// <returns>List of post comments</returns>
        /// <response code="200">Post comments was successfully retrieved.</response>
        [HttpGet("/v{version:apiVersion}/posts/{postId:long}/comments")]
        [ProducesResponseType(typeof(PageDto<CommentDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PageDto<CommentDto>>> ListPostCommentsAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerListPostCommentsInputRouteParameters inputRouteParameters,
            [FromQuery] ControllerListPostCommentsInputQueryParameters inputQueryParameters)
        {
            var paginationInputParameters = PageConverter.ConvertToModel(inputQueryParameters);
            var page = await _commentService.ListPostCommentsAsync(requesterUser,
                paginationInputParameters, inputRouteParameters.PostId, inputQueryParameters.Sort);
            var requestPath = HttpContext.Request.Path;
            var pageDto = _commentConverter.ConvertToDto(page, requestPath);
            return pageDto;
        }

        /// <summary>
        /// List child comments.
        /// </summary>
        /// <returns>List of child comments</returns>
        /// <response code="200">Child comments was successfully retrieved.</response>
        [HttpGet("{parentId:long}/childs")]
        [ProducesResponseType(typeof(List<CommentDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<CommentDto>>> ListChildCommentsAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerListChildCommentsInputRouteParameters inputRouteParameters)
        {
            var comments = await _commentService.ListChildCommentsAsync(requesterUser,
                inputRouteParameters.ParentId);
            var commentDtos = comments.Select(c => _commentConverter.ConvertToDto(c)).ToList();
            return commentDtos;
        }

        /// <summary>
        /// Get a single comment by id.
        /// </summary>
        /// <returns>Requested comment</returns>
        /// <response code="200">The comment was successfully retrieved.</response>
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CommentDto>> GetCommentByIdAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerGetCommentByIdInputRouteParameters inputRouteParameters,
            [FromQuery] ControllerGetCommentInputQueryParameters inputQueryParameters)
        {
            var comment = await _commentService
                .GetCommentByIdAsync(requesterUser, inputRouteParameters.Id,
                inputQueryParameters.IncludeAuthor, inputQueryParameters.IncludePost);
            var commentDto = _commentConverter.ConvertToDto(comment);
            return commentDto;
        }

        /// <summary>
        /// Create a comment which reply to a post.
        /// </summary>
        /// <returns>Created comment</returns>
        /// <response code="200">Returns the newly created item</response>
        [HttpPost("reply-to-post")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CommentDto>> ReplyToPostAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromBody] ControllerReplyToPostInputBodyParameters inputBodyParameters)
        {
            var comment = await _commentService.ReplyToPostAsync(
                requesterUser, inputBodyParameters.PostId,
                inputBodyParameters.Content);
            var commentDto = _commentConverter.ConvertToDto(comment);
            return commentDto;
        }

        /// <summary>
        /// Create a comment which reply to a comment.
        /// </summary>
        /// <returns>Created comment</returns>
        /// <response code="200">Returns the newly created item</response>
        [HttpPost("reply-to-comment")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CommentDto>> ReplyToCommentAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromBody] ControllerReplyToCommentInputBodyParameters inputBodyParameters)
        {
            var comment = await _commentService.ReplyToCommentAsync(
                requesterUser, inputBodyParameters.CommentId,
                inputBodyParameters.Content);
            var commentDto = _commentConverter.ConvertToDto(comment);
            return commentDto;
        }

        /// <summary>
        /// Vote a comment.
        /// </summary>
        /// <returns>Voted comment</returns>
        /// <response code="200">Returns the Voted comment</response>
        [HttpPost("{id:long}/votes")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CommentDto>> VoteCommentAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerVoteCommentInputRouteParameters inputRouteParameters,
            [FromBody] ControllerVoteCommentInputBodyParameters inputBodyParameters)

        {
            var comment = await _commentService.VoteCommentAsync(
                requesterUser, inputRouteParameters.Id, inputBodyParameters.IsUpvote);
            var commentDto = _commentConverter.ConvertToDto(comment);
            return commentDto;
        }

        /// <summary>
        /// Toggle your vote for the comment.
        /// </summary>
        /// <returns>The comment</returns>
        /// <response code="200">Returns the comment</response>
        [HttpPatch("{id:long}/votes/me")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CommentDto>> ToggleVoteForCommentAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerToggleVoteForCommentInputRouteParameters inputRouteParameters)
        {
            var comment = await _commentService.ToggleVoteForCommentAsync(
                requesterUser, inputRouteParameters.Id);
            var commentDto = _commentConverter.ConvertToDto(comment);
            return commentDto;
        }

        /// <summary>
        /// Delete your vote for the comment.
        /// </summary>
        /// <returns>The comment</returns>
        /// <response code="200">Returns the comment</response>
        [HttpDelete("{id:long}/votes/me")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CommentDto>> DeleteVoteForCommentAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerDeleteVoteForCommentInputRouteParameters inputRouteParameters)
        {
            var comment = await _commentService.DeleteVoteForCommentAsync(
                requesterUser, inputRouteParameters.Id);
            var commentDto = _commentConverter.ConvertToDto(comment);
            return commentDto;
        }

        /// <summary>
        /// Update a single comment by id.
        /// </summary>
        /// <returns>Updated comment</returns>
        /// <response code="200">The comment was successfully updated.</response>
        [HttpPatch("{id:long}")]
        [Consumes("application/merge-patch+json")]
        [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CommentDto>> PatchCommentByIdAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerPatchCommentByIdInputRouteParameters inputRouteParameters,
            [FromBody] ControllerPatchCommentInputBodyParameters inputBodyParameters)
        {
            var comment = await _commentService.PatchCommentByIdAsync(requesterUser,
                inputRouteParameters.Id, inputBodyParameters.Content);
            var commentDto = _commentConverter.ConvertToDto(comment);
            return commentDto;
        }

        /// <summary>
        /// Delete a single comment by id.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="200">The comment was successfully deleted.</response>
        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteCommentByIdAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerDeleteCommentByIdInputRouteParameters inputRouteParameters)
        {
            await _commentService.DeleteCommentByIdAsync(requesterUser,
                inputRouteParameters.Id);
            return Ok();
        }
    }
}
