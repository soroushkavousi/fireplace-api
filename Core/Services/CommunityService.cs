using Microsoft.Extensions.Logging;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FireplaceApi.Core.Operators;
using System.Net;
using FireplaceApi.Core.ValueObjects;

namespace FireplaceApi.Core.Services
{
    public class CommunityService
    {
        private readonly ILogger<CommunityService> _logger;
        private readonly CommunityValidator _communityValidator;
        private readonly CommunityOperator _communityOperator;
        
        public CommunityService(ILogger<CommunityService> logger, 
            CommunityValidator communityValidator, CommunityOperator communityOperator)
        {
            _logger = logger;
            _communityValidator = communityValidator;
            _communityOperator = communityOperator;
        }

        public async Task<Page<Community>> ListCommunitiesAsync(User requesterUser, 
            PaginationInputParameters paginationInputParameters, string name)
        {
            await _communityValidator.ValidateListCommunitiesInputParametersAsync(requesterUser,
                paginationInputParameters, name);
            var page = await _communityOperator.ListCommunitiesAsync(requesterUser,
                paginationInputParameters, name);
            return page;
        } 

        public async Task<Community> GetCommunityByIdAsync(User requesterUser, long? id, bool? includeCreator)
        {
            await _communityValidator.ValidateGetCommunityByIdInputParametersAsync(requesterUser, id, includeCreator);
            var community = await _communityOperator.GetCommunityByIdAsync(id.Value, includeCreator.Value);
            return community;
        }

        public async Task<Community> GetCommunityByNameAsync(User requesterUser, string name, bool? includeCreator)
        {
            await _communityValidator.ValidateGetCommunityByNameInputParametersAsync(requesterUser, name, includeCreator);
            var community = await _communityOperator.GetCommunityByNameAsync(name, includeCreator.Value);
            return community;
        }

        public async Task<Community> CreateCommunityAsync(string name, long creatorId)
        {
            await _communityValidator
                .ValidateCreateCommunityInputParametersAsync(name, creatorId);
            return await _communityOperator
                .CreateCommunityAsync(name, creatorId);
        }

        public async Task<Community> PatchCommunityByIdAsync(User requesterUser, long? id)
        {
            await _communityValidator.ValidatePatchCommunityByIdInputParametersAsync(requesterUser, id);
            var community = await _communityOperator.PatchCommunityByIdAsync(id.Value);
            return community;
        }

        public async Task<Community> PatchCommunityByNameAsync(User requesterUser, string name)
        {
            await _communityValidator.ValidatePatchCommunityByNameInputParametersAsync(requesterUser, name);
            var community = await _communityOperator.PatchCommunityByNameAsync(name);
            return community;
        }

        public async Task DeleteCommunityByIdAsync(User requesterUser, long? id)
        {
            await _communityValidator.ValidateDeleteCommunityByIdInputParametersAsync(requesterUser, id);
            await _communityOperator.DeleteCommunityByIdAsync(id.Value);
        }

        public async Task DeleteCommunityByNameAsync(User requesterUser, string name)
        {
            await _communityValidator.ValidateDeleteCommunityByNameInputParametersAsync(requesterUser, name);
            await _communityOperator.DeleteCommunityByNameAsync(name);
        }
    }
}
