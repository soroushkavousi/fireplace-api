using System;

namespace FireplaceApi.Core.Models
{
    public class Community : BaseModel
    {
        public string Name { get; set; }
        public ulong CreatorId { get; set; }
        public User Creator { get; set; }

        public Community(ulong id, string name, ulong creatorId,
            DateTime creationDate, DateTime? modifiedDate = null,
            User creator = null) : base(id, creationDate, modifiedDate)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            CreatorId = creatorId;
            Creator = creator;
        }

        public Community PureCopy() => new Community(Id, Name, CreatorId,
            CreationDate, ModifiedDate);
    }
}
