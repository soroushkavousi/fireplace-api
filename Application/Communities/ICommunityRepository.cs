﻿using FireplaceApi.Domain.Communities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Communities;

public interface ICommunityRepository
{
    public Task<List<Community>> ListCommunitiesAsync(string search, CommunitySortType sort);
    public Task<List<Community>> ListCommunitiesByIdsAsync(List<ulong> Ids);
    public Task<Community> GetCommunityByIdentifierAsync(CommunityIdentifier identifier,
        bool includeCreator = false);
    public Task<string> GetNameByIdAsync(ulong id);
    public Task<ulong> GetIdByNameAsync(string name);
    public Task<Community> CreateCommunityAsync(ulong id, string name,
        ulong creatorId, string creatorUsername);
    public Task<Community> UpdateCommunityAsync(Community community);
    public Task UpdateCommunityNameAsync(ulong id, string newCommunityName);
    public Task DeleteCommunityByIdentifierAsync(CommunityIdentifier identifier);
    public Task<bool> DoesCommunityIdentifierExistAsync(CommunityIdentifier identifier);

}
