using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Models
{
    public class AccessToken : BaseModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Value { get; set; }
        public User User { get; set; }

        public AccessToken(long id, long userId, string value,
            DateTime creationDate, DateTime? modifiedDate = null, 
            User user = null) : base(creationDate, modifiedDate)
        {
            Id = id;
            UserId = userId;
            Value = value ?? throw new ArgumentNullException(nameof(value));
            User = user;
        }

        public AccessToken PureCopy() => new AccessToken(Id, UserId, Value, 
            CreationDate, ModifiedDate);

        public void RemoveLoopReferencing()
        {
            User = User?.PureCopy();
        }
    }
}
