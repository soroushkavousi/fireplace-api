using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Identifiers;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Validators
{
    public class CommunityValidator : ApiValidator
    {
        private readonly ILogger<CommunityValidator> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly CommunityOperator _communityOperator;
        private readonly QueryResultValidator _queryResultValidator;


        public CommunityValidator(ILogger<CommunityValidator> logger,
            IServiceProvider serviceProvider, CommunityOperator communityOperator,
            QueryResultValidator queryResultValidator)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _communityOperator = communityOperator;
            _queryResultValidator = queryResultValidator;
        }

        public async Task ValidateListCommunitiesInputParametersAsync(User requestingUser,
            PaginationInputParameters paginationInputParameters, string name)
        {
            await _queryResultValidator.ValidatePaginationInputParameters(paginationInputParameters,
                ModelName.COMMUNITY);
        }

        public async Task<CommunityIdentifier> ValidateGetCommunityByEncodedIdOrNameInputParametersAsync(
            User requestingUser, string encodedIdOrName, bool? includeCreator)
        {
            var communityIdentifier = await ValidateMultipleIdentifiers(encodedIdOrName, encodedIdOrName);
            return communityIdentifier;
        }

        public async Task ValidateCreateCommunityInputParametersAsync(User requestingUser, string name)
        {
            ValidateCommunityNameFormat(name);
            await ValidateCommunityIdentifierDoesNotExistAsync(CommunityIdentifier.OfName(name));
        }

        public async Task<Community> ValidatePatchCommunityByEncodedIdOrNameInputParametersAsync(User requestingUser,
            string encodedIdOrName, string newName)
        {
            var communityIdentifier = await ValidateMultipleIdentifiers(encodedIdOrName, encodedIdOrName);
            var community = await _communityOperator.GetCommunityByIdentifierAsync(
                communityIdentifier, false);
            ValidateRequestingUserCanAlterCommunity(requestingUser, community);
            ValidateCommunityNameFormat(newName);
            await ValidateCommunityIdentifierDoesNotExistAsync(CommunityIdentifier.OfName(newName));
            return community;
        }

        public async Task<CommunityIdentifier> ValidateDeleteCommunityByEncodedIdOrNameInputParametersAsync(User requestingUser,
            string encodedIdOrName)
        {
            var communityIdentifier = await ValidateMultipleIdentifiers(encodedIdOrName, encodedIdOrName);
            var community = await _communityOperator.GetCommunityByIdentifierAsync(
                communityIdentifier, false);
            ValidateRequestingUserCanAlterCommunity(requestingUser, community);
            return communityIdentifier;
        }

        public async Task<CommunityIdentifier> ValidateMultipleIdentifiers(string encodedId,
            string name, bool throwException = true)
        {
            var id = ValidateEncodedIdFormat(encodedId, "communityName", false);
            if (id.HasValue)
            {
                var identifier = CommunityIdentifier.OfId(id.Value);
                if (await ValidateCommunityIdentifierExists(identifier, false))
                    return identifier;
            }

            if (ValidateCommunityNameFormat(name, false))
            {
                var identifier = CommunityIdentifier.OfName(name);
                if (await ValidateCommunityIdentifierExists(identifier, false))
                    return identifier;
            }

            if (throwException)
            {
                var serverMessage = $"Community {new { encodedId, id, name }.ToJson()} deos not exist!";
                throw new ApiException(ErrorName.COMMUNITY_DOES_NOT_EXIST, serverMessage);
            }
            return default;
        }

        public bool ValidateCommunityIdentifierFormat(
            CommunityIdentifier identifier, bool throwException = true)
        {
            switch (identifier)
            {
                case CommunityIdIdentifier idIdentifier:
                    break;
                case CommunityNameIdentifier nameIdentifier:
                    if (!ValidateCommunityNameFormat(nameIdentifier.Name, throwException))
                        return false;
                    break;
            }
            return true;
        }

        public bool ValidateCommunityNameFormat(string communityName, bool throwException = true)
        {
            return true;
        }

        public async Task<bool> ValidateCommunityIdentifierExists(
            CommunityIdentifier identifier, bool throwException = true)
        {
            if (await _communityOperator.DoesCommunityIdentifierExistAsync(identifier))
                return true;

            if (throwException)
            {
                var serverMessage = $"Community identifier {identifier.ToJson()} deos not exist!";
                throw new ApiException(ErrorName.COMMUNITY_DOES_NOT_EXIST, serverMessage);
            }
            return false;
        }


        public async Task<bool> ValidateCommunityIdentifierDoesNotExistAsync(
            CommunityIdentifier identifier, bool throwException = true)
        {
            if (await _communityOperator.DoesCommunityIdentifierExistAsync(identifier) == false)
                return true;

            if (throwException)
            {
                var serverMessage = $"Community {identifier.ToJson()} already exists!";
                throw new ApiException(ErrorName.COMMUNITY_ALREADY_EXISTS, serverMessage);
            }
            return false;
        }

        public void ValidateRequestingUserCanAlterCommunity(User requestingUser,
            Community community)
        {
            if (requestingUser.Id != community.CreatorId)
            {
                var serverMessage = $"requestingUser {requestingUser.Id} can't alter " +
                    $"community {community.Id}";
                throw new ApiException(ErrorName.USER_CAN_NOT_ALTER_COMMUNITY,
                    serverMessage);
            }
        }
    }
}
