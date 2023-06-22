using FireplaceApi.Application.Comments;
using FireplaceApi.Domain.Comments;
using FireplaceApi.Domain.Common;
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
[Route("v{version:apiVersion}/comments")]
[Produces("application/json")]
public class CommentController : ApiController
{
    private readonly CommentService _commentService;

    public CommentController(CommentService commentService)
    {
        _commentService = commentService;
    }

    /// <summary>
    /// List post comments.
    /// </summary>
    /// <returns>List of post comments</returns>
    /// <response code="200">Post comments was successfully retrieved.</response>
    [AllowAnonymous]
    [ProducesResponseType(typeof(QueryResultDto<CommentDto>), StatusCodes.Status200OK)]
    [HttpGet("/v{version:apiVersion}/posts/{id}/comments")]
    public async Task<ActionResult<QueryResultDto<CommentDto>>> ListPostCommentsAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromRoute] ListPostCommentsInputRouteDto inputRouteDto,
        [FromQuery] ListPostCommentsInputQueryDto inputQueryDto)
    {
        var queryResult = await _commentService.ListPostCommentsAsync(
            inputRouteDto.PostId, inputQueryDto.Sort, requestingUser?.Id);
        var queryResultDto = queryResult.ToDto();
        return queryResultDto;
    }

    /// <summary>
    /// Search for comments.
    /// </summary>
    /// <returns>List of comments</returns>
    /// <response code="200">The comments was successfully retrieved.</response>
    [AllowAnonymous]
    [ProducesResponseType(typeof(QueryResultDto<CommentDto>), StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<QueryResultDto<CommentDto>>> ListCommentsAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromQuery] ListCommentsInputQueryDto inputQueryDto)
    {
        var queryResult = new QueryResult<Comment>(null, null);
        if (!inputQueryDto.EncodedIds.IsNullOrEmpty())
        {
            queryResult.Items = await _commentService.ListCommentsByIdsAsync(
                inputQueryDto.Ids, inputQueryDto.Sort, requestingUser?.Id);
        }

        var queryResultDto = queryResult.ToDto();
        return queryResultDto;
    }

    /// <summary>
    /// List self comments.
    /// </summary>
    /// <returns>List of self comments</returns>
    /// <response code="200">The comments was successfully retrieved.</response>
    [Authorize(Policy = AuthConstants.UserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(typeof(QueryResultDto<CommentDto>), StatusCodes.Status200OK)]
    [HttpGet("me")]
    public async Task<ActionResult<QueryResultDto<CommentDto>>> ListSelfCommentsAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromQuery] ListSelfCommentsInputQueryDto inputQueryDto)
    {
        var queryResult = await _commentService.ListSelfCommentsAsync(requestingUser.Id.Value,
            inputQueryDto.Sort);
        var queryResultDto = queryResult.ToDto();
        return queryResultDto;
    }

    /// <summary>
    /// List child comments.
    /// </summary>
    /// <returns>List of child comments</returns>
    /// <response code="200">Child comments was successfully retrieved.</response>
    [AllowAnonymous]
    [ProducesResponseType(typeof(QueryResultDto<CommentDto>), StatusCodes.Status200OK)]
    [HttpGet("{id}/comments")]
    public async Task<ActionResult<QueryResultDto<CommentDto>>> ListChildCommentsAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromRoute] ListChildCommentsInputRouteDto inputRouteDto,
        [FromQuery] ListChildCommentsInputQueryDto inputQueryDto)
    {
        var queryResult = await _commentService.ListChildCommentsAsync(
            inputRouteDto.ParentId, inputQueryDto.Sort, requestingUser?.Id);
        var queryResultDto = queryResult.ToDto();
        return queryResultDto;
    }

    /// <summary>
    /// Get a single comment by id.
    /// </summary>
    /// <returns>Requested comment</returns>
    /// <response code="200">The comment was successfully retrieved.</response>
    [AllowAnonymous]
    [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDto>> GetCommentByIdAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromRoute] GetCommentByIdInputRouteDto inputRouteDto,
        [FromQuery] GetCommentInputQueryDto inputQueryDto)
    {
        var comment = await _commentService.GetCommentByIdAsync(inputRouteDto.Id,
                inputQueryDto.IncludeAuthor, inputQueryDto.IncludePost,
                requestingUser?.Id);
        var commentDto = comment.ToDto();
        return commentDto;
    }

    /// <summary>
    /// Create a comment which reply to a post.
    /// </summary>
    /// <returns>Created comment</returns>
    /// <response code="200">Returns the newly created item</response>
    [Authorize(Policy = AuthConstants.UserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
    [Consumes(Tools.Constants.JsonContentTypeName)]
    [HttpPost("/v{version:apiVersion}/posts/{id}/comments")]
    public async Task<ActionResult<CommentDto>> ReplyToPostAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromRoute] ReplyToPostInputRouteDto inputRouteDto,
        [FromBody] ReplyToPostInputBodyDto inputBodyDto)
    {
        var comment = await _commentService.ReplyToPostAsync(
            requestingUser.Id.Value, inputRouteDto.PostId,
            inputBodyDto.Content);
        var commentDto = comment.ToDto();
        return commentDto;
    }

    /// <summary>
    /// Create a comment which reply to a comment.
    /// </summary>
    /// <returns>Created comment</returns>
    /// <response code="200">Returns the newly created item</response>
    [Authorize(Policy = AuthConstants.UserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
    [Consumes(Tools.Constants.JsonContentTypeName)]
    [HttpPost("{id}/comments")]
    public async Task<ActionResult<CommentDto>> ReplyToCommentAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromRoute] ReplyToCommentInputRouteDto inputRouteDto,
        [FromBody] ReplyToCommentInputBodyDto inputBodyDto)
    {
        var comment = await _commentService.ReplyToCommentAsync(
            requestingUser.Id.Value, inputRouteDto.ParentCommentId,
            inputBodyDto.Content);
        var commentDto = comment.ToDto();
        return commentDto;
    }

    /// <summary>
    /// Create a vote for the comment.
    /// </summary>
    /// <returns>Voted comment</returns>
    /// <response code="200">Returns the Voted comment</response>
    [Authorize(Policy = AuthConstants.UserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
    [Consumes(Tools.Constants.JsonContentTypeName)]
    [HttpPost("{id}/votes")]
    public async Task<ActionResult<CommentDto>> VoteCommentAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromRoute] VoteCommentInputRouteDto inputRouteDto,
        [FromBody] VoteCommentInputBodyDto inputBodyDto)
    {
        var comment = await _commentService.VoteCommentAsync(
            requestingUser.Id.Value, inputRouteDto.Id, inputBodyDto.IsUpvote);
        var commentDto = comment.ToDto();
        return commentDto;
    }

    /// <summary>
    /// Toggle your vote for the comment.
    /// </summary>
    /// <returns>The comment</returns>
    /// <response code="200">Returns the comment</response>
    [Authorize(Policy = AuthConstants.UserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
    [Consumes(Tools.Constants.JsonContentTypeName)]
    [HttpPatch("{id}/votes/me")]
    public async Task<ActionResult<CommentDto>> ToggleVoteForCommentAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromRoute] ToggleVoteForCommentInputRouteDto inputRouteDto)
    {
        var comment = await _commentService.ToggleVoteForCommentAsync(
            requestingUser.Id.Value, inputRouteDto.Id);
        var commentDto = comment.ToDto();
        return commentDto;
    }

    /// <summary>
    /// Delete your vote for the comment.
    /// </summary>
    /// <returns>The comment</returns>
    /// <response code="200">Returns the comment</response>
    [Authorize(Policy = AuthConstants.UserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
    [Consumes(Tools.Constants.JsonContentTypeName)]
    [HttpDelete("{id}/votes/me")]
    public async Task<ActionResult<CommentDto>> DeleteVoteForCommentAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromRoute] DeleteVoteForCommentInputRouteDto inputRouteDto)
    {
        var comment = await _commentService.DeleteVoteForCommentAsync(
            requestingUser.Id.Value, inputRouteDto.Id);
        var commentDto = comment.ToDto();
        return commentDto;
    }

    /// <summary>
    /// Update a single comment by id.
    /// </summary>
    /// <returns>Updated comment</returns>
    /// <response code="200">The comment was successfully updated.</response>
    [Authorize(Policy = AuthConstants.UserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
    [Consumes(Tools.Constants.JsonContentTypeName)]
    [HttpPatch("{id}")]
    public async Task<ActionResult<CommentDto>> PatchCommentByIdAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromRoute] PatchCommentByIdInputRouteDto inputRouteDto,
        [FromBody] PatchCommentInputBodyDto inputBodyDto)
    {
        var comment = await _commentService.PatchCommentByIdAsync(requestingUser.Id.Value,
            inputRouteDto.Id, inputBodyDto.Content);
        var commentDto = comment.ToDto();
        return commentDto;
    }

    /// <summary>
    /// Delete a single comment by id.
    /// </summary>
    /// <returns>No content</returns>
    /// <response code="200">The comment was successfully deleted.</response>
    [Authorize(Policy = AuthConstants.UserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCommentByIdAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromRoute] DeleteCommentByIdInputRouteDto inputRouteDto)
    {
        await _commentService.DeleteCommentByIdAsync(requestingUser.Id.Value,
            inputRouteDto.Id);
        return Ok();
    }
}
