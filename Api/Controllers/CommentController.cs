using FireplaceApi.Api.Converters;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Services;
using Microsoft.AspNetCore.Authorization;
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
        [ProducesResponseType(typeof(PageDto<CommentDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PageDto<CommentDto>>> ListSelfCommentsAsync(
            [BindNever][FromHeader] User requestingUser,
            [FromQuery] ListSelfCommentsInputQueryParameters inputQueryParameters)
        {
            var paginationInputParameters = PageConverter.ConvertToModel(inputQueryParameters);
            var page = await _commentService.ListSelfCommentsAsync(requestingUser,
                paginationInputParameters, inputQueryParameters.Sort,
                inputQueryParameters.StringOfSort);
            var requestPath = HttpContext.Request.Path;
            var pageDto = _commentConverter.ConvertToDto(page, requestPath);
            return pageDto;
        }

        /// <summary>
        /// List post comments.
        /// </summary>
        /// <returns>List of post comments</returns>
        /// <response code="200">Post comments was successfully retrieved.</response>
        [AllowAnonymous]
        [HttpGet("/v{version:apiVersion}/posts/{id}/comments")]
        [ProducesResponseType(typeof(PageDto<CommentDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PageDto<CommentDto>>> ListPostCommentsAsync(
            [BindNever][FromHeader] User requestingUser,
            [FromRoute] ListPostCommentsInputRouteParameters inputRouteParameters,
            [FromQuery] ListPostCommentsInputQueryParameters inputQueryParameters)
        {
            var paginationInputParameters = PageConverter.ConvertToModel(inputQueryParameters);
            var page = await _commentService.ListPostCommentsAsync(requestingUser,
                paginationInputParameters, inputRouteParameters.PostId, inputQueryParameters.Sort,
                inputQueryParameters.StringOfSort);
            var requestPath = HttpContext.Request.Path;
            var pageDto = _commentConverter.ConvertToDto(page, requestPath);
            return pageDto;
        }

        /// <summary>
        /// List child comments.
        /// </summary>
        /// <returns>List of child comments</returns>
        /// <response code="200">Child comments was successfully retrieved.</response>
        [AllowAnonymous]
        [HttpGet("{id}/children")]
        [ProducesResponseType(typeof(List<CommentDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<CommentDto>>> ListChildCommentsAsync(
            [BindNever][FromHeader] User requestingUser,
            [FromRoute] ListChildCommentsInputRouteParameters inputRouteParameters)
        {
            var comments = await _commentService.ListChildCommentsAsync(requestingUser,
                inputRouteParameters.ParentId);
            var commentDtos = comments.Select(c => _commentConverter.ConvertToDto(c)).ToList();
            return commentDtos;
        }

        /// <summary>
        /// Get a single comment by id.
        /// </summary>
        /// <returns>Requested comment</returns>
        /// <response code="200">The comment was successfully retrieved.</response>
        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CommentDto>> GetCommentByIdAsync(
            [BindNever][FromHeader] User requestingUser,
            [FromRoute] GetCommentByIdInputRouteParameters inputRouteParameters,
            [FromQuery] GetCommentInputQueryParameters inputQueryParameters)
        {
            var comment = await _commentService
                .GetCommentByIdAsync(requestingUser, inputRouteParameters.Id,
                    inputQueryParameters.IncludeAuthor, inputQueryParameters.IncludePost);
            var commentDto = _commentConverter.ConvertToDto(comment);
            return commentDto;
        }

        /// <summary>
        /// Create a comment which reply to a post.
        /// </summary>
        /// <returns>Created comment</returns>
        /// <response code="200">Returns the newly created item</response>
        [HttpPost("/v{version:apiVersion}/posts/{id}/comments")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CommentDto>> ReplyToPostAsync(
            [BindNever][FromHeader] User requestingUser,
            [FromRoute] ReplyToPostInputRouteParameters inputRouteParameters,
            [FromBody] ReplyToPostInputBodyParameters inputBodyParameters)
        {
            var comment = await _commentService.ReplyToPostAsync(
                requestingUser, inputRouteParameters.PostId,
                inputBodyParameters.Content);
            var commentDto = _commentConverter.ConvertToDto(comment);
            return commentDto;
        }

        /// <summary>
        /// Create a comment which reply to a comment.
        /// </summary>
        /// <returns>Created comment</returns>
        /// <response code="200">Returns the newly created item</response>
        [HttpPost("{id}/comments")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CommentDto>> ReplyToCommentAsync(
            [BindNever][FromHeader] User requestingUser,
            [FromRoute] ReplyToCommentInputRouteParameters inputRouteParameters,
            [FromBody] ReplyToCommentInputBodyParameters inputBodyParameters)
        {
            var comment = await _commentService.ReplyToCommentAsync(
                requestingUser, inputRouteParameters.Id,
                inputBodyParameters.Content);
            var commentDto = _commentConverter.ConvertToDto(comment);
            return commentDto;
        }

        /// <summary>
        /// Create a vote for the comment.
        /// </summary>
        /// <returns>Voted comment</returns>
        /// <response code="200">Returns the Voted comment</response>
        [HttpPost("{id}/votes")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CommentDto>> VoteCommentAsync(
            [BindNever][FromHeader] User requestingUser,
            [FromRoute] VoteCommentInputRouteParameters inputRouteParameters,
            [FromBody] VoteCommentInputBodyParameters inputBodyParameters)

        {
            var comment = await _commentService.VoteCommentAsync(
                requestingUser, inputRouteParameters.Id, inputBodyParameters.IsUpvote);
            var commentDto = _commentConverter.ConvertToDto(comment);
            return commentDto;
        }

        /// <summary>
        /// Toggle your vote for the comment.
        /// </summary>
        /// <returns>The comment</returns>
        /// <response code="200">Returns the comment</response>
        [HttpPatch("{id}/votes/me")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CommentDto>> ToggleVoteForCommentAsync(
            [BindNever][FromHeader] User requestingUser,
            [FromRoute] ToggleVoteForCommentInputRouteParameters inputRouteParameters)
        {
            var comment = await _commentService.ToggleVoteForCommentAsync(
                requestingUser, inputRouteParameters.Id);
            var commentDto = _commentConverter.ConvertToDto(comment);
            return commentDto;
        }

        /// <summary>
        /// Delete your vote for the comment.
        /// </summary>
        /// <returns>The comment</returns>
        /// <response code="200">Returns the comment</response>
        [HttpDelete("{id}/votes/me")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CommentDto>> DeleteVoteForCommentAsync(
            [BindNever][FromHeader] User requestingUser,
            [FromRoute] DeleteVoteForCommentInputRouteParameters inputRouteParameters)
        {
            var comment = await _commentService.DeleteVoteForCommentAsync(
                requestingUser, inputRouteParameters.Id);
            var commentDto = _commentConverter.ConvertToDto(comment);
            return commentDto;
        }

        /// <summary>
        /// Update a single comment by id.
        /// </summary>
        /// <returns>Updated comment</returns>
        /// <response code="200">The comment was successfully updated.</response>
        [HttpPatch("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CommentDto>> PatchCommentByIdAsync(
            [BindNever][FromHeader] User requestingUser,
            [FromRoute] PatchCommentByIdInputRouteParameters inputRouteParameters,
            [FromBody] PatchCommentInputBodyParameters inputBodyParameters)
        {
            var comment = await _commentService.PatchCommentByIdAsync(requestingUser,
                inputRouteParameters.Id, inputBodyParameters.Content);
            var commentDto = _commentConverter.ConvertToDto(comment);
            return commentDto;
        }

        /// <summary>
        /// Delete a single comment by id.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="200">The comment was successfully deleted.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteCommentByIdAsync(
            [BindNever][FromHeader] User requestingUser,
            [FromRoute] DeleteCommentByIdInputRouteParameters inputRouteParameters)
        {
            await _commentService.DeleteCommentByIdAsync(requestingUser,
                inputRouteParameters.Id);
            return Ok();
        }
    }
}
