using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Models.UserInformations
{
    public class Session
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public IPAddress IpAddress { get; set; }
        public SessionState State { get; set; }
        public User User { get; set; }

        public Session(long id, long userId, IPAddress ipAddress, 
            SessionState state, User user = null)
        {
            Id = id;
            IpAddress = ipAddress ?? throw new ArgumentNullException(nameof(ipAddress));
            State = state;
            UserId = userId;
            User = user;
        }

        public Session PureCopy() => new Session(Id, UserId, IpAddress, State, null);

        public void RemoveLoopReferencing()
        {
            User = User?.PureCopy();
        }
    }
}