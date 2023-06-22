using FireplaceApi.Domain.Common;
using Newtonsoft.Json;

namespace FireplaceApi.Infrastructure.ValueObjects;

public class GoogleTokenResponse
{
    [JsonProperty("access_token")]
    [Sensitive]
    public string AccessToken { get; set; }

    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonProperty("refresh_token")]
    [Sensitive]
    public string RefreshToken { get; set; }

    [JsonProperty("scope")]
    public string Scope { get; set; }

    [JsonProperty("token_type")]
    public string TokenType { get; set; }


    public GoogleTokenResponse() { }

    public GoogleTokenResponse(string accessToken, int expiresIn,
        string refreshToken, string scope, string tokenType)
    {
        AccessToken = accessToken;
        ExpiresIn = expiresIn;
        RefreshToken = refreshToken;
        Scope = scope;
        TokenType = tokenType;
    }
}
