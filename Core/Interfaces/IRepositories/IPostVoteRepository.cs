using FireplaceApi.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Interfaces
{
    public interface IPostVoteRepository
    {
        public Task<List<PostVote>> ListPostVotesAsync(List<long> Ids);
        public Task<PostVote> GetPostVoteByIdAsync(long id,
            bool includeVoter = false, bool includePost = false);
        public Task<PostVote> GetPostVoteAsync(long voterId,
            long postId, bool includeVoter = false, bool includePost = false);
        public Task<PostVote> CreatePostVoteAsync(long voterUserId,
            string voterUsername, long postId, bool isUp);
        public Task<PostVote> UpdatePostVoteAsync(
            PostVote postVote);
        public Task DeletePostVoteByIdAsync(long id);
        public Task<bool> DoesPostVoteIdExistAsync(long id);
        public Task<bool> DoesPostVoteIdExistAsync(long voterId, long postId);
    }
}
