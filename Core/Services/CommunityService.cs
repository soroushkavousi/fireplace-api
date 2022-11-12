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
            var queryResult = await _communityOperator.ListCommunitiesAsync(
                name, _communityValidator.Sort);
            return queryResult;
        }

        public async Task<List<Community>> ListCommunitiesByIdsAsync(string encodedIds)
        {
            await _communityValidator.ValidateListCommunitiesByIdsInputParametersAsync(encodedIds);
            var communities = await _communityOperator.ListCommunitiesByIdsAsync(
                _communityValidator.Ids);
            return communities;
        }

        public async Task<Community> GetCommunityByEncodedIdOrNameAsync(User requestingUser,
            string encodedIdOrName, bool? includeCreator)
        {
            await _communityValidator.ValidateGetCommunityByEncodedIdOrNameInputParametersAsync(
                requestingUser, encodedIdOrName, includeCreator);
            var community = await _communityOperator.GetCommunityByIdentifierAsync(
                _communityValidator.CommunityIdentifier, includeCreator.Value);
            return community;
        }

        public async Task<Community> CreateCommunityAsync(User requestingUser, string name)
        {
            await _communityValidator.ValidateCreateCommunityInputParametersAsync(
                requestingUser, name);
            return await _communityOperator.CreateCommunityAsync(requestingUser, name);
        }

        public async Task<Community> PatchCommunityByEncodedIdOrNameAsync(User requestingUser,
            string encodedIdOrName, string newName)
        {
            await _communityValidator.ValidatePatchCommunityByEncodedIdOrNameInputParametersAsync(
                requestingUser, encodedIdOrName, newName);
            var community = await _communityOperator.PatchCommunityByIdentifierAsync(
                _communityValidator.Community, newName);
            return community;
        }

        public async Task DeleteCommunityByEncodedIdOrNameAsync(User requestingUser, string encodedIdOrName)
        {
            await _communityValidator.ValidateDeleteCommunityByEncodedIdOrNameInputParametersAsync(
                requestingUser, encodedIdOrName);
            await _communityOperator.DeleteCommunityByIdentifierAsync(
                _communityValidator.CommunityIdentifier);
        }
    }
}
