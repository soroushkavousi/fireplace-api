using FireplaceApi.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FireplaceApi.Infrastructure.Entities.UserInformationEntities
{
    public class UserEntity : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string State { get; set; }
        public string PasswordHash { get; set; }
        public long? Id { get; set; }
        public EmailEntity EmailEntity { get; set; }
        public GoogleUserEntity GoogleUserEntity { get; set; }
        public List<AccessTokenEntity> AccessTokenEntities { get; set; }
        public List<SessionEntity> SessionEntities { get; set; }

        private UserEntity() : base() { }

        public UserEntity(string firstName, string lastName,
            string username, string state, string passwordHash = null, long? id = null,
            EmailEntity emailEntity = null, GoogleUserEntity googleUserEntity = null,
            List<AccessTokenEntity> accessTokenEntities = null, List<SessionEntity> sessionEntities = null) : base()
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            Username = username ?? throw new ArgumentNullException(nameof(username));
            State = state ?? throw new ArgumentNullException(nameof(state));
            PasswordHash = passwordHash;
            Id = id;
            EmailEntity = emailEntity;
            GoogleUserEntity = googleUserEntity;
            AccessTokenEntities = accessTokenEntities;
            SessionEntities = sessionEntities;
        }

        public UserEntity PureCopy() => new UserEntity(FirstName, LastName, 
            Username, State, PasswordHash, Id, null, null, null, null);

        public void RemoveLoopReferencing()
        {
            EmailEntity = EmailEntity?.PureCopy();
            GoogleUserEntity = GoogleUserEntity?.PureCopy();
            AccessTokenEntities?.ForEach(
                    accessTokenEntity => accessTokenEntity?.PureCopy());
            SessionEntities?.ForEach(
                    sessionEntity => sessionEntity?.PureCopy());
        }
    }

    public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> modelBuilder)
        {
            // p => principal / d => dependent

            modelBuilder
                .HasIndex(e => e.Username)
                .IsUnique();
        }
    }
}