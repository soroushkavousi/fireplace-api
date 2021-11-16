using FireplaceApi.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Infrastructure.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    [Index(nameof(Code), IsUnique = true)]
    public class ErrorEntity : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public int Code { get; set; }
        [Required]
        public string ClientMessage { get; set; }
        public int HttpStatusCode { get; set; }

        private ErrorEntity() : base() { }

        public ErrorEntity(ulong id, string name, int code,
            string clientMessage, int httpStatusCode,
            DateTime? creationDate = null, DateTime? modifiedDate = null)
            : base(id, creationDate, modifiedDate)
        {
            Name = name ?? throw new ArgumentNullException(nameof(clientMessage));
            Code = code;
            ClientMessage = clientMessage ?? throw new ArgumentNullException(nameof(clientMessage));
            HttpStatusCode = httpStatusCode;
        }

        public ErrorEntity PureCopy() => new ErrorEntity(Id, Name, Code, ClientMessage,
            HttpStatusCode, CreationDate, ModifiedDate);

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
               .Property(e => e.Name)
               .HasDefaultValue(ErrorName.INTERNAL_SERVER.ToString())
               .IsRequired();

            modelBuilder
                .Property(e => e.HttpStatusCode)
                .HasDefaultValue(400);
        }
    }
}
