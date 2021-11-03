using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FireplaceApi.Infrastructure.Entities
{
    [Index(nameof(VoterEntityId), IsUnique = false)]
    [Index(nameof(VoterEntityUsername), IsUnique = false)]
    [Index(nameof(PostEntityId), IsUnique = false)]
    public class PostVoteEntity : BaseEntity
    {
        public long VoterEntityId { get; set; }
        public string VoterEntityUsername { get; set; }
        public long PostEntityId { get; set; }
        public bool IsUp { get; set; }
        public long? Id { get; set; }
        public UserEntity VoterEntity { get; set; }
        public PostEntity PostEntity { get; set; }

        private PostVoteEntity() : base() { }

        public PostVoteEntity(long voterEntityId, string voterEntityUsername,
            long postEntityId, bool isUp, DateTime? creationDate = null,
            DateTime? modifiedDate = null, long? id = null,
            UserEntity voterEntity = null, PostEntity postEntity = null)
            : base(creationDate, modifiedDate)
        {
            VoterEntityId = voterEntityId;
            VoterEntityUsername = voterEntityUsername;
            PostEntityId = postEntityId;
            IsUp = isUp;
            Id = id;
            VoterEntity = voterEntity;
            PostEntity = postEntity;
        }

        public PostVoteEntity PureCopy() => new PostVoteEntity(VoterEntityId,
            VoterEntityUsername, PostEntityId, IsUp, CreationDate, ModifiedDate, Id);
    }

    public class PostVoteEntityConfiguration : IEntityTypeConfiguration<PostVoteEntity>
    {
        public void Configure(EntityTypeBuilder<PostVoteEntity> modelBuilder)
        {
            // p => principal / d => dependent

            modelBuilder
                .HasOne(d => d.VoterEntity)
                .WithMany(p => p.PostVoteEntities)
                .HasForeignKey(d => new { d.VoterEntityId, d.VoterEntityUsername })
                .HasPrincipalKey(p => new { p.Id, p.Username })
                .IsRequired();

            modelBuilder
                .HasOne(d => d.PostEntity)
                .WithMany(p => p.PostVoteEntities)
                .HasForeignKey(d => d.PostEntityId)
                .HasPrincipalKey(p => p.Id)
                .IsRequired();

            modelBuilder
                .HasAlternateKey(p =>
                    new { p.VoterEntityId, p.PostEntityId });
        }
    }
}
