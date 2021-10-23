using System;

namespace FireplaceApi.Core.Models
{
    public class CommentVote : BaseModel
    {
        public long Id { get; set; }
        public long VoterId { get; set; }
        public string VoterUsername { get; set; }
        public long CommentId { get; set; }
        public bool IsUp { get; set; }
        public User Voter { get; set; }
        public Comment Comment { get; set; }

        public CommentVote(long id, long voterId, string voterUsername,
            long commentId, bool isUp, DateTime creationDate,
            DateTime? modifiedDate = null, User voter = null,
            Comment comment = null) : base(creationDate, modifiedDate)
        {
            Id = id;
            VoterId = voterId;
            VoterUsername = voterUsername ?? throw new ArgumentNullException(nameof(voterUsername));
            CommentId = commentId;
            IsUp = isUp;
            Voter = voter;
            Comment = comment;
        }

        public CommentVote PureCopy() => new CommentVote(Id, VoterId,
            VoterUsername, CommentId, IsUp, CreationDate, ModifiedDate);
    }
}
