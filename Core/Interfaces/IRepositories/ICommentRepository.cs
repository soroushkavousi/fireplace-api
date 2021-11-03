using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Interfaces
{
    public interface ICommentRepository
    {
        public Task<List<Comment>> ListCommentsAsync(List<long> Ids,
            User requesterUser = null);
        public Task<List<long>> ListSelfCommentIdsAsync(long authorId,
            SortType? sort);
        public Task<List<long>> ListPostCommentIdsAsync(long postId,
            SortType? sort);
        public Task<List<Comment>> ListChildCommentsAsync(long postId,
            List<long> parentCommentIds, User requesterUser = null);
        public Task<List<Comment>> ListChildCommentsAsync(long postId,
            long parentCommentId, User requesterUser = null);
        public Task<Comment> GetCommentByIdAsync(long id,
            bool includeAuthor = false, bool includePost = false,
            User requesterUser = null);
        public Task<Comment> CreateCommentAsync(long authorUserId,
            string authorUsername, long postId, string content,
            List<long> parentCommentIds = null);
        public Task<Comment> UpdateCommentAsync(
            Comment comment);
        public Task DeleteCommentByIdAsync(long id);
        public Task<bool> DoesCommentIdExistAsync(long id);
    }
}
