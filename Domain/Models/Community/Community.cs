using System;

namespace FireplaceApi.Domain.Models
{
    public class Community : BaseModel
    {
        public string Name { get; set; }
        public ulong CreatorId { get; set; }
        public string CreatorUsername { get; set; }
        public User Creator { get; set; }

        public Community(ulong id, string name, ulong creatorId,
            string creatorUsername, DateTime creationDate, DateTime? modifiedDate = null,
            User creator = null) : base(id, creationDate, modifiedDate)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            CreatorId = creatorId;
            CreatorUsername = creatorUsername;
            Creator = creator;
        }

        public Community PureCopy() => new(Id, Name, CreatorId,
            CreatorUsername, CreationDate, ModifiedDate);
    }
}
