using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace FireplaceApi.Infrastructure.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class CommunityEntity : BaseEntity
    {
        public string Name { get; set; }
        public ulong CreatorEntityId { get; set; }
        public UserEntity CreatorEntity { get; set; }
        public List<CommunityMembershipEntity> CommunityMemberEntities { get; set; }
        public List<PostEntity> PostEntities { get; set; }

        private CommunityEntity() : base() { }

        public CommunityEntity(ulong id, string name, ulong creatorEntityId,
            DateTime? creationDate = null, DateTime? modifiedDate = null,
            UserEntity creatorEntity = null, List<CommunityMembershipEntity> members = null,
            List<PostEntity> postEntities = null) : base(id, creationDate, modifiedDate)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            CreatorEntityId = creatorEntityId;
            CreatorEntity = creatorEntity;
            CommunityMemberEntities = members;
            PostEntities = postEntities;
        }

        public CommunityEntity PureCopy() => new CommunityEntity(Id, Name, CreatorEntityId,
            CreationDate, ModifiedDate);
    }

    public class CommunityEntityConfiguration : IEntityTypeConfiguration<CommunityEntity>
    {
        public void Configure(EntityTypeBuilder<CommunityEntity> modelBuilder)
        {
            // p => principal / d => dependent

            modelBuilder.DoBaseConfiguration();

            modelBuilder
                .HasOne(d => d.CreatorEntity)
                .WithMany(p => p.OwnCommunities)
                .HasForeignKey(d => d.CreatorEntityId)
                .HasPrincipalKey(p => p.Id)
                .IsRequired();
        }
    }
}
