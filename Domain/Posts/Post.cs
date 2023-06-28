using FireplaceApi.Domain.Comments;
using FireplaceApi.Domain.Common;
using FireplaceApi.Domain.Communities;
using FireplaceApi.Domain.Users;

namespace FireplaceApi.Domain.Posts;

public class Post : BaseModel
{
    public ulong AuthorId { get; set; }
    public Username AuthorUsername { get; set; }
    public ulong CommunityId { get; set; }
    public CommunityName CommunityName { get; set; }
    public int Vote { get; set; }
    public VoteType RequestingUserVote { get; set; }
    public string Content { get; set; }
    public User Author { get; set; }
    public Community Community { get; set; }
    public QueryResult<Comment> Comments { get; set; }

    public Post(ulong id, ulong authorId, Username authorUsername,
        ulong communityId, CommunityName communityName, int vote, VoteType requestingUserVote,
        string content, DateTime creationDate, DateTime? modifiedDate = null,
        User author = null, Community community = null,
        QueryResult<Comment> comments = null)
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
        Comments = comments;
    }

    public Post PureCopy() => new(Id, AuthorId,
        AuthorUsername, CommunityId, CommunityName, Vote,
        RequestingUserVote, Content, CreationDate,
        ModifiedDate);
}
