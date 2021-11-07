using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Tools;
using FireplaceApi.Core.ValueObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Operators
{
    public class CommunityOperator
    {
        private readonly ILogger<CommunityOperator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly ICommunityRepository _communityRepository;
        private readonly PageOperator _pageOperator;

        public CommunityOperator(ILogger<CommunityOperator> logger, IConfiguration configuration,
            IServiceProvider serviceProvider, ICommunityRepository communityRepository,
            PageOperator pageOperator)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _communityRepository = communityRepository;
            _pageOperator = pageOperator;
        }

        public async Task<Page<Community>> ListCommunitiesAsync(User requesterUser,
            PaginationInputParameters paginationInputParameters, string name)
        {
            Page<Community> resultPage = default;
            if (string.IsNullOrWhiteSpace(paginationInputParameters.Pointer))
            {
                var communityIds = await _communityRepository.ListCommunityIdsAsync(name);
                resultPage = await _pageOperator.CreatePageWithoutPointerAsync(ModelName.COMMUNITY,
                    paginationInputParameters, communityIds,
                    _communityRepository.ListCommunitiesAsync);
            }
            else
            {
                resultPage = await _pageOperator.CreatePageWithPointerAsync(ModelName.COMMUNITY,
                    paginationInputParameters, _communityRepository.ListCommunitiesAsync);
            }
            return resultPage;
        }

        public async Task<Community> GetCommunityByIdAsync(ulong id, bool includeCreator)
        {
            var community = await _communityRepository.GetCommunityByIdAsync(id, includeCreator);
            if (community == null)
                return community;

            return community;
        }

        public async Task<Community> GetCommunityByNameAsync(string name, bool includeCreator)
        {
            var community = await _communityRepository.GetCommunityByNameAsync(name, includeCreator);
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

        public async Task<Community> CreateCommunityAsync(ulong creatorId, string name)
        {
            var id = await IdGenerator.GenerateNewIdAsync(DoesCommunityIdExistAsync);
            var community = await _communityRepository.CreateCommunityAsync(
                id, name, creatorId);
            return community;
        }

        public async Task<Community> PatchCommunityByIdAsync(ulong id, string name = null)
        {
            var community = await _communityRepository.GetCommunityByIdAsync(id);
            community = await ApplyCommunityChangesAsync(community, name);
            community = await GetCommunityByIdAsync(community.Id, false);
            return community;
        }

        public async Task<Community> PatchCommunityByNameAsync(string existingName, string name = null)
        {
            var community = await _communityRepository.GetCommunityByNameAsync(existingName);
            community = await ApplyCommunityChangesAsync(community, name);
            community = await GetCommunityByIdAsync(community.Id, false);
            return community;
        }

        public async Task DeleteCommunityByIdAsync(ulong id)
        {
            await _communityRepository.DeleteCommunityByIdAsync(id);
        }

        public async Task DeleteCommunityByNameAsync(string name)
        {
            await _communityRepository.DeleteCommunityByNameAsync(name);
        }

        public async Task<bool> DoesCommunityIdExistAsync(ulong id)
        {
            var communityIdExists = await _communityRepository.DoesCommunityIdExistAsync(id);
            return communityIdExists;
        }

        public async Task<bool> DoesCommunityNameExistAsync(string name)
        {
            return await _communityRepository.DoesCommunityNameExistAsync(name);
        }

        public async Task<Community> ApplyCommunityChangesAsync(Community community, string name = null)
        {
            if (name != null)
            {
                community.Name = name;
            }

            community = await _communityRepository.UpdateCommunityAsync(community);
            return community;
        }
    }
}
