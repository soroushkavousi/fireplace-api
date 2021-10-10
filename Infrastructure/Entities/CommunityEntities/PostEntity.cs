using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FireplaceApi.Infrastructure.Entities
{
    [Index(nameof(AuthorEntityId), IsUnique = true)]
    [Index(nameof(CommunityEntityId), IsUnique = true)]
    public class PostEntity : BaseEntity
    {
        public long AuthorEntityId { get; set; }
        public long CommunityEntityId { get; set; }
        public int Vote { get; set; }
        public string Content { get; set; }
        [Column(Order = 0)]
        public long? Id { get; set; }
        public UserEntity AuthorEntity { get; set; }
        public CommunityEntity CommunityEntity { get; set; }
        public List<PostVoteEntity> PostVoteEntities { get; set; }
        public List<CommentEntity> CommentEntities { get; set; }

        private PostEntity() : base() { }

        public PostEntity(long authorEntityId, long communityEntityId,
            string content, int vote, DateTime? creationDate = null,
            DateTime? modifiedDate = null, long? id = null,
            UserEntity author = null,
            CommunityEntity communityEntity = null,
            List<PostVoteEntity> postVoteEntities = null,
            List<CommentEntity> commentEntities = null) : base(creationDate, modifiedDate)
        {
            AuthorEntityId = authorEntityId;
            CommunityEntityId = communityEntityId;
            Content = content ?? throw new ArgumentNullException(nameof(content));
            Vote = vote;
            Id = id;
            AuthorEntity = author;
            CommunityEntity = communityEntity;
            PostVoteEntities = postVoteEntities;
            CommentEntities = commentEntities;
        }

        public PostEntity PureCopy() => new PostEntity(AuthorEntityId, CommunityEntityId,
            Content, Vote, CreationDate, ModifiedDate, Id);
    }

    public class PostEntityConfiguration : IEntityTypeConfiguration<PostEntity>
    {
        public void Configure(EntityTypeBuilder<PostEntity> modelBuilder)
        {
            // p => principal / d => dependent

            modelBuilder
                .HasOne(d => d.AuthorEntity)
                .WithMany(p => p.PostEntities)
                .HasForeignKey(d => d.AuthorEntityId)
                .HasPrincipalKey(p => p.Id)
                .IsRequired();

            modelBuilder
                .HasOne(d => d.CommunityEntity)
                .WithMany(p => p.PostEntities)
                .HasForeignKey(d => d.CommunityEntityId)
                .HasPrincipalKey(p => p.Id)
                .IsRequired();
        }
    }
}
