using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FireplaceApi.Infrastructure.Entities
{
    public class SessionEntity : BaseEntity
    {
        public long UserEntityId { get; set; }
        public string IpAddress { get; set; }
        public string State { get; set; }
        public long? Id { get; set; }
        public UserEntity UserEntity { get; set; }

        private SessionEntity() : base() { }

        public SessionEntity(long userEntityId, string ipAddress, string state,
            DateTime? creationDate = null, DateTime? modifiedDate = null, long? id = null,
            UserEntity userEntity = null) : base(creationDate, modifiedDate)
        {
            UserEntityId = userEntityId;
            IpAddress = ipAddress ?? throw new ArgumentNullException(nameof(ipAddress));
            State = state ?? throw new ArgumentNullException(nameof(state));
            Id = id;
            UserEntity = userEntity;
        }

        public SessionEntity PureCopy() => new SessionEntity(UserEntityId, IpAddress,
            State, CreationDate, ModifiedDate, Id);

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

            modelBuilder
                .HasOne(d => d.UserEntity)
                .WithMany(p => p.SessionEntities)
                .HasForeignKey(d => d.UserEntityId)
                .HasPrincipalKey(p => p.Id)
                .IsRequired();
        }
    }
}
