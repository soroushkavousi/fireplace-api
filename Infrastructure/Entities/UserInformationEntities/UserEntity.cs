using FireplaceApi.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Infrastructure.Entities;

[Index(nameof(Username), IsUnique = true)]
public class UserEntity : BaseEntity
{
    [Required]
    public Username Username { get; set; }
    [Required]
    public string State { get; set; }
    public List<string> Roles { get; set; }
    public string DisplayName { get; set; }
    public string About { get; set; }
    public string AvatarUrl { get; set; }
    public string BannerUrl { get; set; }
    public string PasswordHash { get; set; }
    public string ResetPasswordCode { get; set; }
    public EmailEntity EmailEntity { get; set; }
    public GoogleUserEntity GoogleUserEntity { get; set; }
    public List<SessionEntity> SessionEntities { get; set; }
    public List<CommunityEntity> OwnCommunities { get; set; }
    public List<CommunityMembershipEntity> JoinedCommunities { get; set; }
    public List<PostEntity> PostEntities { get; set; }
    public List<PostVoteEntity> PostVoteEntities { get; set; }
    public List<CommentEntity> CommentEntities { get; set; }
    public List<CommentVoteEntity> CommentVoteEntities { get; set; }

    private UserEntity() : base() { }

    public UserEntity(ulong id, Username username, string state, List<string> roles,
        DateTime? creationDate = null, string displayName = null, string about = null,
        string avatarUrl = null, string bannerUrl = null, DateTime? modifiedDate = null,
        string passwordHash = null, string resetPasswordCode = null, EmailEntity emailEntity = null,
        GoogleUserEntity googleUserEntity = null, List<SessionEntity> sessionEntities = null,
        List<CommunityEntity> ownCommunities = null, List<CommunityMembershipEntity> joinedCommunities = null,
        List<PostEntity> postEntities = null, List<PostVoteEntity> postVoteEntities = null,
        List<CommentEntity> commentEntities = null,
        List<CommentVoteEntity> commentVoteEntities = null) : base(id, creationDate, modifiedDate)
    {
        Username = username ?? throw new ArgumentNullException(nameof(username));
        State = state ?? throw new ArgumentNullException(nameof(state));
        Roles = roles ?? throw new ArgumentNullException(nameof(roles));
        DisplayName = displayName;
        About = about;
        AvatarUrl = avatarUrl;
        BannerUrl = bannerUrl;
        PasswordHash = passwordHash;
        ResetPasswordCode = resetPasswordCode;
        EmailEntity = emailEntity;
        GoogleUserEntity = googleUserEntity;
        SessionEntities = sessionEntities;
        OwnCommunities = ownCommunities;
        JoinedCommunities = joinedCommunities;
        PostEntities = postEntities;
        PostVoteEntities = postVoteEntities;
        CommentEntities = commentEntities;
        CommentVoteEntities = commentVoteEntities;
    }

    public UserEntity PureCopy() => new(Id, Username, State, Roles, CreationDate,
        DisplayName, About, AvatarUrl, BannerUrl, ModifiedDate, PasswordHash, ResetPasswordCode);

    //public void RemoveLoopReferencing()
    //{
    //    EmailEntity = EmailEntity?.PureCopy();
    //    GoogleUserEntity = GoogleUserEntity?.PureCopy();
    //    AccessTokenEntities?.ForEach(
    //            accessTokenEntity => accessTokenEntity?.PureCopy());
    //    SessionEntities?.ForEach(
    //            sessionEntity => sessionEntity?.PureCopy());
    //}

    public static readonly ValueConverter<Username, string> UsernameConverter
        = new(valueObject => valueObject.Value, value => new(value));
}

public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> modelBuilder)
    {
        // p => principal / d => dependent

        modelBuilder.DoBaseConfiguration();

        modelBuilder
            .Property(e => e.Username)
            .HasConversion(UserEntity.UsernameConverter);
    }
}