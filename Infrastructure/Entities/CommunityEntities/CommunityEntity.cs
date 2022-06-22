using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Infrastructure.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    [Index(nameof(CreatorEntityId), IsUnique = false)]
    [Index(nameof(CreatorEntityUsername), IsUnique = false)]
    public class CommunityEntity : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public ulong CreatorEntityId { get; set; }
        [Required]
        public string CreatorEntityUsername { get; set; }
        public UserEntity CreatorEntity { get; set; }
        public List<CommunityMembershipEntity> CommunityMemberEntities { get; set; }
        public List<PostEntity> PostEntities { get; set; }

        private CommunityEntity() : base() { }

        public CommunityEntity(ulong id, string name, ulong creatorEntityId,
            string creatorEntityUsername, DateTime? creationDate = null, DateTime? modifiedDate = null,
            UserEntity creatorEntity = null, List<CommunityMembershipEntity> members = null,
            List<PostEntity> postEntities = null) : base(id, creationDate, modifiedDate)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            CreatorEntityId = creatorEntityId;
            CreatorEntityUsername = creatorEntityUsername;
            CreatorEntity = creatorEntity;
            CommunityMemberEntities = members;
            PostEntities = postEntities;
        }

        public CommunityEntity PureCopy() => new CommunityEntity(Id, Name,
            CreatorEntityId, CreatorEntityUsername, CreationDate, ModifiedDate);
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
                .HasForeignKey(d => new { d.CreatorEntityId, d.CreatorEntityUsername })
                .HasPrincipalKey(p => new { p.Id, p.Username })
                .IsRequired();
        }
    }
}
