using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GamingCommunityApi.Infrastructure.Entities.UserInformationEntities
{
    public class EmailEntity
    {
        public long UserEntityId { get; set; }
        public string Address { get; set; }
        public long ActivationCode { get; set; }
        public string ActivationStatus { get; set; }
        public long? Id { get; set; }
        public UserEntity UserEntity { get; set; }

        private EmailEntity() { }

        public EmailEntity(long userEntityId, string address, long activationCode,
            string activationStatus, long? id = null, UserEntity userEntity = null)
        {
            UserEntityId = userEntityId;
            Address = address ?? throw new ArgumentNullException(nameof(address));
            ActivationCode = activationCode;
            ActivationStatus = activationStatus ?? throw new ArgumentNullException(nameof(activationStatus));
            Id = id;
            UserEntity = userEntity;
        }

        public EmailEntity PureCopy() => new EmailEntity(UserEntityId, Address,
            ActivationCode, ActivationStatus, Id, null);

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

            modelBuilder
                .HasIndex(e => e.Address)
                .IsUnique();

            modelBuilder
                .HasOne(d => d.UserEntity)
                .WithOne(p => p.EmailEntity)
                .HasForeignKey<EmailEntity>(d => d.UserEntityId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
