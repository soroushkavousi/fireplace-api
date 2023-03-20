using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Identifiers;
using FireplaceApi.Domain.Interfaces;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Tools;
using FireplaceApi.Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Operators
{
    public class CommunityOperator
    {
        private readonly ILogger<CommunityOperator> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly ICommunityRepository _communityRepository;

        public CommunityOperator(ILogger<CommunityOperator> logger,
            IServiceProvider serviceProvider, ICommunityRepository communityRepository)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _communityRepository = communityRepository;
        }

        public async Task<QueryResult<Community>> ListCommunitiesAsync(string search, SortType? sort)
        {
            sort ??= Constants.DefaultSort;
            var communities = await _communityRepository.ListCommunitiesAsync(search, sort);
            var queryResult = new QueryResult<Community>(communities);
            return queryResult;
        }

        public async Task<QueryResult<Community>> ListJoinedCommunitiesAsync(User requestingUser, SortType? sort)
        {
            sort ??= Constants.DefaultSort;
            var communityMembershipOperator = _serviceProvider.GetService<CommunityMembershipOperator>();
            var communityMemberships = await communityMembershipOperator.SearchCommunityMembershipsAsync(
                userIdentifier: UserIdentifier.OfId(requestingUser.Id), includeCommunity: true);
            var communities = communityMemberships.Select(cm => cm.Community).ToList();
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
            var communityMembershipOperator = _serviceProvider.GetService<CommunityMembershipOperator>();
            var membership = await communityMembershipOperator
                .CreateCommunityMembershipAsync(requestingUser, CommunityIdentifier.OfId(community.Id));
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
