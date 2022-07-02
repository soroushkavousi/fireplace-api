using FireplaceApi.Api.Tools;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace FireplaceApi.Api.IntegrationTests.Extensions
{
    public static class HttpClientExtensions
    {
        public static void AddOrUpdateCsrfToken(this HttpClient httpClient, HttpResponseMessage response)
        {
            response.Headers.TryGetValues(Constants.SetCookieHeaderKey, out IEnumerable<string> cookies);
            if (cookies == null || !cookies.Any())
                return;
            var csrfTokenCookie = cookies.FirstOrDefault(c => c.StartsWith(Constants.CsrfTokenKey));
            if (string.IsNullOrWhiteSpace(csrfTokenCookie))
                return;
            var regex = @$"{Constants.CsrfTokenKey}=([^;]+);";
            var csrfToken = Regex.Match(csrfTokenCookie, regex, RegexOptions.IgnoreCase).Groups[1].Value;

            if (httpClient.DefaultRequestHeaders.Contains(Constants.CsrfTokenKey))
                httpClient.DefaultRequestHeaders.Remove(Constants.CsrfTokenKey);
            httpClient.DefaultRequestHeaders.Add(Constants.CsrfTokenKey, csrfToken);
        }

        public static void AddOrUpdateAccessToken(this HttpClient httpClient, HttpResponseMessage response)
        {
            response.Headers.TryGetValues(Constants.SetCookieHeaderKey, out IEnumerable<string> cookies);
            if (cookies == null || !cookies.Any())
                return;
            var accessTokenCookie = cookies.FirstOrDefault(c => c.StartsWith(Constants.ResponseAccessTokenCookieKey));
            if (string.IsNullOrWhiteSpace(accessTokenCookie))
                return;
            var regex = @$"{Constants.ResponseAccessTokenCookieKey}=([^;]+);";
            var accessToken = Regex.Match(accessTokenCookie, regex, RegexOptions.IgnoreCase).Groups[1].Value;

            if (httpClient.DefaultRequestHeaders.Contains(Constants.ResponseAccessTokenCookieKey))
                httpClient.DefaultRequestHeaders.Remove(Constants.ResponseAccessTokenCookieKey);
            httpClient.DefaultRequestHeaders.Add(Constants.ResponseAccessTokenCookieKey, accessToken);
        }
    }
}
