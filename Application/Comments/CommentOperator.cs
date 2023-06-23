using FireplaceApi.Application.Posts;
using FireplaceApi.Application.Users;
using FireplaceApi.Domain.Comments;
using FireplaceApi.Domain.Users;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Comments;

public class CommentOperator
{
    private readonly ILogger<CommentOperator> _logger;
    private readonly ICommentRepository _commentRepository;
    private readonly ICommentVoteRepository _commentVoteRepository;
    private readonly UserOperator _userOperator;
    private readonly PostOperator _postOperator;

    public CommentOperator(ILogger<CommentOperator> logger,
        ICommentRepository commentRepository,
        ICommentVoteRepository commentVoteRepository,
        UserOperator userOperator, PostOperator postOperator)
    {
        _logger = logger;
        _commentRepository = commentRepository;
        _commentVoteRepository = commentVoteRepository;
        _userOperator = userOperator;
        _postOperator = postOperator;
    }

    public async Task<QueryResult<Comment>> ListPostCommentsAsync(ulong postId,
        CommentSortType? sort = null, ulong? userId = null)
    {
        sort ??= default;
        var postComments = await _commentRepository.ListPostCommentsAsync(
            postId, sort.Value, userId);

        var queryResult = CreateQueryResult(postComments);
        return queryResult;
    }

    public async Task<List<Comment>> ListCommentsByIdsAsync(List<ulong> ids,
        CommentSortType? sort = null, ulong? userId = null)
    {
        sort ??= default;
        if (ids.IsNullOrEmpty())
            return null;

        var comments = await _commentRepository.ListCommentsByIdsAsync(
            ids, sort.Value, userId);
        return comments;
    }

    public async Task<QueryResult<Comment>> ListSelfCommentsAsync(ulong authorId,
        CommentSortType? sort = null)
    {
        sort ??= default;
        var selfComments = await _commentRepository.ListSelfCommentsAsync(
            authorId, sort.Value);

        var queryResult = CreateQueryResult(selfComments);
        return queryResult;
    }

    public async Task<QueryResult<Comment>> ListChildCommentsAsync(ulong parentCommentId,
        ulong? userId = null, CommentSortType? sort = null)
    {
        sort ??= default;
        var childComments = await _commentRepository.ListChildCommentAsync(parentCommentId,
            sort.Value, userId: userId);

        var queryResult = CreateQueryResult(childComments);
        return queryResult;
    }

    public async Task<Comment> GetCommentByIdAsync(ulong id, bool includeAuthor,
        bool includePost, ulong? userId = null)
    {
        var comment = await _commentRepository.GetCommentByIdAsync(
            id, includeAuthor, includePost, userId);

        return comment;
    }

    public async Task<Comment> ReplyToPostAsync(ulong userId,
        ulong postId, string content, Username username = null)
    {
        var id = await IdGenerator.GenerateNewIdAsync(DoesCommentIdExistAsync);
        username ??= await _userOperator.GetUsernameByIdAsync(userId);
        var comment = await _commentRepository.CreateCommentAsync(id,
            userId, username, postId, content);
        return comment;
    }

    public async Task<Comment> ReplyToCommentAsync(ulong userId,
        ulong commentId, string content, Username username = null,
        ulong? postId = null)
    {
        if (postId == null)
        {
            var parentComment = await _commentRepository
                .GetCommentByIdAsync(commentId);
            postId = parentComment.PostId;
        }
        var id = await IdGenerator.GenerateNewIdAsync(DoesCommentIdExistAsync);
        username ??= await _userOperator.GetUsernameByIdAsync(userId);
        var comment = await _commentRepository.CreateCommentAsync(id,
            userId, username, postId.Value, content, commentId);
        return comment;
    }

    public async Task<Comment> VoteCommentAsync(ulong userId,
        ulong id, bool isUp, Username username = null)
    {
        var commentVoteId = await IdGenerator.GenerateNewIdAsync(
            DoesCommentVoteIdExistAsync);
        username ??= await _userOperator.GetUsernameByIdAsync(userId);
        var commentVote = await _commentVoteRepository
            .CreateCommentVoteAsync(commentVoteId, userId,
                username, id, isUp);
        var voteChange = commentVote.IsUp ? +1 : -1;
        var comment = await PatchCommentByIdAsync(userId,
            id, null, voteChange: voteChange);
        return comment;
    }

