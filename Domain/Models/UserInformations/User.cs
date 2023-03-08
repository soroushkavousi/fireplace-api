using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace FireplaceApi.Domain.Models
{
    public class User : BaseModel
    {
        public string Username { get; set; }
        public UserState State { get; set; }
        public string DisplayName { get; set; }
        public string About { get; set; }
        public string AvatarUrl { get; set; }
        public string BannerUrl { get; set; }
        public Password Password { get; set; }
        public string ResetPasswordCode { get; set; }
        public Email Email { get; set; }
        public GoogleUser GoogleUser { get; set; }
        public List<AccessToken> AccessTokens { get; set; }
        public List<Session> Sessions { get; set; }

        public User(ulong id, string username, UserState state, DateTime creationDate,
            string displayName, string about, string avatarUrl, string bannerUrl,
            DateTime? modifiedDate = null, Password password = null, string resetPasswordCode = null,
            Email email = null, GoogleUser googleUser = null, List<AccessToken> accessTokens = null,
            List<Session> sessions = null) : base(id, creationDate, modifiedDate)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            State = state;
            DisplayName = displayName;
            About = about;
            AvatarUrl = avatarUrl;
            BannerUrl = bannerUrl;
            Password = password;
            ResetPasswordCode = resetPasswordCode;
            Email = email;
            GoogleUser = googleUser;
            AccessTokens = accessTokens;
            Sessions = sessions;
        }

        public User PureCopy() => new(Id, Username, State, CreationDate,
            DisplayName, About, AvatarUrl, BannerUrl, ModifiedDate, Password, ResetPasswordCode);

        public void RemoveLoopReferencing()
        {
            Email = Email?.PureCopy();
            GoogleUser = GoogleUser?.PureCopy();
            AccessTokens?.ForEach(
                accessToken => accessToken?.PureCopy());
            Sessions?.ForEach(
                session => session?.PureCopy());
        }
    }
}