using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Operators;
using FireplaceApi.Domain.Validators;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Services
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
            await _communityMembershipValidator.ValidateCreateCommunityMembershipInputParametersAsync(
                requestingUser, encodedCommunityId, communityName);
            return await _communityMembershipOperator.CreateCommunityMembershipAsync(requestingUser,
                    _communityMembershipValidator.CommunityIdentitifer);
        }

        public async Task DeleteCommunityMembershipAsync(User requestingUser,
            string communityEncodedIdOrName)
        {
            await _communityMembershipValidator.ValidateDeleteCommunityMembershipInputParametersAsync(
                    requestingUser, communityEncodedIdOrName);
            await _communityMembershipOperator.DeleteCommunityMembershipByIdentifierAsync(
                _communityMembershipValidator.CommunityMembershipIdentifier);
        }
    }
}
