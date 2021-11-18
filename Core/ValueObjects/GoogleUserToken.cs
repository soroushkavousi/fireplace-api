using System;

namespace FireplaceApi.Core.ValueObjects
{
    public class GoogleUserToken
    {
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

        public GoogleUserToken(string code, string accessToken,
            string tokenType, long accessTokenExpiresInSeconds,
            string refreshToken, string scope, string idToken,
            DateTime accessTokenIssuedTime, string gmailAddress,
            bool gmailVerified, long gmailIssuedTimeInSeconds,
            string fullName, string firstName, string lastName,
            string locale, string pictureUrl)
        {
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
            FullName = fullName;
            FirstName = firstName;
            LastName = lastName;
            Locale = locale;
            PictureUrl = pictureUrl;
        }
    }
}