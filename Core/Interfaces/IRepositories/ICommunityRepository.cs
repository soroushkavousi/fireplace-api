using FireplaceApi.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Interfaces
{
    public interface ICommunityRepository
    {
        public Task<List<Community>> ListCommunitiesAsync(List<long> Ids);
        public Task<List<Community>> ListCommunitiesAsync(string name);
        public Task<List<long>> ListCommunityIdsAsync(string name);
        public Task<Community> GetCommunityByIdAsync(long id, bool includeCreator = false);
        public Task<Community> GetCommunityByNameAsync(string name, bool includeCreator = false);
        public Task<string> GetNameByIdAsync(long id);
        public Task<long> GetIdByNameAsync(string name);
        public Task<Community> CreateCommunityAsync(string name, long creatorId);
        public Task<Community> UpdateCommunityAsync(Community community);
        public Task DeleteCommunityByIdAsync(long id);
        public Task DeleteCommunityByNameAsync(string name);
        public Task<bool> DoesCommunityIdExistAsync(long id);
        public Task<bool> DoesCommunityNameExistAsync(string name);

    }
}
