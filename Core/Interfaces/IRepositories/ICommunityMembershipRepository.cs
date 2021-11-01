using FireplaceApi.Core.Models;
using FireplaceApi.Core.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Interfaces
{
    public interface ICommunityMembershipRepository
    {
        public Task<List<CommunityMembership>> ListCommunityMembershipsAsync(List<long> Ids);
        public Task<List<CommunityMembership>> ListCommunityMembershipsAsync(long userId);
        public Task<List<long>> ListCommunityMembershipIdsAsync(long userId);
        public Task<CommunityMembership> GetCommunityMembershipByIdAsync(long id,
            bool includeUser = false, bool includeCommunity = false);
        public Task<CommunityMembership> CreateCommunityMembershipAsync(long userId,
            string username, long communityId, string communityName);
        public Task<CommunityMembership> UpdateCommunityMembershipAsync(
            CommunityMembership communityMembership);
        public Task DeleteCommunityMembershipByIdAsync(long userId,
            Identifier communityIdentifier);
        public Task<bool> DoesCommunityMembershipIdExistAsync(long id);
        public Task<bool> DoesCommunityMembershipExistAsync(long userId, long communityId);
    }
}
