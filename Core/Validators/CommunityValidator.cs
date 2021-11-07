using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.ValueObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Validators
{
    public class CommunityValidator : ApiValidator
    {
        private readonly ILogger<CommunityValidator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly CommunityOperator _communityOperator;
        private readonly QueryResultValidator _queryResultValidator;


        public CommunityValidator(ILogger<CommunityValidator> logger, IConfiguration configuration,
            IServiceProvider serviceProvider, CommunityOperator communityOperator,
            QueryResultValidator queryResultValidator)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _communityOperator = communityOperator;
            _queryResultValidator = queryResultValidator;
        }

        public async Task ValidateListCommunitiesInputParametersAsync(User requesterUser,
            PaginationInputParameters paginationInputParameters, string name)
        {
            await _queryResultValidator.ValidatePaginationInputParameters(paginationInputParameters,
                ModelName.COMMUNITY);
        }

        public async Task ValidateGetCommunityByIdInputParametersAsync(User requesterUser, string encodedId,
            bool? includeCreator)
        {
            var id = ValidateEncodedIdFormatValid(encodedId, nameof(encodedId));
            var community = await ValidateCommunityExistsAsync(id: id);
        }

        public async Task ValidateGetCommunityByNameInputParametersAsync(User requesterUser, string name,
            bool? includeCreator)
        {
            var community = await ValidateCommunityExistsAsync(name: name);
        }

        public async Task ValidateCreateCommunityInputParametersAsync(User requesterUser, string name)
        {
            await ValidateCommunityNameDoesNotExistAsync(name);
        }

        public async Task ValidatePatchCommunityByIdInputParametersAsync(User requesterUser,
            string encodedId, string newName)
        {
            var id = ValidateEncodedIdFormatValid(encodedId, nameof(encodedId));
            var community = await ValidateCommunityExistsAsync(id: id);
            ValidateRequesterUserCanAlterCommunity(requesterUser, community);
            await ValidateCommunityNameDoesNotExistAsync(newName);
        }

        public async Task ValidatePatchCommunityByNameInputParametersAsync(User requesterUser,
            string name, string newName)
        {
            var community = await ValidateCommunityExistsAsync(name: name);
            ValidateRequesterUserCanAlterCommunity(requesterUser, community);
            await ValidateCommunityNameDoesNotExistAsync(newName);
        }

        public async Task ValidateDeleteCommunityByIdInputParametersAsync(User requesterUser,
            string encodedId)
        {
            var id = ValidateEncodedIdFormatValid(encodedId, nameof(encodedId));
            var community = await ValidateCommunityExistsAsync(id: id);
            ValidateRequesterUserCanAlterCommunity(requesterUser, community);
        }

        public async Task ValidateDeleteCommunityByNameInputParametersAsync(User requesterUser,
            string name)
        {
            var community = await ValidateCommunityExistsAsync(name: name);
            ValidateRequesterUserCanAlterCommunity(requesterUser, community);
        }

        public async Task<Community> ValidateCommunityExistsAsync(ulong? id = null, string name = null)
        {
            if (id.HasValue)
            {
                var community = await _communityOperator.GetCommunityByIdAsync(id.Value, true);
                if (community == null)
                {
                    var serverMessage = $"Community id {id} doesn't exist!";
                    throw new ApiException(ErrorName.COMMUNITY_DOES_NOT_EXIST, serverMessage);
                }
                return community;
            }
            else if (!string.IsNullOrWhiteSpace(name))
            {
                var community = await _communityOperator.GetCommunityByNameAsync(name, true);
                if (community == null)
                {
                    var serverMessage = $"Community name {name} doesn't exist!";
                    throw new ApiException(ErrorName.COMMUNITY_DOES_NOT_EXIST, serverMessage);
                }
                return community;
            }
            else
            {
                var serverMessage = $"Community id and name are missing!";
                throw new ApiException(ErrorName.COMMUNITY_ID_AND_NAME_ARE_MISSING, serverMessage);
            }
        }

        public async Task ValidateCommunityNameDoesNotExistAsync(string name)
        {
            var exists = await _communityOperator.DoesCommunityNameExistAsync(name);
            if (exists)
            {
                var serverMessage = $"Community name {name} already exists!";
                throw new ApiException(ErrorName.COMMUNITY_ALREADY_EXISTS, serverMessage);
            }
        }

        public void ValidateRequesterUserCanAlterCommunity(User requesterUser,
            Community community)
        {
            if (requesterUser.Id != community.CreatorId)
            {
                var serverMessage = $"requesterUser {requesterUser.Id} can't alter " +
                    $"community {community.Id}";
                throw new ApiException(ErrorName.USER_CAN_NOT_ALTER_COMMUNITY,
                    serverMessage);
            }
        }
    }
}
