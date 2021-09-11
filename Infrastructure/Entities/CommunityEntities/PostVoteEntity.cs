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
    [Index(nameof(VoterEntityId), IsUnique = true)]
    [Index(nameof(PostEntityId), IsUnique = true)]
    public class PostVoteEntity : BaseEntity
    {
        public long VoterEntityId { get; set; }
        public long PostEntityId { get; set; }
        public bool IsUp { get; set; }
        public long? Id { get; set; }
        public UserEntity VoterEntity { get; set; }
        public PostEntity PostEntity { get; set; }

        private PostVoteEntity() : base() { }

        public PostVoteEntity(long voterEntityId, long postEntityId, bool isUp, 
            DateTime? creationDate = null, DateTime? modifiedDate = null, 
            long? id = null, UserEntity voterEntity = null, 
            PostEntity postEntity = null) : base(creationDate, modifiedDate)
        {
            VoterEntityId = voterEntityId;
            PostEntityId = postEntityId;
            IsUp = isUp;
            Id = id;
            VoterEntity = voterEntity ?? throw new ArgumentNullException(nameof(voterEntity));
            PostEntity = postEntity ?? throw new ArgumentNullException(nameof(postEntity));
        }

        public PostVoteEntity PureCopy() => new PostVoteEntity(VoterEntityId, PostEntityId,
            IsUp, CreationDate, ModifiedDate, Id);
    }

    public class PostVoteEntityConfiguration : IEntityTypeConfiguration<PostVoteEntity>
    {
        public void Configure(EntityTypeBuilder<PostVoteEntity> modelBuilder)
        {
            // p => principal / d => dependent

            modelBuilder
                .HasKey(t => new { t.VoterEntityId, t.PostEntityId });

            modelBuilder
                .HasOne(d => d.VoterEntity)
                .WithMany(p => p.PostVoteEntities)
                .HasForeignKey(d => d.VoterEntityId)
                .HasPrincipalKey(p => p.Id)
                .IsRequired();

            modelBuilder
                .HasOne(d => d.PostEntity)
                .WithMany(p => p.PostVoteEntities)
                .HasForeignKey(d => d.PostEntityId)
                .HasPrincipalKey(p => p.Id)
                .IsRequired();
        }
    }
}
