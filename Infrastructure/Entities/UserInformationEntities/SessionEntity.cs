using GamingCommunityApi.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GamingCommunityApi.Infrastructure.Entities.UserInformationEntities
{
    public class SessionEntity
    {
        public long UserEntityId { get; set; }
        public string IpAddress { get; set; }
        public string State { get; set; }
        public long? Id { get; set; }
        public UserEntity UserEntity { get; set; }

        private SessionEntity() { }

        public SessionEntity(long userEntityId, string ipAddress, string state,
            long? id = null, UserEntity userEntity = null)
        {
            UserEntityId = userEntityId;
            IpAddress = ipAddress ?? throw new ArgumentNullException(nameof(ipAddress));
            State = state ?? throw new ArgumentNullException(nameof(state));
            Id = id;
            UserEntity = userEntity;
        }

        public SessionEntity PureCopy() => new SessionEntity(UserEntityId, IpAddress,
            State, Id, null);

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
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
