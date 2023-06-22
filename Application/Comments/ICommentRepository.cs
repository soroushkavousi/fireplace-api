using FireplaceApi.Domain.Comments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Comments;

public interface ICommentRepository
{
    public Task<List<Comment>> ListPostCommentsAsync(ulong postId,
        CommentSortType sort, ulong? userId = null);
    public Task<List<Comment>> ListChildCommentAsync(ulong id, CommentSortType sort,
        ulong? userId = null);
    public Task<List<Comment>> ListCommentsByIdsAsync(
        List<ulong> Ids, CommentSortType sort, ulong? userId = null);
    public Task<List<Comment>> ListSelfCommentsAsync(ulong authorId,
        CommentSortType sort);
    public Task<Comment> GetCommentByIdAsync(ulong id,
        bool includeAuthor = false, bool includePost = false,
        ulong? userId = null);
    public Task<Comment> CreateCommentAsync(ulong id,
        ulong authorUserId, string authorUsername, ulong postId,
        string content, ulong? parentCommentId = null);
    public Task<Comment> UpdateCommentAsync(Comment comment);
    public Task DeleteCommentByIdAsync(ulong id);
    public Task<bool> DoesCommentIdExistAsync(ulong id);
}
