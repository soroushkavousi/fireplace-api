using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Infrastructure.Entities;

[Index(nameof(VoterEntityId), IsUnique = false)]
[Index(nameof(VoterEntityUsername), IsUnique = false)]
[Index(nameof(CommentEntityId), IsUnique = false)]
public class CommentVoteEntity : BaseEntity
{
    public ulong VoterEntityId { get; set; }
    [Required]
    public string VoterEntityUsername { get; set; }
    public ulong CommentEntityId { get; set; }
    public bool IsUp { get; set; }
    public UserEntity VoterEntity { get; set; }
    public CommentEntity CommentEntity { get; set; }

    private CommentVoteEntity() : base() { }

    public CommentVoteEntity(ulong id, ulong voterEntityId, string voterEntityUsername,
        ulong commentEntityId, bool isUp, DateTime? creationDate = null,
        DateTime? modifiedDate = null, UserEntity voterEntity = null,
        CommentEntity commentEntity = null) : base(id, creationDate, modifiedDate)
    {
        VoterEntityId = voterEntityId;
        VoterEntityUsername = voterEntityUsername;
        CommentEntityId = commentEntityId;
        IsUp = isUp;
        VoterEntity = voterEntity;
        CommentEntity = commentEntity;
    }

    public CommentVoteEntity PureCopy() => new(Id, VoterEntityId,
        VoterEntityUsername, CommentEntityId, IsUp, CreationDate, ModifiedDate);
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
