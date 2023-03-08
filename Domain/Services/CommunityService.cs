using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Identifiers;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Operators;
using FireplaceApi.Domain.Validators;
using FireplaceApi.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Services
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

        public async Task<QueryResult<Community>> ListCommunitiesAsync(string name, SortType? sort)
        {
            await _communityValidator.ValidateListCommunitiesInputParametersAsync(name, sort);
            var queryResult = await _communityOperator.ListCommunitiesAsync(name, sort);
            return queryResult;
        }

        public async Task<List<Community>> ListCommunitiesByIdsAsync(List<ulong> ids)
        {
            await _communityValidator.ValidateListCommunitiesByIdsInputParametersAsync(ids);
            var communities = await _communityOperator.ListCommunitiesByIdsAsync(ids);
            return communities;
        }

        public async Task<Community> GetCommunityByIdentifierAsync(User requestingUser,
            CommunityIdentifier identifier, bool? includeCreator)
        {
            await _communityValidator.ValidateGetCommunityByIdentifierInputParametersAsync(
                requestingUser, identifier, includeCreator);
            var community = await _communityOperator.GetCommunityByIdentifierAsync(
                identifier, includeCreator.Value);
            return community;
        }

        public async Task<Community> CreateCommunityAsync(User requestingUser, string name)
        {
            await _communityValidator.ValidateCreateCommunityInputParametersAsync(
                requestingUser, name);
            return await _communityOperator.CreateCommunityAsync(requestingUser, name);
        }

        public async Task<Community> PatchCommunityByIdentifierAsync(User requestingUser,
            CommunityIdentifier identifier, string newName)
        {
            await _communityValidator.ValidatePatchCommunityByIdentifierInputParametersAsync(
                requestingUser, identifier, newName);
            var community = await _communityOperator.PatchCommunityByIdentifierAsync(
                _communityValidator.Community, newName);
            return community;
        }

        public async Task DeleteCommunityByIdentifierAsync(User requestingUser, CommunityIdentifier identifier)
        {
            await _communityValidator.ValidateDeleteCommunityByIdentifierInputParametersAsync(
                requestingUser, identifier);
            await _communityOperator.DeleteCommunityByIdentifierAsync(identifier);
        }
    }
}
