using FireplaceApi.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FireplaceApi.Infrastructure.Entities
{
    public class GlobalEntity : BaseEntity
    {
        [Required]
        public string EnvironmentName { get; set; }
        [Required]
        [Column("Values", TypeName = "jsonb")]
        public GlobalValues Values { get; set; }

        private GlobalEntity() : base() { }

        public GlobalEntity(ulong id, string environmentName, GlobalValues values,
            DateTime? creationDate = null, DateTime? modifiedDate = null)
            : base(id, creationDate, modifiedDate)
        {
            EnvironmentName = environmentName;
            Values = values ?? throw new ArgumentNullException(nameof(values));
        }

        public GlobalEntity PureCopy() => new GlobalEntity(Id, EnvironmentName,
            Values, CreationDate, ModifiedDate);

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
