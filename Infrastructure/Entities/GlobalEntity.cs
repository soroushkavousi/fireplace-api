using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FireplaceApi.Core.ValueObjects;

namespace FireplaceApi.Infrastructure.Entities
{
    public class GlobalEntity : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Column("Values", TypeName = "jsonb")]
        public GlobalValues Values { get; set; }

        private GlobalEntity() : base() { }

        public GlobalEntity(int id, GlobalValues values) : base()
        {
            Id = id;
            Values = values ?? throw new ArgumentNullException(nameof(values));
        }

        public GlobalEntity PureCopy() => new GlobalEntity(Id, Values);

        public void RemoveLoopReferencing()
        {

        }
    }

    public class GlobalEntityConfiguration : IEntityTypeConfiguration<GlobalEntity>
    {
        public void Configure(EntityTypeBuilder<GlobalEntity> modelBuilder)
        {
            // p => principal / d => dependent / v => value
        }
    }
}
