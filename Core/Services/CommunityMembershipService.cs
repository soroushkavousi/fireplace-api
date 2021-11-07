using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.Validators;
using FireplaceApi.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Services
{
    public class CommunityMembershipService
    {
        private readonly ILogger<CommunityMembershipService> _logger;
        private readonly CommunityMembershipValidator _communityMembershipValidator;
        private readonly CommunityMembershipOperator _communityMembershipOperator;

        public CommunityMembershipService(ILogger<CommunityMembershipService> logger,
            CommunityMembershipValidator communityMembershipValidator, CommunityMembershipOperator communityMembershipOperator)
        {
            _logger = logger;
            _communityMembershipValidator = communityMembershipValidator;
            _communityMembershipOperator = communityMembershipOperator;
        }

        public async Task<CommunityMembership> CreateCommunityMembershipAsync(
            User requesterUser, string encodedCommunityId, string communityName)
        {
            await _communityMembershipValidator
                .ValidateCreateCommunityMembershipInputParametersAsync(
                requesterUser, encodedCommunityId, communityName);
            var communityId = encodedCommunityId.DecodeIdOrDefault();
            var communityIdentifier = new Identifier(communityId, communityName);
            return await _communityMembershipOperator
                .CreateCommunityMembershipAsync(requesterUser,
                    communityIdentifier);
        }

        public async Task DeleteCommunityMembershipAsync(User requesterUser,
            string encodedCommunityId, string communityName)
        {
            await _communityMembershipValidator
                .ValidateDeleteCommunityMembershipByIdInputParametersAsync(
                    requesterUser, encodedCommunityId, communityName);
            var communityId = encodedCommunityId.DecodeIdOrDefault();
            var communityIdentifier = new Identifier(communityId, communityName);
            await _communityMembershipOperator
                .DeleteCommunityMembershipByIdAsync(requesterUser.Id, communityIdentifier);
        }
    }
}
