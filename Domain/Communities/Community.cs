using FireplaceApi.Domain.Common;
using FireplaceApi.Domain.Posts;
using FireplaceApi.Domain.Users;

namespace FireplaceApi.Domain.Communities;

public class Community : BaseModel
{
    public string Name { get; set; }
    public ulong CreatorId { get; set; }
    public string CreatorUsername { get; set; }
    public User Creator { get; set; }
    public QueryResult<Post> Posts { get; set; }

    public Community(ulong id, string name, ulong creatorId,
        string creatorUsername, DateTime creationDate, DateTime? modifiedDate = null,
        User creator = null, QueryResult<Post> posts = null)
        : base(id, creationDate, modifiedDate)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        CreatorId = creatorId;
        CreatorUsername = creatorUsername;
        Creator = creator;
        Posts = posts;
    }

    public Community PureCopy() => new(Id, Name, CreatorId,
        CreatorUsername, CreationDate, ModifiedDate);
}
