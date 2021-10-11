using System;

namespace FireplaceApi.Core.Models
{
    public class Post : BaseModel
    {
        public long Id { get; set; }
        public long AuthorId { get; set; }
        public string AuthorUsername { get; set; }
        public long CommunityId { get; set; }
        public string CommunityName { get; set; }
        public int Vote { get; set; }
        public string Content { get; set; }
        public User Author { get; set; }
        public Community Community { get; set; }

        public Post(long id, long authorId, string authorUsername, 
            long communityId, string communityName, int vote, 
            string content, DateTime creationDate, DateTime? modifiedDate = null,
            User author = null, Community community = null) 
            : base(creationDate, modifiedDate)
        {
            Id = id;
            AuthorId = authorId;
            AuthorUsername = authorUsername;
            CommunityId = communityId;
            CommunityName = communityName;
            Vote = vote;
            Content = content;
            Author = author;
            Community = community;
        }


        public Post PureCopy() => new Post(Id, AuthorId,
            AuthorUsername, CommunityId, CommunityName, Vote,
            Content, CreationDate, ModifiedDate);
    }
}
