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
        public long AuthorEntityId { get; set; }
        public string AuthorEntityUsername { get; set; }
        public long PostEntityId { get; set; }
        public int Vote { get; set; }
        public string Content { get; set; }
        public List<long> ParentCommentEntityIds { get; set; }
        public long? Id { get; set; }
        public UserEntity AuthorEntity { get; set; }
        public PostEntity PostEntity { get; set; }

        private CommentEntity() : base() { }

        public CommentEntity(long authorEntityId, string authorEntityUsername,
            long postEntityId, string content, List<long> parentCommentEntityIds = null,
            DateTime? creationDate = null, DateTime? modifiedDate = null,
            long? id = null, int vote = 0, UserEntity authorEntity = null,
            PostEntity postEntity = null) : base(creationDate, modifiedDate)
        {
            AuthorEntityId = authorEntityId;
            AuthorEntityUsername = authorEntityUsername;
            PostEntityId = postEntityId;
            Content = content ?? throw new ArgumentNullException(nameof(content));
            ParentCommentEntityIds = parentCommentEntityIds;
            Id = id;
            Vote = vote;
            AuthorEntity = authorEntity;
            PostEntity = postEntity;
        }

        public CommentEntity PureCopy() => new CommentEntity(AuthorEntityId,
            AuthorEntityUsername, PostEntityId, Content, ParentCommentEntityIds,
            CreationDate, ModifiedDate, Id, Vote);
    }

    public class CommentEntityConfiguration : IEntityTypeConfiguration<CommentEntity>
    {
        public void Configure(EntityTypeBuilder<CommentEntity> modelBuilder)
        {
            // p => principal / d => dependent
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
