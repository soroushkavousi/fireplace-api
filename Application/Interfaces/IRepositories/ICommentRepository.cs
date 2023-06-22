using FireplaceApi.Application.Enums;
using FireplaceApi.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Interfaces;

public interface ICommentRepository
{
    public Task<List<Comment>> ListPostCommentsAsync(ulong postId,
        SortType sort, ulong? userId = null);
    public Task<List<Comment>> ListChildCommentAsync(ulong id, SortType sort,
        ulong? userId = null);
    public Task<List<Comment>> ListCommentsByIdsAsync(
        List<ulong> Ids, SortType sort, ulong? userId = null);
    public Task<List<Comment>> ListSelfCommentsAsync(ulong authorId,
        SortType sort);
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
