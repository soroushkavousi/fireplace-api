using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FireplaceApi.Infrastructure.Entities
{
    [Index(nameof(UserEntityId), IsUnique = false)]
    [Index(nameof(UserEntityName), IsUnique = false)]
    [Index(nameof(CommunityEntityId), IsUnique = false)]
    [Index(nameof(CommunityEntityName), IsUnique = false)]
    public class CommunityMembershipEntity : BaseEntity
    {
        public ulong UserEntityId { get; set; }
        public string UserEntityName { get; set; }
        public ulong CommunityEntityId { get; set; }
        public string CommunityEntityName { get; set; }
        public UserEntity UserEntity { get; set; }
        public CommunityEntity CommunityEntity { get; set; }

        private CommunityMembershipEntity() : base() { }

        public CommunityMembershipEntity(ulong id, ulong userEntityId, string userEntityName,
            ulong communityEntityId, string communityEntityName,
            DateTime? creationDate = null, DateTime? modifiedDate = null,
            UserEntity userEntity = null, CommunityEntity communityEntity = null)
            : base(id, creationDate, modifiedDate)
        {
            UserEntityId = userEntityId;
            UserEntityName = userEntityName;
            CommunityEntityId = communityEntityId;
            CommunityEntityName = communityEntityName;
            UserEntity = userEntity;
            CommunityEntity = communityEntity;
        }

        public CommunityMembershipEntity PureCopy() => new CommunityMembershipEntity(Id, UserEntityId,
            UserEntityName, CommunityEntityId, CommunityEntityName, CreationDate, ModifiedDate);
    }

    public class CommunityMembershipEntityConfiguration : IEntityTypeConfiguration<CommunityMembershipEntity>
    {
        public void Configure(EntityTypeBuilder<CommunityMembershipEntity> modelBuilder)
        {
            // p => principal / d => dependent

            modelBuilder.DoBaseConfiguration();

            modelBuilder
                .HasOne(d => d.UserEntity)
                .WithMany(p => p.JoinedCommunities)
                .HasForeignKey(d => new { d.UserEntityId, d.UserEntityName })
                .HasPrincipalKey(p => new { p.Id, p.Username })
                .IsRequired();

            modelBuilder
                .HasOne(d => d.CommunityEntity)
                .WithMany(p => p.CommunityMemberEntities)
                .HasForeignKey(d => new { d.CommunityEntityId, d.CommunityEntityName })
                .HasPrincipalKey(p => new { p.Id, p.Name })
                .IsRequired();

            modelBuilder
                .HasAlternateKey(p =>
                    new { p.UserEntityId, p.CommunityEntityId });
        }
    }
}
