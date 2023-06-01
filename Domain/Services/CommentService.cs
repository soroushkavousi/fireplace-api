using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Operators;
using FireplaceApi.Domain.Validators;
using FireplaceApi.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Services;

public class CommentService
{
    private readonly ILogger<CommentService> _logger;
    private readonly CommentValidator _commentValidator;
    private readonly CommentOperator _commentOperator;

    public CommentService(ILogger<CommentService> logger,
        CommentValidator commentValidator, CommentOperator commentOperator)
    {
        _logger = logger;
        _commentValidator = commentValidator;
        _commentOperator = commentOperator;
    }

    public async Task<QueryResult<Comment>> ListPostCommentsAsync(ulong postId,
        SortType? sort, User requestingUser = null)
    {
        await _commentValidator.ValidateListPostCommentsInputParametersAsync(
            postId, sort, requestingUser);
        var queryResult = await _commentOperator.ListPostCommentsAsync(
            postId, sort, requestingUser);
        return queryResult;
    }

    public async Task<List<Comment>> ListCommentsByIdsAsync(List<ulong> ids,
        SortType? sort, User requestingUser = null)
    {
        await _commentValidator.ValidateListCommentsByIdsInputParametersAsync(
            ids, sort, requestingUser);
        var comments = await _commentOperator.ListCommentsByIdsAsync(
            ids, sort, requestingUser);
        return comments;
    }

    public async Task<QueryResult<Comment>> ListSelfCommentsAsync(User requestingUser,
        SortType? sort)
    {
        await _commentValidator.ValidateListSelfCommentsInputParametersAsync(requestingUser,
            sort);
        var queryResult = await _commentOperator.ListSelfCommentsAsync(requestingUser,
            sort);
        return queryResult;
    }

    public async Task<QueryResult<Comment>> ListChildCommentsAsync(
        ulong parentCommentId, SortType? sort, User requestingUser = null)
    {
        await _commentValidator.ValidateListChildCommentsAsyncInputParametersAsync(
            parentCommentId, sort, requestingUser);
        var queryResult = await _commentOperator.ListChildCommentsAsync(requestingUser,
            parentCommentId);
        return queryResult;
    }

    public async Task<Comment> GetCommentByIdAsync(ulong id,
        bool? includeAuthor, bool? includePost, User requestingUser = null)
    {
        await _commentValidator.ValidateGetCommentByIdInputParametersAsync(
            id, includeAuthor, includePost, requestingUser);
        var comment = await _commentOperator.GetCommentByIdAsync(
            id, includeAuthor.Value, includePost.Value, requestingUser);
        return comment;
    }

    public async Task<Comment> ReplyToPostAsync(User requestingUser, ulong postId,
        string content)
    {
        await _commentValidator.ValidateReplyToPostInputParametersAsync(
            requestingUser, postId, content);
        return await _commentOperator.ReplyToPostAsync(requestingUser,
            postId, content);
    }

    public async Task<Comment> ReplyToCommentAsync(User requestingUser,
        ulong parentCommentId, string content)
    {
        await _commentValidator.ValidateReplyToCommentInputParametersAsync(
            requestingUser, parentCommentId, content);
        return await _commentOperator.ReplyToCommentAsync(requestingUser,
            parentCommentId, content);
    }

    public async Task<Comment> VoteCommentAsync(User requestingUser,
        ulong id, bool? isUpvote)
    {
        await _commentValidator.ValidateVoteCommentInputParametersAsync(
            requestingUser, id, isUpvote);
        var comment = await _commentOperator.VoteCommentAsync(
            requestingUser, id, isUpvote.Value);
        return comment;
    }

    public async Task<Comment> ToggleVoteForCommentAsync(User requestingUser,
        ulong id)
    {
        await _commentValidator.ValidateToggleVoteForCommentInputParametersAsync(
            requestingUser, id);
        var comment = await _commentOperator.ToggleVoteForCommentAsync(
            requestingUser, id);
        return comment;
    }

    public async Task<Comment> DeleteVoteForCommentAsync(User requestingUser,
        ulong id)
    {
        await _commentValidator.ValidateDeleteVoteForCommentInputParametersAsync(
            requestingUser, id);
        var comment = await _commentOperator.DeleteVoteForCommentAsync(
            requestingUser, id);
        return comment;
    }

    public async Task<Comment> PatchCommentByIdAsync(User requestingUser,
        ulong id, string content)
    {
        await _commentValidator
            .ValidatePatchCommentByIdInputParametersAsync(requestingUser,
                id, content);
        var comment = await _commentOperator.PatchCommentByIdAsync(
            requestingUser, id, content, null);
        return comment;
    }

    public async Task DeleteCommentByIdAsync(User requestingUser, ulong id)
    {
        await _commentValidator
            .ValidateDeleteCommentByIdInputParametersAsync(requestingUser, id);
        await _commentOperator.DeleteCommentByIdAsync(id);
    }
}
