using FireplaceApi.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FireplaceApi.Infrastructure.Entities
{
    [Index(nameof(Username), IsUnique = true)]
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
        public List<CommunityEntity> OwnCommunities { get; set; }
        public List<CommunityMemberEntity> JoinedCommunities { get; set; }
        public List<PostEntity> PostEntities { get; set; }
        public List<PostVoteEntity> PostVoteEntities { get; set; }
        public List<CommentEntity> CommentEntities { get; set; }
        public List<CommentVoteEntity> CommentVoteEntities { get; set; }

        private UserEntity() : base() { }

        public UserEntity(string firstName, string lastName,
            string username, string state, DateTime? creationDate = null, 
            DateTime? modifiedDate = null, string passwordHash = null, long? id = null,
            EmailEntity emailEntity = null, GoogleUserEntity googleUserEntity = null,
            List<AccessTokenEntity> accessTokenEntities = null, 
            List<SessionEntity> sessionEntities = null,
            List<CommunityEntity> ownCommunities = null,
            List<CommunityMemberEntity> joinedCommunities = null,
            List<PostEntity> postEntities = null,
            List<PostVoteEntity> postVoteEntities = null,
            List<CommentEntity> commentEntities = null,
            List<CommentVoteEntity> commentVoteEntities = null) : base(creationDate, modifiedDate)
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
            OwnCommunities = ownCommunities;
            JoinedCommunities = joinedCommunities;
            PostEntities = postEntities;
            PostVoteEntities = postVoteEntities;
            CommentEntities = commentEntities;
            CommentVoteEntities = commentVoteEntities;
        }

        public UserEntity PureCopy() => new UserEntity(FirstName, LastName, 
            Username, State, CreationDate, ModifiedDate, PasswordHash, Id);

        //public void RemoveLoopReferencing()
        //{
        //    EmailEntity = EmailEntity?.PureCopy();
        //    GoogleUserEntity = GoogleUserEntity?.PureCopy();
        //    AccessTokenEntities?.ForEach(
        //            accessTokenEntity => accessTokenEntity?.PureCopy());
        //    SessionEntities?.ForEach(
        //            sessionEntity => sessionEntity?.PureCopy());
        //}
    }

    public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> modelBuilder)
        {
            // p => principal / d => dependent
        }
    }
}