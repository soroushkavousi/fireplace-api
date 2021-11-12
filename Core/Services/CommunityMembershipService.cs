using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.Validators;
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
            User requestingUser, string encodedCommunityId, string communityName)
        {
            var communityIdentifier = await _communityMembershipValidator
                .ValidateCreateCommunityMembershipInputParametersAsync(
                requestingUser, encodedCommunityId, communityName);
            return await _communityMembershipOperator
                .CreateCommunityMembershipAsync(requestingUser,
                    communityIdentifier);
        }

        public async Task DeleteCommunityMembershipAsync(User requestingUser,
            string communityEncodedIdOrName)
        {
            var communityMembershipIdentifier = await _communityMembershipValidator
                .ValidateDeleteCommunityMembershipInputParametersAsync(
                    requestingUser, communityEncodedIdOrName);
            await _communityMembershipOperator
                .DeleteCommunityMembershipByIdentifierAsync(communityMembershipIdentifier);
        }
    }
}
