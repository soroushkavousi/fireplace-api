using FireplaceApi.Core.Identifiers;
using FireplaceApi.Core.Models;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Interfaces
{
    public interface IConfigsRepository
    {
        public Task<Configs> GetConfigsByIdentifierAsync(ConfigsIdentifier identifier);
        public Task<Configs> CreateConfigsAsync(Configs configs);
        public Task<Configs> UpdateConfigsAsync(Configs configs);
        public Task<bool> DoesConfigsIdentifierExistAsync(ConfigsIdentifier identifier);
    }
}
