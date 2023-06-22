using FireplaceApi.Domain.Common;
using FireplaceApi.Domain.Users;

namespace FireplaceApi.Domain.GoogleUsers;

public class GoogleUser : BaseModel
{
    public ulong UserId { get; set; }
    [Sensitive]
    public string Code { get; set; }
    [Sensitive]
    public string AccessToken { get; set; }
    public string TokenType { get; set; }
    public long AccessTokenExpiresInSeconds { get; set; }
    [Sensitive]
    public string RefreshToken { get; set; }
    public string Scope { get; set; }
    [Sensitive]
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
    public User User { get; set; }

    public GoogleUser(ulong id, ulong userId, string code,
        string accessToken, string tokenType,
        long accessTokenExpiresInSeconds, string refreshToken,
        string scope, string idToken, DateTime accessTokenIssuedTime,
        string gmailAddress, bool gmailVerified, long gmailIssuedTimeInSeconds,
        string fullName, string firstName, string lastName, string locale,
        string pictureUrl, string state, string authUser, string prompt,
        string redirectToUserUrl, DateTime creationDate, DateTime? modifiedDate = null,
        User user = null) : base(id, creationDate, modifiedDate)
    {
        UserId = userId;
        Code = code ?? throw new ArgumentNullException(nameof(code));
        AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
        TokenType = tokenType ?? throw new ArgumentNullException(nameof(tokenType));
        AccessTokenExpiresInSeconds = accessTokenExpiresInSeconds;
        RefreshToken = refreshToken;
        Scope = scope ?? throw new ArgumentNullException(nameof(scope));
        IdToken = idToken ?? throw new ArgumentNullException(nameof(idToken));
        AccessTokenIssuedTime = accessTokenIssuedTime;
        GmailAddress = gmailAddress ?? throw new ArgumentNullException(nameof(gmailAddress));
        GmailVerified = gmailVerified;
        GmailIssuedTimeInSeconds = gmailIssuedTimeInSeconds;
        FullName = fullName;
        FirstName = firstName;
        LastName = lastName;
        Locale = locale;
        PictureUrl = pictureUrl;
        State = state;
        AuthUser = authUser;
        Prompt = prompt;
        RedirectToUserUrl = redirectToUserUrl;
        User = user;
    }

    public GoogleUser PureCopy() => new(Id, UserId, Code, AccessToken,
        TokenType, AccessTokenExpiresInSeconds, RefreshToken, Scope, IdToken,
        AccessTokenIssuedTime, GmailAddress, GmailVerified, GmailIssuedTimeInSeconds,
        FullName, FirstName, LastName, Locale, PictureUrl, State, AuthUser, Prompt,
        RedirectToUserUrl, CreationDate, ModifiedDate);

    public void RemoveLoopReferencing()
    {
        User = User?.PureCopy();
    }
}
