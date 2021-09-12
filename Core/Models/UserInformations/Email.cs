using FireplaceApi.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Models
{
    public class Email : BaseModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Address { get; set; }
        public Activation Activation { get; set; }
        public User User { get; set; }

        public Email(long id, long userId, string address, 
            Activation activation, DateTime creationDate, 
            DateTime? modifiedDate = null, 
            User user = null) : base(creationDate, modifiedDate)
        {
            Id = id;
            UserId = userId;
            Address = address ?? throw new ArgumentNullException(nameof(address));
            Activation = activation ?? throw new ArgumentNullException(nameof(activation));
            User = user;
        }

        public Email PureCopy() => new Email(Id, UserId, Address, 
            Activation, CreationDate, ModifiedDate, null);

        public void RemoveLoopReferencing()
        {
            User = User?.PureCopy();
        }
    }
}
