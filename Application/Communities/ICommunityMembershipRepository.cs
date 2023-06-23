using FireplaceApi.Domain.Communities;
using FireplaceApi.Domain.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Communities;

public interface ICommunityMembershipRepository
{
    public Task<List<CommunityMembership>> SearchCommunityMembershipsAsync(
        UserIdentifier userIdentifier = null, CommunityIdentifier communityIdentifier = null,
        bool includeUser = false, bool includeCommunity = false);
    public Task<CommunityMembership> GetCommunityMembershipByIdentifierAsync(
        CommunityMembershipIdentifier identifier, bool includeUser = false,
        bool includeCommunity = false);
    public Task<CommunityMembership> CreateCommunityMembershipAsync(ulong id,
        ulong userId, Username username, ulong communityId, string communityName);
    public Task<CommunityMembership> UpdateCommunityMembershipAsync(
        CommunityMembership communityMembership);
    public Task DeleteCommunityMembershipByIdentifierAsync(CommunityMembershipIdentifier identifier);
    public Task<bool> DoesCommunityMembershipIdentifierExistAsync(CommunityMembershipIdentifier identifier);
}
