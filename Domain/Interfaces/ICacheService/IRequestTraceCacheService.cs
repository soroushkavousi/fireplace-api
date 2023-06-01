using FireplaceApi.Domain.ValueObjects;
using System;
using System.Net;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Interfaces;

public interface IRequestTraceCacheService
{
    public Task AddIpRequestTimeAsync(IPAddress ip, TimeSpan lifeSpan);
    public Task<int> CoutIpRequestTimesAsync(IPAddress ip);
    public Task<ExpirableCollection<ExpirableData>> ListIpRequestTimesAsync(IPAddress ip);
}
