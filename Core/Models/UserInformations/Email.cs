using GamingCommunityApi.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GamingCommunityApi.Core.Models.UserInformations
{
    public class Email
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Address { get; set; }
        public Activation Activation { get; set; }
        public User User { get; set; }

        public Email(long id, long userId, string address, Activation activation, User user = null)
        {
            Id = id;
            UserId = userId;
            Address = address ?? throw new ArgumentNullException(nameof(address));
            Activation = activation ?? throw new ArgumentNullException(nameof(activation));
            User = user;
        }

        public Email PureCopy() => new Email(Id, UserId, Address, Activation, null);

        public void RemoveLoopReferencing()
        {
            User = User?.PureCopy();
        }
    }
}
