using FireplaceApi.Core.Identifiers;
using FireplaceApi.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Interfaces
{
    public interface ICommunityRepository
    {
        public Task<List<Community>> ListCommunitiesAsync(List<ulong> Ids);
        public Task<List<Community>> ListCommunitiesAsync(string name);
        public Task<List<ulong>> ListCommunityIdsAsync(string name);
        public Task<Community> GetCommunityByIdentifierAsync(CommunityIdentifier identifier,
            bool includeCreator = false);
        public Task<string> GetNameByIdAsync(ulong id);
        public Task<ulong> GetIdByNameAsync(string name);
        public Task<Community> CreateCommunityAsync(ulong id, string name,
            ulong creatorId, string creatorUsername);
        public Task<Community> UpdateCommunityAsync(Community community);
        public Task DeleteCommunityByIdentifierAsync(CommunityIdentifier identifier);
        public Task<bool> DoesCommunityIdentifierExistAsync(CommunityIdentifier identifier);

    }
}
