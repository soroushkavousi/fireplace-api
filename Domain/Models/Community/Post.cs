using FireplaceApi.Domain.Enums;
using System;

namespace FireplaceApi.Domain.Models
{
    public class Post : BaseModel
    {
        public ulong AuthorId { get; set; }
        public string AuthorUsername { get; set; }
        public ulong CommunityId { get; set; }
        public string CommunityName { get; set; }
        public int Vote { get; set; }
        public VoteType RequestingUserVote { get; set; }
        public string Content { get; set; }
        public User Author { get; set; }
        public Community Community { get; set; }

        public Post(ulong id, ulong authorId, string authorUsername,
            ulong communityId, string communityName, int vote, VoteType requestingUserVote,
            string content, DateTime creationDate, DateTime? modifiedDate = null,
            User author = null, Community community = null)
            : base(id, creationDate, modifiedDate)
        {
            AuthorId = authorId;
            AuthorUsername = authorUsername;
            CommunityId = communityId;
            CommunityName = communityName;
            Vote = vote;
            RequestingUserVote = requestingUserVote;
            Content = content;
            Author = author;
            Community = community;
        }

        public Post PureCopy() => new(Id, AuthorId,
            AuthorUsername, CommunityId, CommunityName, Vote,
            RequestingUserVote, Content, CreationDate, ModifiedDate);
    }
}
