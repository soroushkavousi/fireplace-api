using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FireplaceApi.Infrastructure.Entities
{
    [Index(nameof(UserEntityId), IsUnique = true)]
    [Index(nameof(CommunityEntityId), IsUnique = true)]
    public class CommunityMemberEntity : BaseEntity
    {
        public long UserEntityId { get; set; }
        public long CommunityEntityId { get; set; }
        public long? Id { get; set; }
        public UserEntity UserEntity { get; set; }
        public CommunityEntity CommunityEntity { get; set; }

        private CommunityMemberEntity() : base() { }

        public CommunityMemberEntity(long userEntityId, long communityEntityId,
            DateTime? creationDate = null, DateTime? modifiedDate = null, long? id = null, 
            UserEntity userEntity = null, CommunityEntity communityEntity = null) : base(creationDate, modifiedDate)
        {
            UserEntityId = userEntityId;
            CommunityEntityId = communityEntityId;
            Id = id;
            UserEntity = userEntity ?? throw new ArgumentNullException(nameof(userEntity));
            CommunityEntity = communityEntity ?? throw new ArgumentNullException(nameof(communityEntity));
        }

        public CommunityMemberEntity PureCopy() => new CommunityMemberEntity(UserEntityId, CommunityEntityId,
            CreationDate, ModifiedDate, Id);
    }

    public class CommunityMemberEntityConfiguration : IEntityTypeConfiguration<CommunityMemberEntity>
    {
        public void Configure(EntityTypeBuilder<CommunityMemberEntity> modelBuilder)
        {
            // p => principal / d => dependent

            modelBuilder
                .HasKey(t => new { t.UserEntityId, t.CommunityEntityId });

            modelBuilder
                .HasOne(d => d.UserEntity)
                .WithMany(p => p.JoinedCommunities)
                .HasForeignKey(d => d.UserEntityId)
                .HasPrincipalKey(p => p.Id)
                .IsRequired();

            modelBuilder
                .HasOne(d => d.CommunityEntity)
                .WithMany(p => p.CommunityMemberEntities)
                .HasForeignKey(d => d.CommunityEntityId)
                .HasPrincipalKey(p => p.Id)
                .IsRequired();
        }
    }
}
