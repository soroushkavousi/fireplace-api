using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Interfaces
{
    public interface ICommentRepository
    {
        public Task<List<Comment>> ListCommentsAsync(List<ulong> Ids,
            User requestingUser = null);
        public Task<List<ulong>> ListSelfCommentIdsAsync(ulong authorId,
            SortType? sort);
        public Task<List<ulong>> ListPostCommentIdsAsync(ulong postId,
            SortType? sort);
        public Task<List<Comment>> ListChildCommentsAsync(ulong postId,
            List<ulong> parentCommentIds, User requestingUser = null);
        public Task<List<Comment>> ListChildCommentsAsync(ulong postId,
            ulong parentCommentId, User requestingUser = null);
        public Task<Comment> GetCommentByIdAsync(ulong id,
            bool includeAuthor = false, bool includePost = false,
            User requestingUser = null);
        public Task<Comment> CreateCommentAsync(ulong id,
            ulong authorUserId, string authorUsername, ulong postId,
            string content, List<ulong> parentCommentIds = null);
        public Task<Comment> UpdateCommentAsync(
            Comment comment);
        public Task DeleteCommentByIdAsync(ulong id);
        public Task<bool> DoesCommentIdExistAsync(ulong id);
    }
}
