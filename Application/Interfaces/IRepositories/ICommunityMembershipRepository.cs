﻿using FireplaceApi.Application.Identifiers;
using FireplaceApi.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Interfaces;

public interface ICommunityMembershipRepository
{
    public Task<List<CommunityMembership>> SearchCommunityMembershipsAsync(
        UserIdentifier userIdentifier = null, CommunityIdentifier communityIdentifier = null,
        bool includeUser = false, bool includeCommunity = false);
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
