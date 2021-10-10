using System;

namespace FireplaceApi.Infrastructure.Entities
{
    public class BaseEntity
    {
        public DateTime CreationDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        protected BaseEntity()
        {
            CreationDate = DateTime.UtcNow;
        }

        public BaseEntity(DateTime? creationDate = null, DateTime? modifiedDate = null) : this()
        {
            if (creationDate.HasValue)
                CreationDate = creationDate.Value;
            ModifiedDate = modifiedDate;
        }
    }
}
