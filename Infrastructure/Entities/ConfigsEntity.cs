using FireplaceApi.Infrastructure.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FireplaceApi.Infrastructure.Entities
{
    [Index(nameof(EnvironmentName), IsUnique = true)]
    public class ConfigsEntity : BaseEntity
    {
        [Required]
        public string EnvironmentName { get; set; }
        [Required]
        [Column("Data", TypeName = "jsonb")]
        public ConfigsEntityData Data { get; set; }

        private ConfigsEntity() : base() { }

        public ConfigsEntity(ulong id, string environmentName,
            ConfigsEntityData data, DateTime? creationDate = null,
            DateTime? modifiedDate = null) : base(id, creationDate, modifiedDate)
        {
            EnvironmentName = environmentName;
            Data = data;
        }

        public ConfigsEntity PureCopy() => new(Id, EnvironmentName,
            Data, CreationDate, ModifiedDate);
    }

    public class ConfigsEntityConfiguration : IEntityTypeConfiguration<ConfigsEntity>
    {
        public void Configure(EntityTypeBuilder<ConfigsEntity> modelBuilder)
        {
            modelBuilder.DoBaseConfiguration();
        }
    }
}
