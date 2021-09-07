using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FireplaceApi.Core.Enums;

namespace FireplaceApi.Infrastructure.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    [Index(nameof(Code), IsUnique = true)]
    public class ErrorEntity : BaseEntity
    {
        [Column(Order = 1)]
        public string Name { get; set; }
        [Column(Order = 2)]
        public int Code { get; set; }
        [Column(Order = 3)]
        public string ClientMessage { get; set; }
        [Column(Order = 4)]
        public int HttpStatusCode { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 0)]
        public int? Id { get; set; }

        private ErrorEntity() : base() { }

        public ErrorEntity(string name, int code, 
            string clientMessage, int httpStatusCode,
            DateTime? creationDate = null, DateTime? modifiedDate = null, 
            int? id = null) : base(creationDate, modifiedDate)
        {
            Name = name ?? throw new ArgumentNullException(nameof(clientMessage));
            Code = code;
            ClientMessage = clientMessage ?? throw new ArgumentNullException(nameof(clientMessage));
            HttpStatusCode = httpStatusCode;
            Id = id;
        }

        public ErrorEntity PureCopy() => new ErrorEntity(Name, Code, ClientMessage,
            HttpStatusCode, CreationDate, ModifiedDate, Id);

        public void RemoveLoopReferencing()
        {

        }
    }

    public class ErrorEntityConfiguration : IEntityTypeConfiguration<ErrorEntity>
    {
        public void Configure(EntityTypeBuilder<ErrorEntity> modelBuilder)
        {
            // p => principal / d => dependent / e => entity

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
