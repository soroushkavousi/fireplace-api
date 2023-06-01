using FireplaceApi.Domain.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Infrastructure.Entities;

[Index(nameof(Value), IsUnique = true)]
public class AccessTokenEntity : BaseEntity
{
    public ulong UserEntityId { get; set; }
    [Required]
    [Sensitive]
    public string Value { get; set; }
    public UserEntity UserEntity { get; set; }

    private AccessTokenEntity() : base() { }

    public AccessTokenEntity(ulong id, ulong userEntityId, string value,
        DateTime? creationDate = null, DateTime? modifiedDate = null,
        UserEntity userEntity = null) : base(id, creationDate, modifiedDate)
    {
        UserEntityId = userEntityId;
        Value = value ?? throw new ArgumentNullException(nameof(value));
        UserEntity = userEntity;
    }

    public AccessTokenEntity PureCopy() => new(Id, UserEntityId,
        Value, CreationDate, ModifiedDate);

    public void RemoveLoopReferencing()
    {
        UserEntity = UserEntity?.PureCopy();
    }
}

public class AccessTokenEntityConfiguration : IEntityTypeConfiguration<AccessTokenEntity>
{
    public void Configure(EntityTypeBuilder<AccessTokenEntity> modelBuilder)
    {
        // p => principal / d => dependent

        modelBuilder.DoBaseConfiguration();

        modelBuilder
            .HasOne(d => d.UserEntity)
            .WithMany(p => p.AccessTokenEntities)
            .HasForeignKey(d => d.UserEntityId)
            .HasPrincipalKey(p => p.Id)
            .IsRequired();
    }
}
