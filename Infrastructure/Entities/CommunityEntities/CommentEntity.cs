using FireplaceApi.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace FireplaceApi.Infrastructure.Entities;

[Index(nameof(AuthorEntityId), IsUnique = false)]
[Index(nameof(AuthorEntityUsername), IsUnique = false)]
[Index(nameof(PostEntityId), IsUnique = false)]
public class CommentEntity : BaseEntity
{
    public ulong AuthorEntityId { get; set; }
    [Required]
    public Username AuthorEntityUsername { get; set; }
    public ulong PostEntityId { get; set; }
    public ulong? ParentCommentEntityId { get; set; }
    public int Vote { get; set; }
    [Required]
    public string Content { get; set; }
    public UserEntity AuthorEntity { get; set; }
    public PostEntity PostEntity { get; set; }
    public CommentEntity ParentCommentEntity { get; set; }
    public List<CommentEntity> ChildCommentEntities { get; set; }
    public virtual List<CommentVoteEntity> CommentVoteEntities { get; set; }
    [NotMapped]
    public VoteType RequestingUserVote { get; set; }

    private CommentEntity() : base() { }

    public CommentEntity(ulong id, ulong authorEntityId, Username authorEntityUsername,
        ulong postEntityId, string content, ulong? parentCommentEntityId = null,
        int vote = 0, VoteType requestingUserVote = VoteType.NEUTRAL,
        DateTime? creationDate = null, DateTime? modifiedDate = null,
        UserEntity authorEntity = null, PostEntity postEntity = null,
        CommentEntity parentCommentEntity = null,
        List<CommentEntity> childCommentEntities = null,
        List<CommentVoteEntity> commentVoteEntities = null)
        : base(id, creationDate, modifiedDate)
    {
        AuthorEntityId = authorEntityId;
        AuthorEntityUsername = authorEntityUsername;
        PostEntityId = postEntityId;
        ParentCommentEntityId = parentCommentEntityId;
        Vote = vote;
        RequestingUserVote = requestingUserVote;
        Content = content ?? throw new ArgumentNullException(nameof(content));
        AuthorEntity = authorEntity;
        PostEntity = postEntity;
        ParentCommentEntity = parentCommentEntity;
        ChildCommentEntities = childCommentEntities;
        CommentVoteEntities = commentVoteEntities;
    }

    public void CheckRequestingUserVote(ulong? userId)
    {
        if (userId == null)
            return;

        var requestingUserVote = VoteType.NEUTRAL;
        if (CommentVoteEntities != null)
        {
            var requestingUserVoteEntity = CommentVoteEntities
                .SingleOrDefault(cve => cve.VoterEntityId == userId);
            if (requestingUserVoteEntity != null)
            {
                if (requestingUserVoteEntity.IsUp)
                    requestingUserVote = VoteType.UPVOTE;
                else
                    requestingUserVote = VoteType.DOWNVOTE;
            }
        }
        RequestingUserVote = requestingUserVote;

        if (ChildCommentEntities == null)
            return;

        ChildCommentEntities.ForEach(cce => cce.CheckRequestingUserVote(userId));
    }

    public CommentEntity PureCopy() => new(Id, AuthorEntityId,
        AuthorEntityUsername, PostEntityId, Content, ParentCommentEntityId, Vote,
        RequestingUserVote, CreationDate, ModifiedDate, childCommentEntities: ChildCommentEntities);
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

        modelBuilder
            .HasOne(d => d.ParentCommentEntity)
            .WithMany(p => p.ChildCommentEntities)
            .HasForeignKey(d => d.ParentCommentEntityId)
            .HasPrincipalKey(p => p.Id)
            .IsRequired(false);
    }
}
