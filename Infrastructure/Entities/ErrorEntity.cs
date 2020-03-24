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
using GamingCommunityApi.Core.Enums;

namespace GamingCommunityApi.Infrastructure.Entities
{
    public class ErrorEntity
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

        private ErrorEntity() { }

        public ErrorEntity(string name, int code, 
            string clientMessage, int httpStatusCode, int? id = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(clientMessage));
            Code = code;
            ClientMessage = clientMessage ?? throw new ArgumentNullException(nameof(clientMessage));
            HttpStatusCode = httpStatusCode;
            Id = id;
        }

        public ErrorEntity PureCopy() => new ErrorEntity(Name, Code, ClientMessage,
            HttpStatusCode, Id);

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
                .HasIndex(e => e.Name)
                .IsUnique();

            modelBuilder
                .HasIndex(e => e.Code)
                .IsUnique();

            modelBuilder
                .Property(e => e.HttpStatusCode)
                .HasDefaultValue(400);
        }
    }
}
