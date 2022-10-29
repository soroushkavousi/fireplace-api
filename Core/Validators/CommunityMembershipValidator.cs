using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Identifiers;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Validators
{
    public class CommunityMembershipValidator : ApiValidator
    {
        private readonly ILogger<CommunityMembershipValidator> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly CommunityMembershipOperator _communityMembershipOperator;
        private readonly CommunityValidator _communityValidator;

        public CommunityMembershipValidator(ILogger<CommunityMembershipValidator> logger,
            IServiceProvider serviceProvider, CommunityMembershipOperator communityMembershipOperator,
            CommunityValidator communityValidator)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _communityMembershipOperator = communityMembershipOperator;
            _communityValidator = communityValidator;
        }

        public async Task<CommunityIdentifier> ValidateCreateCommunityMembershipInputParametersAsync(User requestingUser,
            string encodedCommunityId, string communityName)
        {
            var communityIdentifier = await _communityValidator
                .ValidateMultipleIdentifiers(encodedCommunityId, communityName);
            var userIdentifier = UserIdentifier.OfId(requestingUser.Id);
            var communityMembershipIdentifier = CommunityMembershipIdentifier
                .OfUserAndCommunity(userIdentifier, communityIdentifier);
            await ValidateCommunityMembershipDoesNotAlreadyExist(communityMembershipIdentifier);

            return communityIdentifier;
        }

        public async Task<CommunityMembershipIdentifier> ValidateDeleteCommunityMembershipInputParametersAsync(User requestingUser,
            string communityEncodedIdOrName)
        {
            var communityIdentifier = await _communityValidator
                .ValidateMultipleIdentifiers(communityEncodedIdOrName, communityEncodedIdOrName);
            var userIdentifier = UserIdentifier.OfId(requestingUser.Id);
            var communityMembershipIdentifier = CommunityMembershipIdentifier.OfUserAndCommunity(userIdentifier, communityIdentifier);
            await ValidateCommunityMembershipAlreadyExists(communityMembershipIdentifier);

            return communityMembershipIdentifier;
        }

        public void ValidateRequestingUserCanAlterCommunityMembership(User requestingUser,
            CommunityMembership communityMembership)
        {
            if (requestingUser.Id != communityMembership.UserId)
            {
                var serverMessage = $"requestingUser {requestingUser.Id} can't alter " +
                    $"community membership {communityMembership.Id}";
                throw new ApiException(ErrorName.USER_CAN_NOT_ALTER_COMMUNITY_MEMBERSHIP, serverMessage);
            }
        }

        public async Task<bool> ValidateCommunityMembershipDoesNotAlreadyExist(
            CommunityMembershipIdentifier identifier, bool throwException = true)
        {
            if (await _communityMembershipOperator
                .DoesCommunityMembershipIdentifierExistAsync(identifier) == false)
                return true;

            if (throwException)
            {
                var serverMessage = $"CommunityMembership already exists! " + identifier.ToJson();
                throw new ApiException(ErrorName.COMMUNITY_MEMBERSHIP_ALREADY_EXISTS, serverMessage);
            }
            return false;
        }

        public async Task<bool> ValidateCommunityMembershipAlreadyExists(
            CommunityMembershipIdentifier identifier, bool throwException = true)
        {
            if (await _communityMembershipOperator
                .DoesCommunityMembershipIdentifierExistAsync(identifier))
                return true;

            if (throwException)
            {
                var serverMessage = $"CommunityMembership does not already exists! "
                    + identifier.ToJson();
                throw new ApiException(ErrorName.COMMUNITY_MEMBERSHIP_NOT_EXIST, serverMessage);
            }
            return false;
        }
    }
}
