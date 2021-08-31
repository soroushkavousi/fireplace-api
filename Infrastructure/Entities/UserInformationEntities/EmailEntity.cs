using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FireplaceApi.Infrastructure.Entities.UserInformationEntities
{
    public class EmailEntity : BaseEntity
    {
        public long UserEntityId { get; set; }
        public string Address { get; set; }
        public string ActivationStatus { get; set; }
        public int? ActivationCode { get; set; }
        public long? Id { get; set; }
        public UserEntity UserEntity { get; set; }

        private EmailEntity() : base() { }

        public EmailEntity(long userEntityId, string address, string activationStatus,
            DateTime? creationDate = null, DateTime? modifiedDate = null, int? activationCode = null, 
            long? id = null, UserEntity userEntity = null) : base(creationDate, modifiedDate)
        {
            UserEntityId = userEntityId;
            Address = address ?? throw new ArgumentNullException(nameof(address));
            ActivationStatus = activationStatus ?? throw new ArgumentNullException(nameof(activationStatus));
            ActivationCode = activationCode;
            Id = id;
            UserEntity = userEntity;
        }

        public EmailEntity PureCopy() => new EmailEntity(UserEntityId, Address,
            ActivationStatus, CreationDate, ModifiedDate, ActivationCode, Id, null);

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
