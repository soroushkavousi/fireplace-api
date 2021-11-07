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
        public Task<Community> GetCommunityByIdAsync(ulong id, bool includeCreator = false);
        public Task<Community> GetCommunityByNameAsync(string name, bool includeCreator = false);
        public Task<string> GetNameByIdAsync(ulong id);
        public Task<ulong> GetIdByNameAsync(string name);
        public Task<Community> CreateCommunityAsync(ulong id, string name, ulong creatorId);
        public Task<Community> UpdateCommunityAsync(Community community);
        public Task DeleteCommunityByIdAsync(ulong id);
        public Task DeleteCommunityByNameAsync(string name);
        public Task<bool> DoesCommunityIdExistAsync(ulong id);
        public Task<bool> DoesCommunityNameExistAsync(string name);

    }
}
