using GamingCommunityApi.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GamingCommunityApi.Infrastructure.Entities.UserInformationEntities
{
    public class AccessTokenEntity
    {
        public long UserEntityId { get; set; }
        public string Value { get; set; }
        public long? Id { get; set; }
        public UserEntity UserEntity { get; set; }

        private AccessTokenEntity() { }

        public AccessTokenEntity(long userEntityId, string value,
             long? id = null, UserEntity userEntity = null)
        {
            UserEntityId = userEntityId;
            Value = value ?? throw new ArgumentNullException(nameof(value));
            Id = id;
            UserEntity = userEntity;
        }

        public AccessTokenEntity PureCopy() => new AccessTokenEntity(UserEntityId, 
            Value, Id, null);

        public void RemoveLoopReferencing()
        {
            UserEntity = UserEntity?.PureCopy();
        }
    }

    public class AccessTokenEntityConfiguration : IEntityTypeConfiguration<AccessTokenEntity>
    {
        public void Configure(EntityTypeBuilder<AccessTokenEntity> modelBuilder)
        {
            // p => principal / d => dependent

            modelBuilder
                .HasIndex(e => e.Value)
                .IsUnique();

            modelBuilder
                .HasOne(d => d.UserEntity)
                .WithMany(p => p.AccessTokenEntities)
                .HasForeignKey(d => d.UserEntityId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
