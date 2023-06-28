using FireplaceApi.Domain.Posts;
using FireplaceApi.Domain.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Posts;

public interface IPostVoteRepository
{
    public Task<List<PostVote>> ListPostVotesAsync(List<ulong> Ids);
    public Task<PostVote> GetPostVoteByIdAsync(ulong id,
        bool includeVoter = false, bool includePost = false);
    public Task<PostVote> GetPostVoteAsync(ulong voterId,
        ulong postId, bool includeVoter = false, bool includePost = false);
    public Task<PostVote> CreatePostVoteAsync(ulong voterUserId,
        Username voterUsername, ulong postId, bool isUp);
    public Task<PostVote> UpdatePostVoteAsync(
        PostVote postVote);
    public Task DeletePostVoteByIdAsync(ulong id);
    public Task<bool> DoesPostVoteIdExistAsync(ulong id);
    public Task<bool> DoesPostVoteIdExistAsync(ulong voterId, ulong postId);
}
