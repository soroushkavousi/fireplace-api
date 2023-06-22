using FireplaceApi.Presentation.Auth;
using FireplaceApi.Presentation.Tools;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace FireplaceApi.IntegrationTests.Extensions;

public static class HttpClientExtensions
{
    public static void AddOrUpdateCsrfToken(this HttpClient httpClient, HttpResponseMessage response)
    {
        response.Headers.TryGetValues(Constants.ResponseSetCookieHeaderKey, out IEnumerable<string> cookies);
        if (cookies == null || !cookies.Any())
            return;
        var csrfTokenCookie = cookies.FirstOrDefault(c => c.StartsWith(AntiforgeryConstants.CsrfTokenKey));
        if (string.IsNullOrWhiteSpace(csrfTokenCookie))
            return;
        var regex = @$"{AntiforgeryConstants.CsrfTokenKey}=([^;]+);";
        var csrfToken = Regex.Match(csrfTokenCookie, regex, RegexOptions.IgnoreCase).Groups[1].Value;

        if (httpClient.DefaultRequestHeaders.Contains(AntiforgeryConstants.CsrfTokenKey))
            httpClient.DefaultRequestHeaders.Remove(AntiforgeryConstants.CsrfTokenKey);
        httpClient.DefaultRequestHeaders.Add(AntiforgeryConstants.CsrfTokenKey, csrfToken);
    }

    public static void AddOrUpdateAccessToken(this HttpClient httpClient, HttpResponseMessage response)
    {
        response.Headers.TryGetValues(Constants.ResponseSetCookieHeaderKey, out IEnumerable<string> cookies);
        if (cookies == null || !cookies.Any())
            return;
        var accessTokenCookie = cookies.FirstOrDefault(c => c.StartsWith(AuthConstants.AccessTokenCookieKey));
        if (string.IsNullOrWhiteSpace(accessTokenCookie))
            return;
        var regex = @$"{AuthConstants.AccessTokenCookieKey}=([^;]+);";
        var accessToken = Regex.Match(accessTokenCookie, regex, RegexOptions.IgnoreCase).Groups[1].Value;
        httpClient.AddToCookies(AuthConstants.AccessTokenCookieKey, accessToken);

        // Todo - For access token in header
        //if (httpClient.DefaultRequestHeaders.Contains(AntiforgeryConstants.AccessTokenCookieKey))
        //    httpClient.DefaultRequestHeaders.Remove(AntiforgeryConstants.AccessTokenCookieKey);
        //httpClient.DefaultRequestHeaders.Add(AntiforgeryConstants.AccessTokenCookieKey, accessToken);
    }

    public static void AddToCookies(this HttpClient httpClient, string key, string value)
    {
        httpClient.DefaultRequestHeaders.TryGetValues(Constants.RequestCookieHeaderKey, out IEnumerable<string> cookies);
        if (cookies == null || !cookies.Any())
            cookies = new List<string>();

        cookies = cookies.Append($"{key}={value}");
        httpClient.DefaultRequestHeaders.Add(Constants.RequestCookieHeaderKey, cookies);
    }
}
