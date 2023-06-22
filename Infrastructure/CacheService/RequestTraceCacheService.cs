using FireplaceApi.Application.Interfaces;
using FireplaceApi.Application.ValueObjects;
using FireplaceApi.Infrastructure.Gateways;
using System;
using System.Net;
using System.Threading.Tasks;

namespace FireplaceApi.Infrastructure.CacheService;

public class RequestTraceCacheService : IRequestTraceCacheService
{
    private readonly RedisGateway _redisGateway;

    public RequestTraceCacheService(RedisGateway redisGateway)
    {
        _redisGateway = redisGateway;
    }

    public async Task AddIpRequestTimeAsync(IPAddress ip, TimeSpan lifeSpan)
    {
        var ipRequestTimes = await ListIpRequestTimesAsync(ip);
        ipRequestTimes ??= new ExpirableCollection<ExpirableData>(lifeSpan);
        ipRequestTimes.Items.Add(new ExpirableData());
        await _redisGateway.SetData(MakeKeyForIpRequestTimes(ip), ipRequestTimes, lifeSpan);
    }

    public async Task<int> CoutIpRequestTimesAsync(IPAddress ip)
    {
        var ipRequestTimes = await _redisGateway
            .GetDataAsync<ExpirableCollection<ExpirableData>>(MakeKeyForIpRequestTimes(ip));
        if (ipRequestTimes == null)
            return 0;

        return ipRequestTimes.Items.Count;
    }

    public async Task<ExpirableCollection<ExpirableData>> ListIpRequestTimesAsync(IPAddress ip)
        => await _redisGateway.GetDataAsync<ExpirableCollection<ExpirableData>>(MakeKeyForIpRequestTimes(ip));

    private static string MakeKeyForIpRequestTimes(IPAddress ip)
    {
        var ipString = ip.ToString();
        if (ipString == "::1")
            ipString = "localhost";
        return $"request-traces:ips:{ipString}";
    }
}
