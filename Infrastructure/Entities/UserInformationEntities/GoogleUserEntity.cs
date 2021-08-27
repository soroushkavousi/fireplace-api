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
    public class GoogleUserEntity : BaseEntity
    {
        public long UserEntityId { get; set; }
        public string Code { get; set; }
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public long AccessTokenExpiresInSeconds { get; set; }
        public string RefreshToken { get; set; }
        public string Scope { get; set; }
        public string IdToken { get; set; }
        public DateTime AccessTokenIssuedTime { get; set; }
        public string GmailAddress { get; set; }
        public bool GmailVerified { get; set; }
        public long GmailIssuedTimeInSeconds { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Locale { get; set; }
        public string PictureUrl { get; set; }
        public string State { get; set; }
        public string AuthUser { get; set; }
        public string Prompt { get; set; }
        public string RedirectToUserUrl { get; set; }
        public long? Id { get; set; }
        public UserEntity UserEntity { get; set; }

        private GoogleUserEntity() : base() { }

        public GoogleUserEntity(long userEntityId, string code, 
            string accessToken, string tokenType, 
            long accessTokenExpiresInSeconds, string refreshToken, 
            string scope, string idToken, DateTime accessTokenIssuedTime, 
            string gmailAddress, bool gmailVerified, 
            long gmailIssuedTimeInSeconds, string fullName, 
            string firstName, string lastName, string locale, 
            string pictureUrl, string state, string authUser, 
            string prompt, string redirectToUserUrl, long? id = null, 
            UserEntity userEntity = null) : base()
        {
            UserEntityId = userEntityId;
            Code = code ?? throw new ArgumentNullException(nameof(code));
            AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
            TokenType = tokenType ?? throw new ArgumentNullException(nameof(tokenType));
            AccessTokenExpiresInSeconds = accessTokenExpiresInSeconds;
            RefreshToken = refreshToken ?? throw new ArgumentNullException(nameof(refreshToken));
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
            IdToken = idToken ?? throw new ArgumentNullException(nameof(idToken));
            AccessTokenIssuedTime = accessTokenIssuedTime;
            GmailAddress = gmailAddress ?? throw new ArgumentNullException(nameof(gmailAddress));
            GmailVerified = gmailVerified;
            GmailIssuedTimeInSeconds = gmailIssuedTimeInSeconds;
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            Locale = locale ?? throw new ArgumentNullException(nameof(locale));
            PictureUrl = pictureUrl ?? throw new ArgumentNullException(nameof(pictureUrl));
            State = state ?? throw new ArgumentNullException(nameof(state));
            AuthUser = authUser ?? throw new ArgumentNullException(nameof(authUser));
            Prompt = prompt ?? throw new ArgumentNullException(nameof(prompt));
            RedirectToUserUrl = redirectToUserUrl ?? throw new ArgumentNullException(nameof(redirectToUserUrl));
            Id = id;
            UserEntity = userEntity;
        }

        public GoogleUserEntity PureCopy() => new GoogleUserEntity(UserEntityId, Code, AccessToken,
            TokenType, AccessTokenExpiresInSeconds, RefreshToken, Scope, IdToken,
            AccessTokenIssuedTime, GmailAddress, GmailVerified, GmailIssuedTimeInSeconds,
            FullName, FirstName, LastName, Locale, PictureUrl, State, AuthUser, Prompt,
            RedirectToUserUrl, Id, null);

        public void RemoveLoopReferencing()
        {
            UserEntity = UserEntity?.PureCopy();
        }
    }

    public class GoogleUserEntityConfiguration : IEntityTypeConfiguration<GoogleUserEntity>
    {
        public void Configure(EntityTypeBuilder<GoogleUserEntity> modelBuilder)
        {
            // p => principal / d => dependent

            modelBuilder
                .HasIndex(e => e.GmailAddress)
                .IsUnique();

            modelBuilder
                .HasOne(d => d.UserEntity)
                .WithOne(p => p.GoogleUserEntity)
                .HasForeignKey<GoogleUserEntity>(d => d.UserEntityId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
