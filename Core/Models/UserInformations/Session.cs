using GamingCommunityApi.Core.Enums;
using GamingCommunityApi.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GamingCommunityApi.Core.Models.UserInformations
{
    public class Session
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public IPAddress IpAddress { get; set; }
        public SessionState State { get; set; }
        public User User { get; set; }

        public Session(long id, long userId, IPAddress ipAddress, 
            SessionState state, User user)
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
            var pureSession = new Session(Id, UserId, IpAddress,
                State, null);

            if (User != null && User.Sessions.IsNullOrEmpty() == false)
            {
                var index = User.Sessions.FindIndex(
                    session => session.Id == Id);
                if (index != -1)
                    User.Sessions[index] = pureSession;
            }
        }
    }
}