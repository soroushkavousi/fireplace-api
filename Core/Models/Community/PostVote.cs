using System;

namespace FireplaceApi.Core.Models
{
    public class PostVote : BaseModel
    {
        public long Id { get; set; }
        public long VoterId { get; set; }
        public string VoterUsername { get; set; }
        public long PostId { get; set; }
        public bool IsUp { get; set; }
        public User Voter { get; set; }
        public Post Post { get; set; }

        public PostVote(long id, long voterId, string voterUsername,
            long postId, bool isUp, DateTime creationDate,
            DateTime? modifiedDate = null, User voter = null,
            Post post = null) : base(creationDate, modifiedDate)
        {
            Id = id;
            VoterId = voterId;
            VoterUsername = voterUsername ?? throw new ArgumentNullException(nameof(voterUsername));
            PostId = postId;
            IsUp = isUp;
            Voter = voter;
            Post = post;
        }

        public PostVote PureCopy() => new PostVote(Id, VoterId,
            VoterUsername, PostId, IsUp, CreationDate, ModifiedDate);
    }
}
