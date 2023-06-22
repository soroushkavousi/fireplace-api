using FireplaceApi.Domain.Common;
using FireplaceApi.Domain.Users;

namespace FireplaceApi.Domain.Comments;

public class CommentVote : BaseModel
{
    public ulong VoterId { get; set; }
    public string VoterUsername { get; set; }
    public ulong CommentId { get; set; }
    public bool IsUp { get; set; }
    public User Voter { get; set; }
    public Comment Comment { get; set; }

    public CommentVote(ulong id, ulong voterId, string voterUsername,
        ulong commentId, bool isUp, DateTime creationDate,
        DateTime? modifiedDate = null, User voter = null,
        Comment comment = null) : base(id, creationDate, modifiedDate)
    {
        Id = id;
        VoterId = voterId;
        VoterUsername = voterUsername ?? throw new ArgumentNullException(nameof(voterUsername));
        CommentId = commentId;
        IsUp = isUp;
        Voter = voter;
        Comment = comment;
    }

    public CommentVote PureCopy() => new(Id, VoterId,
        VoterUsername, CommentId, IsUp, CreationDate, ModifiedDate);
}
