using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace FireplaceApi.Infrastructure.Entities
{
    [Index(nameof(AuthorEntityId), IsUnique = true)]
    [Index(nameof(PostEntityId), IsUnique = true)]
    [Index(nameof(ParentCommentEntityId), IsUnique = true)]
    public class CommentEntity : BaseEntity
    {
        public long AuthorEntityId { get; set; }
        public long PostEntityId { get; set; }
        public string Content { get; set; }
        public int Vote { get; set; }
        public long? ParentCommentEntityId { get; set; }
        public long? Id { get; set; }
        public UserEntity AuthorEntity { get; set; }
        public PostEntity PostEntity { get; set; }
        public CommentEntity ParentCommentEntity { get; set; }
        public List<CommentVoteEntity> CommentVoteEntities { get; set; }
        public List<CommentEntity> ChildCommentEntities { get; set; }

        private CommentEntity() : base() { }

        public CommentEntity(long authorEntityId, long postEntityId,
            string content, int vote, DateTime? creationDate = null,
            DateTime? modifiedDate = null, long? id = null,
            UserEntity author = null, PostEntity postEntity = null,
            List<CommentVoteEntity> commentVoteEntities = null) : base(creationDate, modifiedDate)
        {
            AuthorEntityId = authorEntityId;
            PostEntityId = postEntityId;
            Content = content ?? throw new ArgumentNullException(nameof(content));
            Vote = vote;
            Id = id;
            AuthorEntity = author;
            PostEntity = postEntity;
            CommentVoteEntities = commentVoteEntities;
        }

        public CommentEntity PureCopy() => new CommentEntity(AuthorEntityId, PostEntityId,
            Content, Vote, CreationDate, ModifiedDate, Id);
    }

    public class CommentEntityConfiguration : IEntityTypeConfiguration<CommentEntity>
    {
        public void Configure(EntityTypeBuilder<CommentEntity> modelBuilder)
        {
            // p => principal / d => dependent
            modelBuilder
                .HasOne(d => d.AuthorEntity)
                .WithMany(p => p.CommentEntities)
                .HasForeignKey(d => d.AuthorEntityId)
                .HasPrincipalKey(p => p.Id)
                .IsRequired();

            modelBuilder
                .HasOne(d => d.PostEntity)
                .WithMany(p => p.CommentEntities)
                .HasForeignKey(d => d.PostEntityId)
                .HasPrincipalKey(p => p.Id)
                .IsRequired();

            modelBuilder
                .HasOne(d => d.ParentCommentEntity)
                .WithMany(p => p.ChildCommentEntities)
                .HasForeignKey(d => d.ParentCommentEntityId)
                .HasPrincipalKey(p => p.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
