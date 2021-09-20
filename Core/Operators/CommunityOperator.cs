using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FireplaceApi.Core.ValueObjects;
using FireplaceApi.Core.Tools;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Interfaces;

namespace FireplaceApi.Core.Operators
{
    public class CommunityOperator
    {
        private readonly ILogger<CommunityOperator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly ICommunityRepository _communityRepository;

        public CommunityOperator(ILogger<CommunityOperator> logger, IConfiguration configuration,
            IServiceProvider serviceProvider, ICommunityRepository communityRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _communityRepository = communityRepository;
        }

        public async Task<Page<Community>> ListCommunitiesAsync()
        {
            var communities = await _communityRepository.ListCommunitiesAsync();
            var pagination = new Pagination(-1);
            var page = new Page<Community>(-1, communities, pagination);
            return page;
        }

        public async Task<Community> GetCommunityByIdAsync(long id, bool includeCreator)
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

        public async Task<Community> CreateCommunityAsync(string name, long creatorId)
        {
            var community = await _communityRepository.CreateCommunityAsync(name, creatorId);
            return community;
        }

        public async Task<Community> PatchCommunityByIdAsync(long id, string name = null)
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

        public async Task DeleteCommunityAsync(long id)
        {
            await _communityRepository.DeleteCommunityAsync(id);
        }

        public async Task<bool> DoesCommunityIdExistAsync(long id)
        {
            var communityIdExists = await _communityRepository.DoesCommunityIdExistAsync(id);
            return communityIdExists;
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

        public async Task<bool> DoesCommunityNameExistAsync(string name)
        {
            return await _communityRepository.DoesCommunityNameExistAsync(name);
        }
    }
}
