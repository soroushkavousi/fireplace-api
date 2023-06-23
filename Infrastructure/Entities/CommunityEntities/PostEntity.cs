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
[Index(nameof(CommunityEntityId), IsUnique = false)]
[Index(nameof(CommunityEntityName), IsUnique = false)]
public class PostEntity : BaseEntity
{
    public ulong AuthorEntityId { get; set; }
    [Required]
    public Username AuthorEntityUsername { get; set; }
    public ulong CommunityEntityId { get; set; }
    [Required]
    public string CommunityEntityName { get; set; }
    public int Vote { get; set; }
    [Required]
    public string Content { get; set; }
    public UserEntity AuthorEntity { get; set; }
    public CommunityEntity CommunityEntity { get; set; }
    public List<PostVoteEntity> PostVoteEntities { get; set; }
    public List<CommentEntity> CommentEntities { get; set; }
    [NotMapped]
    public VoteType RequestingUserVote { get; set; }

    private PostEntity() : base() { }

    public PostEntity(ulong id, ulong authorEntityId, Username authorEntityUsername,
        ulong communityEntityId, string communityEntityName, string content,
        int vote = 0, VoteType requestingUserVote = VoteType.NEUTRAL,
        DateTime? creationDate = null, DateTime? modifiedDate = null,
        UserEntity author = null, CommunityEntity communityEntity = null,
        List<PostVoteEntity> postVoteEntities = null,
        List<CommentEntity> commentEntities = null) : base(id, creationDate, modifiedDate)
    {
        AuthorEntityId = authorEntityId;
        AuthorEntityUsername = authorEntityUsername;
        CommunityEntityId = communityEntityId;
        CommunityEntityName = communityEntityName;
        Content = content ?? throw new ArgumentNullException(nameof(content));
        Vote = vote;
        RequestingUserVote = requestingUserVote;
        AuthorEntity = author;
        CommunityEntity = communityEntity;
        PostVoteEntities = postVoteEntities;
        CommentEntities = commentEntities;
    }

    public void CheckRequestingUserVote(ulong? userId)
    {
        if (userId == null)
            return;

        var requestingUserVote = VoteType.NEUTRAL;
        if (PostVoteEntities != null)
        {
            var requestingUserVoteEntity = PostVoteEntities
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
    }

    public PostEntity PureCopy() => new(Id, AuthorEntityId,
        AuthorEntityUsername, CommunityEntityId, CommunityEntityName,
        Content, Vote, RequestingUserVote, CreationDate, ModifiedDate);
}

public class PostEntityConfiguration : IEntityTypeConfiguration<PostEntity>
{
    public void Configure(EntityTypeBuilder<PostEntity> modelBuilder)
    {
        // p => principal / d => dependent

        modelBuilder.DoBaseConfiguration();

        modelBuilder
            .HasOne(d => d.AuthorEntity)
            .WithMany(p => p.PostEntities)
            .HasForeignKey(d => new { d.AuthorEntityId, d.AuthorEntityUsername })
            .HasPrincipalKey(p => new { p.Id, p.Username })
            .IsRequired();

        modelBuilder
            .HasOne(d => d.CommunityEntity)
            .WithMany(p => p.PostEntities)
            .HasForeignKey(d => new { d.CommunityEntityId, d.CommunityEntityName })
            .HasPrincipalKey(p => new { p.Id, p.Name })
            .IsRequired();
    }
}
