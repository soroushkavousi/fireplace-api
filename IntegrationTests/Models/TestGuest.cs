using System.Net.Http;

namespace FireplaceApi.IntegrationTests.Models;

public class TestGuest
{
    public HttpClient HttpClient { get; set; }

    public TestGuest(HttpClient httpClient)
    {
        HttpClient = httpClient;
    }
}