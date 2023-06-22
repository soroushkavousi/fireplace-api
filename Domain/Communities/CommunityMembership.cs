using FireplaceApi.Domain.Common;
using FireplaceApi.Domain.Users;

namespace FireplaceApi.Domain.Communities;

public class CommunityMembership : BaseModel
{
    public ulong UserId { get; set; }
    public string Username { get; set; }
    public ulong CommunityId { get; set; }
    public string CommunityName { get; set; }
    public User User { get; set; }
    public Community Community { get; set; }

    public CommunityMembership(ulong id, ulong userId,
        string username, ulong communityId, string communityName,
        DateTime creationDate, DateTime? modifiedDate = null,
        User user = null, Community community = null)
        : base(id, creationDate, modifiedDate)
    {
        UserId = userId;
        Username = username;
        CommunityId = communityId;
        CommunityName = communityName;
        User = user;
        Community = community;
    }

    public CommunityMembership PureCopy() => new(Id, UserId,
        Username, CommunityId, CommunityName, CreationDate, ModifiedDate);
}
