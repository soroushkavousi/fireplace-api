using FireplaceApi.Domain.Identifiers;
using FireplaceApi.Domain.Models;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Interfaces
{
    public interface IConfigsRepository
    {
        public Task<Configs> GetConfigsByIdentifierAsync(ConfigsIdentifier identifier);
        public Task<Configs> CreateConfigsAsync(Configs configs);
        public Task<Configs> UpdateConfigsAsync(Configs configs);
        public Task<bool> DoesConfigsIdentifierExistAsync(ConfigsIdentifier identifier);
    }
}
