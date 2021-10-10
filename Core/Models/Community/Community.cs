using System;

namespace FireplaceApi.Core.Models
{
    public class Community : BaseModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long CreatorId { get; set; }
        public User Creator { get; set; }

        public Community(long id, string name, long creatorId,
            DateTime creationDate, DateTime? modifiedDate = null,
            User creator = null) : base(creationDate, modifiedDate)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            CreatorId = creatorId;
            Creator = creator;
        }

        public Community PureCopy() => new Community(Id, Name, CreatorId,
            CreationDate, ModifiedDate);
    }
}
