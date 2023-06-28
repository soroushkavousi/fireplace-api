using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Infrastructure.Entities;

[Index(nameof(MacAddress), IsUnique = true)]
public class ServerEntity : BaseEntity
{
    public static ServerEntity Current { get; set; }

    [Required]
    public string MacAddress { get; set; }

    private ServerEntity() : base() { }

    public ServerEntity(ulong id, string macAddress,
        DateTime? creationDate = null, DateTime? modifiedDate = null)
        : base(id, creationDate, modifiedDate)
    {
        MacAddress = macAddress ?? throw new ArgumentNullException(nameof(macAddress));
    }

    public ServerEntity PureCopy() => new(Id, MacAddress, CreationDate, ModifiedDate);
}

public class ServerEntityConfiguration : IEntityTypeConfiguration<ServerEntity>
{
    public void Configure(EntityTypeBuilder<ServerEntity> modelBuilder)
    {
        // p => principal / d => dependent / e => entity

        modelBuilder.DoBaseConfiguration();
    }
}
