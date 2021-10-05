using FireplaceApi.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Models
{
    public class CommunityMembership : BaseModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Username { get; set; }
        public long CommunityId { get; set; }
        public string CommunityName { get; set; }
        public User User { get; set; }
        public Community Community { get; set; }

        public CommunityMembership(long id, long userId, 
            string username, long communityId, string communityName,
            DateTime creationDate, DateTime? modifiedDate = null,
            User user = null, Community community = null) 
            : base(creationDate, modifiedDate)
        {
            Id = id;
            UserId = userId;
            Username = username;
            CommunityId = communityId;
            CommunityName = communityName;
            User = user;
            Community = community;
        }

        public CommunityMembership PureCopy() => new CommunityMembership(Id, UserId,
            Username, CommunityId, CommunityName, CreationDate, ModifiedDate);
    }
}
