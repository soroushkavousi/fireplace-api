using FireplaceApi.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Interfaces
{
    public interface ICommentVoteRepository
    {
        public Task<List<CommentVote>> ListCommentVotesAsync(List<long> Ids);
        public Task<CommentVote> GetCommentVoteByIdAsync(long id,
            bool includeVoter = false, bool includeComment = false);
        public Task<CommentVote> GetCommentVoteAsync(long voterId,
            long commentId, bool includeVoter = false, bool includeComment = false);
        public Task<CommentVote> CreateCommentVoteAsync(long voterUserId,
            string voterUsername, long commentId, bool isUp);
        public Task<CommentVote> UpdateCommentVoteAsync(
            CommentVote commentVote);
        public Task DeleteCommentVoteByIdAsync(long id);
        public Task<bool> DoesCommentVoteIdExistAsync(long id);
        public Task<bool> DoesCommentVoteIdExistAsync(long voterId, long commentId);
    }
}
