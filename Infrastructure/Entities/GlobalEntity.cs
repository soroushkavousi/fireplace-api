using FireplaceApi.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FireplaceApi.Infrastructure.Entities
{
    public class GlobalEntity : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Column("Values", TypeName = "jsonb")]
        public GlobalValues Values { get; set; }

        private GlobalEntity() : base() { }

        public GlobalEntity(int id, GlobalValues values, DateTime? creationDate = null,
            DateTime? modifiedDate = null) : base(creationDate, modifiedDate)
        {
            Id = id;
            Values = values ?? throw new ArgumentNullException(nameof(values));
        }

        public GlobalEntity PureCopy() => new GlobalEntity(Id, Values, CreationDate, ModifiedDate);

        public void RemoveLoopReferencing()
        {

        }
    }

    public class GlobalEntityConfiguration : IEntityTypeConfiguration<GlobalEntity>
    {
        public void Configure(EntityTypeBuilder<GlobalEntity> modelBuilder)
        {
            // p => principal / d => dependent / v => value

            modelBuilder.DoBaseConfiguration();

        }
    }
}
