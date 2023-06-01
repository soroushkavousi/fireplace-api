using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Infrastructure.Entities;

[Index(nameof(Code), IsUnique = true)]
public class ErrorEntity : BaseEntity
{
    [Required]
    public int Code { get; set; }
    [Required]
    public string Type { get; set; }
    [Required]
    public string Field { get; set; }
    [Required]
    public string ClientMessage { get; set; }
    public int HttpStatusCode { get; set; }

    private ErrorEntity() : base() { }

    public ErrorEntity(ulong id, int code, string type,
        string field, string clientMessage, int httpStatusCode,
        DateTime? creationDate = null, DateTime? modifiedDate = null)
        : base(id, creationDate, modifiedDate)
    {
        Code = code;
        Type = type ?? throw new ArgumentNullException(nameof(type));
        Field = field ?? throw new ArgumentNullException(nameof(field));
        ClientMessage = clientMessage ?? throw new ArgumentNullException(nameof(clientMessage));
        HttpStatusCode = httpStatusCode;
    }

    public ErrorEntity PureCopy() => new(Id, Code, Type, Field,
        ClientMessage, HttpStatusCode, CreationDate, ModifiedDate);

    public void RemoveLoopReferencing()
    {

    }
}

public class ErrorEntityConfiguration : IEntityTypeConfiguration<ErrorEntity>
{
    public void Configure(EntityTypeBuilder<ErrorEntity> modelBuilder)
    {
        // p => principal / d => dependent / e => entity

        modelBuilder.DoBaseConfiguration();

        modelBuilder
            .Property(e => e.HttpStatusCode)
            .HasDefaultValue(400);
    }
}
