using FireplaceApi.Domain.Communities;
using FireplaceApi.Domain.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Communities;

public interface ICommunityRepository
{
    public Task<List<Community>> ListCommunitiesAsync(string search, CommunitySortType sort);
    public Task<List<Community>> ListCommunitiesByIdsAsync(List<ulong> Ids);
    public Task<Community> GetCommunityAsync(CommunityIdentifier identifier,
        bool includeCreator = false);
    public Task<CommunityName> GetNameByIdAsync(ulong id);
    public Task<ulong> GetIdByNameAsync(CommunityName name);
    public Task<Community> CreateCommunityAsync(CommunityName name,
        ulong creatorId, Username creatorUsername);
    public Task<Community> UpdateCommunityAsync(Community community);
    public Task UpdateCommunityNameAsync(ulong id, CommunityName newCommunityName);
    public Task DeleteCommunityAsync(CommunityIdentifier identifier);
    public Task<bool> DoesCommunityExistAsync(CommunityIdentifier identifier);

}
