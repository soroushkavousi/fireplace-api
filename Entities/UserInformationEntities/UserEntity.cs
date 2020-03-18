using GamingCommunityApi.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GamingCommunityApi.Entities.UserInformationEntities
{
    public class UserEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string State { get; set; }
        public long? Id { get; set; }
        public EmailEntity EmailEntity { get; set; }
        public List<AccessTokenEntity> AccessTokenEntities { get; set; }
        public List<SessionEntity> SessionEntities { get; set; }

        private UserEntity() { }

        public UserEntity(string firstName, string lastName,
            string username, string passwordHash, string state, long? id = null,
            EmailEntity emailEntity = null,
            List<AccessTokenEntity> accessTokenEntities = null, List<SessionEntity> sessionEntities = null)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            Username = username ?? throw new ArgumentNullException(nameof(username));
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
            State = state ?? throw new ArgumentNullException(nameof(state));
            Id = id;
            EmailEntity = emailEntity;
            AccessTokenEntities = accessTokenEntities;
            SessionEntities = sessionEntities;
        }

        public UserEntity PureCopy() => new UserEntity(FirstName, LastName, Username,
            PasswordHash, State, Id, null,
            null, null);

        public UserEntity Copy(bool deep = false)
        {
            var copy = new UserEntity(FirstName, LastName, Username,
                PasswordHash, State, Id, EmailEntity, 
                AccessTokenEntities, SessionEntities);
            return copy;
        }

        public void RemoveLoopReferencing()
        {
            var pureUserEntity = new UserEntity(FirstName, LastName, Username,
                PasswordHash, State, Id, null,
                null, null);

            if (EmailEntity != null && EmailEntity.UserEntity != null)
                EmailEntity.UserEntity = pureUserEntity;

            if(AccessTokenEntities.IsNullOrEmpty() == false)
            {
                AccessTokenEntities.ForEach(
                    accessTokenEntity => accessTokenEntity.UserEntity = pureUserEntity);
            }

            if (SessionEntities.IsNullOrEmpty() == false)
            {
                SessionEntities.ForEach(
                    sessionEntity => sessionEntity.UserEntity = pureUserEntity);
            }
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