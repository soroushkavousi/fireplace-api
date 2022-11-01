using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Identifiers;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Tools;
using FireplaceApi.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Operators
{
    public class CommunityOperator
    {
        private readonly ILogger<CommunityOperator> _logger;
        private readonly ICommunityRepository _communityRepository;

        public CommunityOperator(ILogger<CommunityOperator> logger,
            ICommunityRepository communityRepository)
        {
            _logger = logger;
            _communityRepository = communityRepository;
        }

        public async Task<QueryResult<Community>> ListCommunitiesAsync(string name, SortType? sort)
        {
            sort ??= Constants.DefaultSort;
            var communities = await _communityRepository.ListCommunitiesAsync(name, sort);
            var queryResult = new QueryResult<Community>(communities);
            return queryResult;
        }

        public async Task<List<Community>> ListCommunitiesByIdsAsync(List<ulong> ids)
        {
            if (ids.IsNullOrEmpty())
                return null;

            var communities = await _communityRepository
                .ListCommunitiesByIdsAsync(ids);
            return communities;
        }

        public async Task<Community> GetCommunityByIdentifierAsync(
            CommunityIdentifier identifier, bool includeCreator)
        {
            var community = await _communityRepository.GetCommunityByIdentifierAsync(
                identifier, includeCreator);
            if (community == null)
                return community;

            return community;
        }

        public async Task<string> GetNameByIdAsync(ulong id)
        {
            var communityName = await _communityRepository.GetNameByIdAsync(id);
            return communityName;
        }

        public async Task<ulong> GetIdByNameAsync(string name)
        {
            var communityId = await _communityRepository.GetIdByNameAsync(name);
            return communityId;
        }

        public async Task<Community> CreateCommunityAsync(User requestingUser, string name)
        {
            var id = await IdGenerator.GenerateNewIdAsync(
                id => DoesCommunityIdentifierExistAsync(CommunityIdentifier.OfId(id)));
            var community = await _communityRepository.CreateCommunityAsync(
                id, name, requestingUser.Id, requestingUser.Username);
            return community;
        }

        public async Task<Community> PatchCommunityByIdentifierAsync(CommunityIdentifier identifier,
            string name = null)
        {
            var community = await _communityRepository.GetCommunityByIdentifierAsync(identifier);
            return await PatchCommunityByIdentifierAsync(community, name);
        }

        public async Task<Community> PatchCommunityByIdentifierAsync(Community community,
            string name = null)
        {
            community = await ApplyCommunityChangesAsync(community, name);
            community = await GetCommunityByIdentifierAsync(
                CommunityIdentifier.OfId(community.Id), false);
            return community;
        }

        public async Task DeleteCommunityByIdentifierAsync(CommunityIdentifier identifier)
        {
            await _communityRepository.DeleteCommunityByIdentifierAsync(identifier);
        }

        public async Task<bool> DoesCommunityIdentifierExistAsync(CommunityIdentifier identifier)
        {
            var communityIdExists = await _communityRepository
                .DoesCommunityIdentifierExistAsync(identifier);
            return communityIdExists;
        }

        public async Task<Community> ApplyCommunityChangesAsync(Community community, string name = null)
        {
            var foundAnyChange = false;
            if (name != null)
            {
                community.Name = name;
                await _communityRepository.UpdateCommunityNameAsync(community.Id, name);
            }

            if (foundAnyChange)
                community = await _communityRepository.UpdateCommunityAsync(community);
            return community;
        }
    }
}