    public async Task<Comment> ToggleVoteForCommentAsync(ulong userId,
        ulong id)
    {
        var commentVote = await _commentVoteRepository.GetCommentVoteAsync(
            userId, id, includeComment: true);
        commentVote.IsUp = !commentVote.IsUp;
        await _commentVoteRepository.UpdateCommentVoteAsync(commentVote);
        var voteChange = commentVote.IsUp ? +2 : -2;
        var comment = await ApplyCommentChangesAsync(commentVote.Comment,
            null, voteChange: voteChange);
        comment = await GetCommentByIdAsync(comment.Id, false, false,
            userId);
        return comment;
    }

    public async Task<Comment> DeleteVoteForCommentAsync(ulong userId,
        ulong id)
    {
        var commentVote = await _commentVoteRepository.GetCommentVoteAsync(
            userId, id, includeComment: true);
        var voteChange = commentVote.IsUp ? -1 : +1;
        await _commentVoteRepository.DeleteCommentVoteByIdAsync(
            commentVote.Id);
        var comment = await ApplyCommentChangesAsync(commentVote.Comment,
            null, voteChange: voteChange);
        return comment;
    }

    public async Task<Comment> PatchCommentByIdAsync(ulong userId,
        ulong id, string content, int? voteChange)
    {
        var comment = await _commentRepository
            .GetCommentByIdAsync(id);
        comment = await ApplyCommentChangesAsync(comment, content,
            voteChange);
        comment = await GetCommentByIdAsync(comment.Id, false, false,
            userId);
        return comment;
    }

    public async Task DeleteCommentByIdAsync(ulong id)
    {
        await _commentRepository.DeleteCommentByIdAsync(id);
    }

    public async Task<bool> DoesCommentIdExistAsync(ulong id)
    {
        var commentIdExists = await _commentRepository
            .DoesCommentIdExistAsync(id);
        return commentIdExists;
    }

    public async Task<bool> DoesCommentVoteIdExistAsync(ulong commentVoteId)
    {
        var commentIdExists = await _commentVoteRepository
            .DoesCommentVoteIdExistAsync(commentVoteId);
        return commentIdExists;
    }

    public async Task<Comment> ApplyCommentChangesAsync(
        Comment comment, string content, int? voteChange)
    {
        if (content != null)
        {
            comment.Content = content;
        }

        if (voteChange.HasValue)
        {
            comment.Vote += voteChange.Value;
        }

        comment = await _commentRepository
            .UpdateCommentAsync(comment);
        return comment;
    }

    private QueryResult<Comment> CreateQueryResult(List<Comment> totalComments)
    {
        var queryResult = new QueryResult<Comment>(totalComments);

        if (queryResult.Items == null)
            return queryResult;

        foreach (var comment in queryResult.Items)
        {
            LimitCommentView(comment, depth: 1);
        }

        return queryResult;
    }

    private Comment LimitCommentView(Comment comment, int depth)
    {
        if (comment.ChildComments == null)
            return comment;

        if (depth == Configs.Current.QueryResult.DepthLimit)
        {
            comment.MoreChildCommentIds = comment.ChildComments
                .Select(c => c.Id)
                .ToList();

            comment.ChildComments = null;
            return comment;
        }

        var queryResult = new QueryResult<Comment>(comment.ChildComments);
        comment.ChildComments = queryResult.Items;
        comment.MoreChildCommentIds = queryResult.MoreItemIds;

        comment.ChildComments = comment.ChildComments
            .Select(cc => LimitCommentView(cc, depth: depth + 1))
            .ToList();

        return comment;
    }

    //private void SetChilds(List<Comment> parentComments,
    //    List<Comment> childComments)
    //{
    //    var childrenPerParentId = childComments
    //        .GroupBy(cc => cc.ParentCommentIds.Last())
    //        .ToDictionary(g => g.Key, g => g.ToList());
    //    SetChilds(parentComments, childrenPerParentId);
    //}

    //private void SetChilds(List<Comment> parentComments,
    //    Dictionary<ulong, List<Comment>> childrenPerParentId)
    //{
    //    if (parentComments == null || parentComments.Count == 0)
    //        return;
    //    foreach (var pc in parentComments)
    //    {
    //        pc.ChildComments = childrenPerParentId.GetValueOrDefault(pc.Id);
    //        SetChilds(pc.ChildComments, childrenPerParentId);
    //    }
    //}
}
