using FireplaceApi.Core.Models;
using FireplaceApi.Core.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Interfaces
{
    public interface ICommunityMembershipRepository
    {
        public Task<List<CommunityMembership>> ListCommunityMembershipsAsync(List<ulong> Ids);
        public Task<List<CommunityMembership>> ListCommunityMembershipsAsync(ulong userId);
        public Task<List<ulong>> ListCommunityMembershipIdsAsync(ulong userId);
        public Task<CommunityMembership> GetCommunityMembershipByIdAsync(ulong id,
            bool includeUser = false, bool includeCommunity = false);
        public Task<CommunityMembership> CreateCommunityMembershipAsync(ulong id,
            ulong userId, string username, ulong communityId, string communityName);
        public Task<CommunityMembership> UpdateCommunityMembershipAsync(
            CommunityMembership communityMembership);
        public Task DeleteCommunityMembershipByIdAsync(ulong userId,
            Identifier communityIdentifier);
        public Task<bool> DoesCommunityMembershipIdExistAsync(ulong id);
        public Task<bool> DoesCommunityMembershipExistAsync(ulong userId, ulong communityId);
    }
}
