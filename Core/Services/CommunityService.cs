using FireplaceApi.Core.Identifiers;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.Validators;
using FireplaceApi.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<QueryResult<Community>> ListCommunitiesAsync(string name, string sort)
        {
            await _communityValidator.ValidateListCommunitiesInputParametersAsync(name, sort);
            var page = await _communityOperator.ListCommunitiesAsync(name, _communityValidator.Sort);
            return page;
        }

        public async Task<List<Community>> ListCommunitiesByIdsAsync(string encodedIds)
        {
            await _communityValidator
                .ValidateListCommunitiesByIdsInputParametersAsync(encodedIds);
            var communities = await _communityOperator.ListCommunitiesByIdsAsync(
                _communityValidator.Ids);
            return communities;
        }

        public async Task<Community> GetCommunityByEncodedIdOrNameAsync(User requestingUser,
            string encodedIdOrName, bool? includeCreator)
        {
            var communityIdentifier = await _communityValidator.ValidateGetCommunityByEncodedIdOrNameInputParametersAsync(
                requestingUser, encodedIdOrName, includeCreator);
            var community = await _communityOperator.GetCommunityByIdentifierAsync(
                    communityIdentifier, includeCreator.Value);
            return community;
        }

        public async Task<Community> CreateCommunityAsync(User requestingUser, string name)
        {
            await _communityValidator
                .ValidateCreateCommunityInputParametersAsync(requestingUser, name);
            return await _communityOperator
                .CreateCommunityAsync(requestingUser, name);
        }

        public async Task<Community> PatchCommunityByEncodedIdOrNameAsync(User requestingUser,
            string encodedIdOrName, string newName)
        {
            var community = await _communityValidator.ValidatePatchCommunityByEncodedIdOrNameInputParametersAsync(
                requestingUser, encodedIdOrName, newName);
            community = await _communityOperator.ApplyCommunityChangesAsync(
                community, newName);
            community = await _communityOperator.GetCommunityByIdentifierAsync(
                CommunityIdentifier.OfId(community.Id), false);
            return community;
        }

        public async Task DeleteCommunityByEncodedIdOrNameAsync(User requestingUser, string encodedIdOrName)
        {
            var communityIdentifier = await _communityValidator.ValidateDeleteCommunityByEncodedIdOrNameInputParametersAsync(
                requestingUser, encodedIdOrName);
            await _communityOperator.DeleteCommunityByIdentifierAsync(
                    communityIdentifier);
        }
    }
}
