using GamingCommunityApi.Core.Enums;
using GamingCommunityApi.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingCommunityApi.Core.Models.UserInformations
{
    public class AccessToken
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Value { get; set; }
        public User User { get; set; }

        public AccessToken(long id, long userId, string value, User user = null)
        {
            Id = id;
            UserId = userId;
            Value = value ?? throw new ArgumentNullException(nameof(value));
            User = user;
        }

        public AccessToken PureCopy() => new AccessToken(Id, UserId, Value, null);

        public void RemoveLoopReferencing()
        {
            User = User?.PureCopy();
        }
    }
}
