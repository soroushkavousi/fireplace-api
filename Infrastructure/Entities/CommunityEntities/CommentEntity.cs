using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace FireplaceApi.Infrastructure.Entities
{
    [Index(nameof(AuthorEntityId), IsUnique = false)]
    [Index(nameof(AuthorEntityUsername), IsUnique = false)]
    [Index(nameof(PostEntityId), IsUnique = false)]
    public class CommentEntity : BaseEntity
    {
        public ulong AuthorEntityId { get; set; }
        public string AuthorEntityUsername { get; set; }
        public ulong PostEntityId { get; set; }
        public int Vote { get; set; }
        public string Content { get; set; }
        public List<decimal> ParentCommentEntityIds { get; set; }
        public UserEntity AuthorEntity { get; set; }
        public PostEntity PostEntity { get; set; }
        public List<CommentVoteEntity> CommentVoteEntities { get; set; }

        private CommentEntity() : base() { }

        public CommentEntity(ulong id, ulong authorEntityId, string authorEntityUsername,
            ulong postEntityId, string content, List<decimal> parentCommentEntityIds = null,
            DateTime? creationDate = null, DateTime? modifiedDate = null,
            int vote = 0, UserEntity authorEntity = null,
            PostEntity postEntity = null, List<CommentVoteEntity> commentVoteEntities = null)
            : base(id, creationDate, modifiedDate)
        {
            AuthorEntityId = authorEntityId;
            AuthorEntityUsername = authorEntityUsername;
            PostEntityId = postEntityId;
            Content = content ?? throw new ArgumentNullException(nameof(content));
            ParentCommentEntityIds = parentCommentEntityIds;
            Vote = vote;
            AuthorEntity = authorEntity;
            PostEntity = postEntity;
            CommentVoteEntities = commentVoteEntities;
        }

        public CommentEntity PureCopy() => new CommentEntity(Id, AuthorEntityId,
            AuthorEntityUsername, PostEntityId, Content, ParentCommentEntityIds,
            CreationDate, ModifiedDate, Vote);
    }

    public class CommentEntityConfiguration : IEntityTypeConfiguration<CommentEntity>
    {
        public void Configure(EntityTypeBuilder<CommentEntity> modelBuilder)
        {
            // p => principal / d => dependent

            modelBuilder.DoBaseConfiguration();

            modelBuilder
                .HasOne(d => d.AuthorEntity)
                .WithMany(p => p.CommentEntities)
                .HasForeignKey(d => new { d.AuthorEntityId, d.AuthorEntityUsername })
                .HasPrincipalKey(p => new { p.Id, p.Username })
                .IsRequired();

            modelBuilder
                .HasOne(d => d.PostEntity)
                .WithMany(p => p.CommentEntities)
                .HasForeignKey(d => d.PostEntityId)
                .HasPrincipalKey(p => p.Id)
                .IsRequired();
        }
    }
}
