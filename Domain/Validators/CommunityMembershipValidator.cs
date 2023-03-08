using FireplaceApi.Domain.Exceptions;
using FireplaceApi.Domain.Identifiers;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Operators;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Validators
{
    public class CommunityMembershipValidator : DomainValidator
    {
        private readonly ILogger<CommunityMembershipValidator> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly CommunityMembershipOperator _communityMembershipOperator;
        private readonly CommunityValidator _communityValidator;

        public CommunityMembershipIdentifier CommunityMembershipIdentifier { get; private set; }
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
            CommunityIdentifier communityIdentifier)
        {
            UserIdentifier = UserIdentifier.OfId(requestingUser.Id);
            CommunityMembershipIdentifier = CommunityMembershipIdentifier
                .OfUserAndCommunity(UserIdentifier, communityIdentifier);
            await ValidateCommunityMembershipDoesNotAlreadyExist(CommunityMembershipIdentifier);
        }

        public async Task ValidateDeleteCommunityMembershipInputParametersAsync(User requestingUser,
            CommunityIdentifier communityIdentifier)
        {
            UserIdentifier = UserIdentifier.OfId(requestingUser.Id);
            CommunityMembershipIdentifier = CommunityMembershipIdentifier
                .OfUserAndCommunity(UserIdentifier, communityIdentifier);
            await ValidateCommunityMembershipAlreadyExists(CommunityMembershipIdentifier);
        }

        public void ValidateRequestingUserCanAlterCommunityMembership(User requestingUser,
            CommunityMembership communityMembership)
        {
            if (requestingUser.Id != communityMembership.UserId)
                throw new CommunityMembershipAccessDeniedException(requestingUser.Id, communityMembership.Id);
        }

        public async Task<bool> ValidateCommunityMembershipDoesNotAlreadyExist(
            CommunityMembershipIdentifier identifier, bool throwException = true)
        {
            if (await _communityMembershipOperator
                .DoesCommunityMembershipIdentifierExistAsync(identifier) == false)
                return true;

            if (throwException)
                throw new CommunityMembershipAlreadyExistsException(identifier);

            return false;
        }

        public async Task<bool> ValidateCommunityMembershipAlreadyExists(
            CommunityMembershipIdentifier identifier, bool throwException = true)
        {
            if (await _communityMembershipOperator
                .DoesCommunityMembershipIdentifierExistAsync(identifier))
                return true;

            if (throwException)
                throw new CommunityMembershipNotExistException(identifier);

            return false;
        }
    }
}
