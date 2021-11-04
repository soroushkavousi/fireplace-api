using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FireplaceApi.Infrastructure.Entities
{
    [Index(nameof(VoterEntityId), IsUnique = false)]
    [Index(nameof(VoterEntityUsername), IsUnique = false)]
    [Index(nameof(CommentEntityId), IsUnique = false)]
    public class CommentVoteEntity : BaseEntity
    {
        public long VoterEntityId { get; set; }
        public string VoterEntityUsername { get; set; }
        public long CommentEntityId { get; set; }
        public bool IsUp { get; set; }
        public long? Id { get; set; }
        public UserEntity VoterEntity { get; set; }
        public CommentEntity CommentEntity { get; set; }

        private CommentVoteEntity() : base() { }

        public CommentVoteEntity(long voterEntityId, string voterEntityUsername,
            long commentEntityId, bool isUp, DateTime? creationDate = null,
            DateTime? modifiedDate = null, long? id = null, UserEntity voterEntity = null,
            CommentEntity commentEntity = null) : base(creationDate, modifiedDate)
        {
            VoterEntityId = voterEntityId;
            VoterEntityUsername = voterEntityUsername;
            CommentEntityId = commentEntityId;
            IsUp = isUp;
            Id = id;
            VoterEntity = voterEntity;
            CommentEntity = commentEntity;
        }

        public CommentVoteEntity PureCopy() => new CommentVoteEntity(VoterEntityId,
            VoterEntityUsername, CommentEntityId, IsUp, CreationDate, ModifiedDate, Id);
    }

    public class CommentVoteEntityConfiguration : IEntityTypeConfiguration<CommentVoteEntity>
    {
        public void Configure(EntityTypeBuilder<CommentVoteEntity> modelBuilder)
        {
            // p => principal / d => dependent

            modelBuilder.DoBaseConfiguration();

            modelBuilder
                .HasOne(d => d.VoterEntity)
                .WithMany(p => p.CommentVoteEntities)
                .HasForeignKey(d => new { d.VoterEntityId, d.VoterEntityUsername })
                .HasPrincipalKey(p => new { p.Id, p.Username })
                .IsRequired();

            modelBuilder
                .HasOne(d => d.CommentEntity)
                .WithMany(p => p.CommentVoteEntities)
                .HasForeignKey(d => d.CommentEntityId)
                .HasPrincipalKey(p => p.Id)
                .IsRequired();

            modelBuilder
                .HasAlternateKey(p =>
                    new { p.VoterEntityId, p.CommentEntityId });
        }
    }
}
