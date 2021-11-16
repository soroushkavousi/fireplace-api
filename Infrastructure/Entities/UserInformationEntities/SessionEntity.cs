using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Infrastructure.Entities
{
    public class SessionEntity : BaseEntity
    {
        public ulong UserEntityId { get; set; }
        [Required]
        public string IpAddress { get; set; }
        [Required]
        public string State { get; set; }
        public UserEntity UserEntity { get; set; }

        private SessionEntity() : base() { }

        public SessionEntity(ulong id, ulong userEntityId, string ipAddress, string state,
            DateTime? creationDate = null, DateTime? modifiedDate = null,
            UserEntity userEntity = null) : base(id, creationDate, modifiedDate)
        {
            UserEntityId = userEntityId;
            IpAddress = ipAddress ?? throw new ArgumentNullException(nameof(ipAddress));
            State = state ?? throw new ArgumentNullException(nameof(state));
            UserEntity = userEntity;
        }

        public SessionEntity PureCopy() => new SessionEntity(Id, UserEntityId, IpAddress,
            State, CreationDate, ModifiedDate);

        public void RemoveLoopReferencing()
        {
            UserEntity = UserEntity?.PureCopy();
        }
    }

    public class SessionEntityConfiguration : IEntityTypeConfiguration<SessionEntity>
    {
        public void Configure(EntityTypeBuilder<SessionEntity> modelBuilder)
        {
            // p => principal / d => dependent

            modelBuilder.DoBaseConfiguration();

            modelBuilder
                .HasOne(d => d.UserEntity)
                .WithMany(p => p.SessionEntities)
                .HasForeignKey(d => d.UserEntityId)
                .HasPrincipalKey(p => p.Id)
                .IsRequired();
        }
    }
}
