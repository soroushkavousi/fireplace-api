using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Exceptions;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Identifiers;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Operators;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Validators
{
    public class CommunityMembershipValidator : CoreValidator
    {
        private readonly ILogger<CommunityMembershipValidator> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly CommunityMembershipOperator _communityMembershipOperator;
        private readonly CommunityValidator _communityValidator;

        public CommunityMembershipIdentifier CommunityMembershipIdentifier { get; private set; }
        public CommunityIdentifier CommunityIdentitifer { get; private set; }
        public UserIdentifier UserIdentifier { get; private set; }

        public CommunityMembershipValidator(ILogger<CommunityMembershipValidator> logger,
            IServiceProvider serviceProvider, CommunityMembershipOperator communityMembershipOperator,
            CommunityValidator communityValidator)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _communityMembershipOperator = communityMembershipOperator;
            _communityValidator = communityValidator;
        }

        public async Task ValidateCreateCommunityMembershipInputParametersAsync(User requestingUser,
            string encodedCommunityId, string communityName)
        {
            CommunityIdentitifer = await _communityValidator
                .ValidateMultipleIdentifiers(encodedCommunityId, communityName);
            UserIdentifier = UserIdentifier.OfId(requestingUser.Id);
            CommunityMembershipIdentifier = CommunityMembershipIdentifier
                .OfUserAndCommunity(UserIdentifier, CommunityIdentitifer);
            await ValidateCommunityMembershipDoesNotAlreadyExist(CommunityMembershipIdentifier);
        }

        public async Task ValidateDeleteCommunityMembershipInputParametersAsync(User requestingUser,
            string communityEncodedIdOrName)
        {
            CommunityIdentitifer = await _communityValidator
                .ValidateMultipleIdentifiers(communityEncodedIdOrName, communityEncodedIdOrName);
            UserIdentifier = UserIdentifier.OfId(requestingUser.Id);
            CommunityMembershipIdentifier = CommunityMembershipIdentifier
                .OfUserAndCommunity(UserIdentifier, CommunityIdentitifer);
            await ValidateCommunityMembershipAlreadyExists(CommunityMembershipIdentifier);
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
