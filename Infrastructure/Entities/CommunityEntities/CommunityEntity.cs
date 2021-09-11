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
    [Index(nameof(Name), IsUnique = true)]
    public class CommunityEntity : BaseEntity
    {
        public long CreatorEntityId { get; set; }
        public string Name { get; set; }
        public long? Id { get; set; }
        public UserEntity CreatorEntity { get; set; }
        public List<CommunityMemberEntity> CommunityMemberEntities { get; set; }
        public List<PostEntity> PostEntities { get; set; }

        private CommunityEntity() : base() { }

        public CommunityEntity(long creatorEntityId, string name,
            DateTime? creationDate = null, DateTime? modifiedDate = null,
            long? id = null, UserEntity creatorEntity = null, 
            List<CommunityMemberEntity> members = null,
            List<PostEntity> postEntities = null) : base(creationDate, modifiedDate)
        {
            CreatorEntityId = creatorEntityId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Id = id;
            CreatorEntity = creatorEntity;
            CommunityMemberEntities = members;
            PostEntities = postEntities;
        }

        public CommunityEntity PureCopy() => new CommunityEntity(CreatorEntityId, Name,
            CreationDate, ModifiedDate, Id);
    }

    public class CommunityEntityConfiguration : IEntityTypeConfiguration<CommunityEntity>
    {
        public void Configure(EntityTypeBuilder<CommunityEntity> modelBuilder)
        {
            // p => principal / d => dependent

            modelBuilder
                .HasOne(d => d.CreatorEntity)
                .WithMany(p => p.OwnCommunities)
                .HasForeignKey(d => d.CreatorEntityId)
                .HasPrincipalKey(p => p.Id)
                .IsRequired();
        }
    }
}
