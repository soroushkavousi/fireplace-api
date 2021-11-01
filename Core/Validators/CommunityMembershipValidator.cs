using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Validators
{
    public class CommunityMembershipValidator : ApiValidator
    {
        private readonly ILogger<CommunityMembershipValidator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly CommunityMembershipOperator _communityMembershipOperator;
        private readonly QueryResultValidator _queryResultValidator;
        private readonly CommunityValidator _communityValidator;

        public CommunityMembershipValidator(ILogger<CommunityMembershipValidator> logger, IConfiguration configuration,
            IServiceProvider serviceProvider, CommunityMembershipOperator communityMembershipOperator,
            QueryResultValidator queryResultValidator, CommunityValidator communityValidator)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _communityMembershipOperator = communityMembershipOperator;
            _queryResultValidator = queryResultValidator;
            _communityValidator = communityValidator;
        }

        public async Task ValidateCreateCommunityMembershipInputParametersAsync(User requesterUser,
            long? communityId, string communityName)
        {
            var community = _communityValidator.ValidateCommunityExistsAsync(communityId, communityName);
            await ValidateCommunityMembershipDoesNotAlreadyExist(requesterUser.Id, community.Id);
        }

        public async Task ValidateDeleteCommunityMembershipByIdInputParametersAsync(User requesterUser,
            long? communityId, string communityName)
        {
            var community = _communityValidator.ValidateCommunityExistsAsync(communityId, communityName);
            await ValidateCommunityMembershipAlreadyExists(requesterUser.Id, community.Id);
        }

        public async Task<CommunityMembership> ValidateCommunityMembershipExists(long id)
        {
            var communityMembership = await _communityMembershipOperator
                .GetCommunityMembershipByIdAsync(id, true, true);
            if (communityMembership == null)
            {
                var serverMessage = $"CommunityMembership id {id} doesn't exist!";
                throw new ApiException(ErrorName.POST_DOES_NOT_EXIST, serverMessage);
            }
            return communityMembership;
        }

        public void ValidateRequesterUserCanAlterCommunityMembership(User requesterUser,
            CommunityMembership communityMembership)
        {
            if (requesterUser.Id != communityMembership.UserId)
            {
                var serverMessage = $"requesterUser {requesterUser.Id} can't alter " +
                    $"community membership {communityMembership.Id}";
                throw new ApiException(ErrorName.USER_CAN_NOT_ALTER_COMMUNITY_MEMBERSHIP, serverMessage);
            }
        }

        public async Task ValidateCommunityMembershipDoesNotAlreadyExist(long userId, long communityId)
        {
            var exists = await _communityMembershipOperator
                .DoesCommunityMembershipExistAsync(userId, communityId);
            if (exists)
            {
                var serverMessage = $"CommunityMembership already exists! " + new { userId, communityId }.ToJson();
                throw new ApiException(ErrorName.COMMUNITY_MEMBERSHIP_ALREADY_EXISTS, serverMessage);
            }
        }

        public async Task ValidateCommunityMembershipAlreadyExists(long userId, long communityId)
        {
            var exists = await _communityMembershipOperator
                .DoesCommunityMembershipExistAsync(userId, communityId);
            if (!exists)
            {
                var serverMessage = $"CommunityMembership does not already exists! "
                    + new { userId, communityId }.ToJson();
                throw new ApiException(ErrorName.COMMUNITY_MEMBERSHIP_NOT_EXIST, serverMessage);
            }
        }
    }
}
