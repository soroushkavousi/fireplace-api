using System;

namespace FireplaceApi.Application.Models;

public class PostVote : BaseModel
{
    public ulong VoterId { get; set; }
    public string VoterUsername { get; set; }
    public ulong PostId { get; set; }
    public bool IsUp { get; set; }
    public User Voter { get; set; }
    public Post Post { get; set; }

    public PostVote(ulong id, ulong voterId, string voterUsername,
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
