using FireplaceApi.Domain.Common;
using FireplaceApi.Domain.Users;

namespace FireplaceApi.Domain.Posts;

public class PostVote : BaseModel
{
    public ulong VoterId { get; set; }
    public Username VoterUsername { get; set; }
    public ulong PostId { get; set; }
    public bool IsUp { get; set; }
    public User Voter { get; set; }
    public Post Post { get; set; }

    public PostVote(ulong id, ulong voterId, Username voterUsername,
        ulong postId, bool isUp, DateTime creationDate,
        DateTime? modifiedDate = null, User voter = null,
        Post post = null) : base(id, creationDate, modifiedDate)
    {
        VoterId = voterId;
        VoterUsername = voterUsername ?? throw new ArgumentNullException(nameof(voterUsername));
        PostId = postId;
        IsUp = isUp;
        Voter = voter;
        Post = post;
    }

    public PostVote PureCopy() => new(Id, VoterId,
        VoterUsername, PostId, IsUp, CreationDate, ModifiedDate);
}
