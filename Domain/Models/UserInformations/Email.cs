using FireplaceApi.Domain.ValueObjects;
using System;

namespace FireplaceApi.Domain.Models
{
    public class Email : BaseModel
    {
        public ulong UserId { get; set; }
        public string Address { get; set; }
        public Activation Activation { get; set; }
        public User User { get; set; }

        public Email(ulong id, ulong userId, string address,
            Activation activation, DateTime creationDate,
            DateTime? modifiedDate = null,
            User user = null) : base(id, creationDate, modifiedDate)
        {
            UserId = userId;
            Address = address ?? throw new ArgumentNullException(nameof(address));
            Activation = activation ?? throw new ArgumentNullException(nameof(activation));
            User = user;
        }

        public Email PureCopy() => new(Id, UserId, Address,
            Activation, CreationDate, ModifiedDate);

        public void RemoveLoopReferencing()
        {
            User = User?.PureCopy();
        }
    }
}
