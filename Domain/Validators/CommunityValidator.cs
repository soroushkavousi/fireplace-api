using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Exceptions;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Identifiers;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Operators;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Validators
{
    public class CommunityValidator : CoreValidator
    {
        private readonly ILogger<CommunityValidator> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly CommunityOperator _communityOperator;

        public List<ulong> Ids { get; private set; }
        public SortType? Sort { get; private set; }
        public CommunityIdentifier CommunityIdentifier { get; private set; }
        public Community Community { get; private set; }

        public CommunityValidator(ILogger<CommunityValidator> logger,
            IServiceProvider serviceProvider, CommunityOperator communityOperator)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _communityOperator = communityOperator;
        }

        public async Task ValidateListCommunitiesInputParametersAsync(string name, string sort)
        {
            Sort = (SortType?)ValidateInputEnum<CommunitySortType>(sort, nameof(sort), ErrorName.INPUT_SORT_IS_NOT_VALID);
            await Task.CompletedTask;
        }

        public async Task ValidateListCommunitiesByIdsInputParametersAsync(string encodedIds)
        {
            Ids = ValidateIdsFormat(encodedIds);
            await Task.CompletedTask;
        }

        public async Task ValidateGetCommunityByEncodedIdOrNameInputParametersAsync(
            User requestingUser, string encodedIdOrName, bool? includeCreator)
        {
            CommunityIdentifier = await ValidateMultipleIdentifiers(encodedIdOrName, encodedIdOrName);
        }

        public async Task ValidateCreateCommunityInputParametersAsync(User requestingUser, string name)
        {
            ValidateCommunityNameFormat(name);
            await ValidateCommunityIdentifierDoesNotExistAsync(CommunityIdentifier.OfName(name));
        }

        public async Task ValidatePatchCommunityByEncodedIdOrNameInputParametersAsync(User requestingUser,
            string encodedIdOrName, string newName)
        {
            CommunityIdentifier = await ValidateMultipleIdentifiers(encodedIdOrName, encodedIdOrName);
            Community = await _communityOperator.GetCommunityByIdentifierAsync(
                CommunityIdentifier, false);
            ValidateRequestingUserCanAlterCommunity(requestingUser, Community);
            ValidateCommunityNameFormat(newName);
            await ValidateCommunityIdentifierDoesNotExistAsync(CommunityIdentifier.OfName(newName));
        }

        public async Task ValidateDeleteCommunityByEncodedIdOrNameInputParametersAsync(User requestingUser,
            string encodedIdOrName)
        {
            CommunityIdentifier = await ValidateMultipleIdentifiers(encodedIdOrName, encodedIdOrName);
            Community = await _communityOperator.GetCommunityByIdentifierAsync(
                CommunityIdentifier, false);
            ValidateRequestingUserCanAlterCommunity(requestingUser, Community);
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
