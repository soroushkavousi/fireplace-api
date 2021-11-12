using FireplaceApi.Core.Identifiers;
using FireplaceApi.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Interfaces
{
    public interface ICommunityMembershipRepository
    {
        public Task<List<CommunityMembership>> ListCommunityMembershipsAsync(List<ulong> Ids);
        public Task<List<CommunityMembership>> ListCommunityMembershipsAsync(UserIdentifier userIdentifier);
        public Task<List<ulong>> ListCommunityMembershipIdsAsync(UserIdentifier userIdentifier);
        public Task<CommunityMembership> GetCommunityMembershipByIdentifierAsync(
            CommunityMembershipIdentifier identifier, bool includeUser = false,
            bool includeCommunity = false);
        public Task<CommunityMembership> CreateCommunityMembershipAsync(ulong id,
            ulong userId, string username, ulong communityId, string communityName);
        public Task<CommunityMembership> UpdateCommunityMembershipAsync(
            CommunityMembership communityMembership);
        public Task DeleteCommunityMembershipByIdentifierAsync(CommunityMembershipIdentifier identifier);
        public Task<bool> DoesCommunityMembershipIdentifierExistAsync(CommunityMembershipIdentifier identifier);
    }
}
