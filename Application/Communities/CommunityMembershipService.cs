using FireplaceApi.Domain.Communities;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Communities;

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
        ulong userId, CommunityIdentifier communityIdentifier)
    {
        await _communityMembershipValidator.ValidateCreateCommunityMembershipInputParametersAsync(
            userId, communityIdentifier);
        return await _communityMembershipOperator.CreateCommunityMembershipAsync(userId,
            communityIdentifier);
    }

    public async Task DeleteCommunityMembershipAsync(ulong userId,
        CommunityIdentifier communityIdentifier)
    {
        await _communityMembershipValidator.ValidateDeleteCommunityMembershipInputParametersAsync(
                userId, communityIdentifier);
        await _communityMembershipOperator.DeleteCommunityMembershipByIdentifierAsync(
            _communityMembershipValidator.CommunityMembershipIdentifier);
    }
}
