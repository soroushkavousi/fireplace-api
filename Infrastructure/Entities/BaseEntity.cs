using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FireplaceApi.Infrastructure.Entities
{
    public class BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        protected BaseEntity()
        {
            CreationDate = DateTime.UtcNow;
        }

        public BaseEntity(ulong id, DateTime? creationDate = null, DateTime? modifiedDate = null) : this()
        {
            Id = id;
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
               .Property(e => e.Id)
               .ValueGeneratedNever();

            modelBuilder
               .Property(e => e.CreationDate)
               .HasDefaultValueSql("NOW()");
        }
    }
}
