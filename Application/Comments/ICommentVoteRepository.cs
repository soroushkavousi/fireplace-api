using FireplaceApi.Domain.Comments;
using FireplaceApi.Domain.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Comments;

public interface ICommentVoteRepository
{
    public Task<List<CommentVote>> ListCommentVotesAsync(List<ulong> Ids);
    public Task<CommentVote> GetCommentVoteByIdAsync(ulong id,
        bool includeVoter = false, bool includeComment = false);
    public Task<CommentVote> GetCommentVoteAsync(ulong voterId,
        ulong commentId, bool includeVoter = false, bool includeComment = false);
    public Task<CommentVote> CreateCommentVoteAsync(ulong id, ulong voterUserId,
        Username voterUsername, ulong commentId, bool isUp);
    public Task<CommentVote> UpdateCommentVoteAsync(
        CommentVote commentVote);
    public Task DeleteCommentVoteByIdAsync(ulong id);
    public Task<bool> DoesCommentVoteIdExistAsync(ulong id);
    public Task<bool> DoesCommentVoteIdExistAsync(ulong voterId, ulong commentId);
}
