using FireplaceApi.Domain.Comments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Comments;

public class CommentService
{
    private readonly IServerLogger<CommentService> _logger;
    private readonly CommentValidator _commentValidator;
    private readonly CommentOperator _commentOperator;

    public CommentService(IServerLogger<CommentService> logger,
        CommentValidator commentValidator, CommentOperator commentOperator)
    {
        _logger = logger;
        _commentValidator = commentValidator;
        _commentOperator = commentOperator;
    }

    public async Task<QueryResult<Comment>> ListPostCommentsAsync(ulong postId,
        CommentSortType? sort, ulong? userId = null)
    {
        await _commentValidator.ValidateListPostCommentsInputParametersAsync(
            postId, sort, userId);
        var queryResult = await _commentOperator.ListPostCommentsAsync(
            postId, sort, userId);
        return queryResult;
    }

    public async Task<List<Comment>> ListCommentsByIdsAsync(List<ulong> ids,
        CommentSortType? sort, ulong? userId = null)
    {
        await _commentValidator.ValidateListCommentsByIdsInputParametersAsync(
            ids, sort, userId);
        var comments = await _commentOperator.ListCommentsByIdsAsync(
            ids, sort, userId);
        return comments;
    }

    public async Task<QueryResult<Comment>> ListSelfCommentsAsync(ulong userId,
        CommentSortType? sort)
    {
        await _commentValidator.ValidateListSelfCommentsInputParametersAsync(userId,
            sort);
        var queryResult = await _commentOperator.ListSelfCommentsAsync(userId,
            sort);
        return queryResult;
    }

    public async Task<QueryResult<Comment>> ListChildCommentsAsync(
        ulong parentCommentId, CommentSortType? sort, ulong? userId = null)
    {
        await _commentValidator.ValidateListChildCommentsAsyncInputParametersAsync(
            parentCommentId, sort, userId);
        var queryResult = await _commentOperator.ListChildCommentsAsync(parentCommentId,
            userId);
        return queryResult;
    }

    public async Task<Comment> GetCommentByIdAsync(ulong id,
        bool? includeAuthor, bool? includePost, ulong? userId = null)
    {
        await _commentValidator.ValidateGetCommentByIdInputParametersAsync(
            id, includeAuthor, includePost, userId);
        var comment = await _commentOperator.GetCommentByIdAsync(
            id, includeAuthor.Value, includePost.Value, userId);
        return comment;
    }

    public async Task<Comment> ReplyToPostAsync(ulong userId, ulong postId,
        string content)
    {
        await _commentValidator.ValidateReplyToPostInputParametersAsync(
            userId, postId, content);
        return await _commentOperator.ReplyToPostAsync(userId,
            postId, content);
    }

    public async Task<Comment> ReplyToCommentAsync(ulong userId,
        ulong parentCommentId, string content)
    {
        await _commentValidator.ValidateReplyToCommentInputParametersAsync(
            userId, parentCommentId, content);
        return await _commentOperator.ReplyToCommentAsync(userId,
            parentCommentId, content);
    }

    public async Task<Comment> VoteCommentAsync(ulong userId,
        ulong id, bool? isUpvote)
    {
        await _commentValidator.ValidateVoteCommentInputParametersAsync(
            userId, id, isUpvote);
        var comment = await _commentOperator.VoteCommentAsync(
            userId, id, isUpvote.Value);
        return comment;
    }

    public async Task<Comment> ToggleVoteForCommentAsync(ulong userId,
        ulong id)
    {
        await _commentValidator.ValidateToggleVoteForCommentInputParametersAsync(
            userId, id);
        var comment = await _commentOperator.ToggleVoteForCommentAsync(
            userId, id);
        return comment;
    }

    public async Task<Comment> DeleteVoteForCommentAsync(ulong userId,
        ulong id)
    {
        await _commentValidator.ValidateDeleteVoteForCommentInputParametersAsync(
            userId, id);
        var comment = await _commentOperator.DeleteVoteForCommentAsync(
            userId, id);
        return comment;
    }

    public async Task<Comment> PatchCommentByIdAsync(ulong userId,
        ulong id, string content)
    {
        await _commentValidator
            .ValidatePatchCommentByIdInputParametersAsync(userId,
                id, content);
        var comment = await _commentOperator.PatchCommentByIdAsync(
            userId, id, content, null);
        return comment;
    }

    public async Task DeleteCommentByIdAsync(ulong userId, ulong id)
    {
        await _commentValidator
            .ValidateDeleteCommentByIdInputParametersAsync(userId, id);
        await _commentOperator.DeleteCommentByIdAsync(id);
    }
}
