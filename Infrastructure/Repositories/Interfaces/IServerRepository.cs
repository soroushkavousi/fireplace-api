using FireplaceApi.Infrastructure.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Infrastructure.Repositories;

public interface IServerRepository
{
    public Task<List<ulong>> ListServerIdsAsync();
    public Task<ServerEntity> GetServerAsync(ServerIdentifier identifier);
    public Task<ServerEntity> CreateServerAsync(ulong id, string macAddress);
    public Task DeleteServerAsync(ulong id);
    public Task<bool> DoesServerIdExistAsync(ulong id);
}
