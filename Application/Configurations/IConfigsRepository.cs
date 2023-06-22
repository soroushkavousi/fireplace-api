using FireplaceApi.Domain.Configurations;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Configurations;

public interface IConfigsRepository
{
    public Task<Configs> GetConfigsByIdentifierAsync(ConfigsIdentifier identifier);
    public Task<Configs> CreateConfigsAsync(Configs configs);
    public Task<Configs> UpdateConfigsAsync(Configs configs);
    public Task<bool> DoesConfigsIdentifierExistAsync(ConfigsIdentifier identifier);
}
