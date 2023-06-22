using FireplaceApi.Application.Identifiers;
using FireplaceApi.Application.Models;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Interfaces;

public interface IConfigsRepository
{
    public Task<Configs> GetConfigsByIdentifierAsync(ConfigsIdentifier identifier);
    public Task<Configs> CreateConfigsAsync(Configs configs);
    public Task<Configs> UpdateConfigsAsync(Configs configs);
    public Task<bool> DoesConfigsIdentifierExistAsync(ConfigsIdentifier identifier);
}
