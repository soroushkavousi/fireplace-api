using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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

    public static class BaseEntityExtensions
    {
        public static void DoBaseConfiguration<T>(this EntityTypeBuilder<T> modelBuilder) where T : BaseEntity
        {
            // p => principal / d => dependent / e => entity

            modelBuilder
               .Property(e => e.CreationDate)
               .HasDefaultValueSql("NOW()");
        }
    }
}
