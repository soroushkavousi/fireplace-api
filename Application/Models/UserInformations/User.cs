using FireplaceApi.Application.Enums;
using FireplaceApi.Application.ValueObjects;
using System;
using System.Collections.Generic;

namespace FireplaceApi.Application.Models;

public class User : BaseModel
{
    public string Username { get; set; }
    public UserState State { get; set; }
    public List<UserRole> Roles { get; set; }
    public string DisplayName { get; set; }
    public string About { get; set; }
    public string AvatarUrl { get; set; }
    public string BannerUrl { get; set; }
    public Password Password { get; set; }
    public string ResetPasswordCode { get; set; }
    public Email Email { get; set; }
    public GoogleUser GoogleUser { get; set; }
    public List<Session> Sessions { get; set; }

    public User(ulong id, string username, UserState state, List<UserRole> roles,
        string displayName, string about, string avatarUrl, string bannerUrl,
        DateTime creationDate, DateTime? modifiedDate = null, Password password = null,
        string resetPasswordCode = null, Email email = null, GoogleUser googleUser = null,
        List<Session> sessions = null) : base(id, creationDate, modifiedDate)
    {
        Username = username ?? throw new ArgumentNullException(nameof(username));
        Roles = roles;
        State = state;
        DisplayName = displayName;
        About = about;
        AvatarUrl = avatarUrl;
        BannerUrl = bannerUrl;
        Password = password;
        ResetPasswordCode = resetPasswordCode;
        Email = email;
        GoogleUser = googleUser;
        Sessions = sessions;
    }

    public User PureCopy() => new(Id, Username, State, Roles,
        DisplayName, About, AvatarUrl, BannerUrl, CreationDate,
        ModifiedDate, Password, ResetPasswordCode);
}