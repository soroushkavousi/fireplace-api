using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Infrastructure.Entities;

[Index(nameof(Address), IsUnique = true)]
public class EmailEntity : BaseEntity
{
    public ulong UserEntityId { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public string ActivationStatus { get; set; }
    public int? ActivationCode { get; set; }
    public UserEntity UserEntity { get; set; }

    private EmailEntity() : base() { }

    public EmailEntity(ulong id, ulong userEntityId, string address, string activationStatus,
        DateTime? creationDate = null, DateTime? modifiedDate = null, int? activationCode = null,
        UserEntity userEntity = null) : base(id, creationDate, modifiedDate)
    {
        UserEntityId = userEntityId;
        Address = address ?? throw new ArgumentNullException(nameof(address));
        ActivationStatus = activationStatus ?? throw new ArgumentNullException(nameof(activationStatus));
        ActivationCode = activationCode;
        UserEntity = userEntity;
    }

    public EmailEntity PureCopy() => new(Id, UserEntityId, Address,
        ActivationStatus, CreationDate, ModifiedDate, ActivationCode);

    public void RemoveLoopReferencing()
    {
        UserEntity = UserEntity?.PureCopy();
    }
}

public class EmailEntityConfiguration : IEntityTypeConfiguration<EmailEntity>
{
    public void Configure(EntityTypeBuilder<EmailEntity> modelBuilder)
    {
        // p => principal / d => dependent

        modelBuilder.DoBaseConfiguration();

        modelBuilder
            .HasOne(d => d.UserEntity)
            .WithOne(p => p.EmailEntity)
            .HasForeignKey<EmailEntity>(d => d.UserEntityId)
            .HasPrincipalKey<UserEntity>(p => p.Id)
            .IsRequired();
    }
}
