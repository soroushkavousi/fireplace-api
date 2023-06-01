using FireplaceApi.Domain.Identifiers;
using FireplaceApi.Domain.Interfaces;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Tools;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Operators;

public class CommunityMembershipOperator
{
    private readonly ILogger<CommunityMembershipOperator> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly ICommunityMembershipRepository _communityMembershipRepository;
    private readonly UserOperator _userOperator;
    private readonly CommunityOperator _communityOperator;

    public CommunityMembershipOperator(ILogger<CommunityMembershipOperator> logger,
        IServiceProvider serviceProvider, ICommunityMembershipRepository communityMembershipRepository,
        UserOperator userOperator, CommunityOperator communityOperator)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _communityMembershipRepository = communityMembershipRepository;
        _userOperator = userOperator;
        _communityOperator = communityOperator;
    }

    public async Task<List<CommunityMembership>> SearchCommunityMembershipsAsync(
        UserIdentifier userIdentifier = null, CommunityIdentifier communityIdentifier = null,
        bool includeUser = false, bool includeCommunity = false)
    {
        var communityMemberships = await _communityMembershipRepository
            .SearchCommunityMembershipsAsync(userIdentifier, communityIdentifier,
                includeUser, includeCommunity);

        return communityMemberships;
    }

    public async Task<CommunityMembership> GetCommunityMembershipByIdentifierAsync(
        CommunityMembershipIdentifier identifier, bool includeCreator, bool includeCommunity)
    {
        var communityMembership = await _communityMembershipRepository
            .GetCommunityMembershipByIdentifierAsync(identifier, includeCreator, includeCommunity);
        if (communityMembership == null)
            return communityMembership;

        return communityMembership;
    }

    public async Task<CommunityMembership> CreateCommunityMembershipAsync(User requestingUser,
        CommunityIdentifier communityIdentifier)
    {
        ulong communityId = default;
        string communityName = default;
        switch (communityIdentifier)
        {
            case CommunityIdIdentifier idIdentifier:
                communityId = idIdentifier.Id;
                communityName = await _communityOperator
                    .GetNameByIdAsync(communityId);
                break;
            case CommunityNameIdentifier nameIdentifier:
                communityName = nameIdentifier.Name;
                communityId = await _communityOperator
                    .GetIdByNameAsync(communityName);
                break;
        }

        var id = await IdGenerator.GenerateNewIdAsync(
            id => DoesCommunityMembershipIdentifierExistAsync(CommunityMembershipIdentifier.OfId(id)));
        var communityMembership = await _communityMembershipRepository
            .CreateCommunityMembershipAsync(id, requestingUser.Id,
                requestingUser.Username, communityId, communityName);
        return communityMembership;
    }

    public async Task<CommunityMembership> PatchCommunityMembershipByIdentifierAsync(
        CommunityMembershipIdentifier identifier)
    {
        var communityMembership = await _communityMembershipRepository
            .GetCommunityMembershipByIdentifierAsync(identifier);
        communityMembership = await ApplyCommunityMembershipChangesAsync(communityMembership);
        communityMembership = await GetCommunityMembershipByIdentifierAsync(
            CommunityMembershipIdentifier.OfId(communityMembership.Id),
            false, false);
        return communityMembership;
    }

    public async Task DeleteCommunityMembershipByIdentifierAsync(CommunityMembershipIdentifier identifier)
    {
        await _communityMembershipRepository.DeleteCommunityMembershipByIdentifierAsync(
            identifier);
    }

    public async Task<bool> DoesCommunityMembershipIdentifierExistAsync(
        CommunityMembershipIdentifier identifier)
    {
        var communityMembershipIdExists = await _communityMembershipRepository
            .DoesCommunityMembershipIdentifierExistAsync(identifier);
        return communityMembershipIdExists;
    }

    public async Task<CommunityMembership> ApplyCommunityMembershipChangesAsync(
        CommunityMembership communityMembership)
    {
        communityMembership = await _communityMembershipRepository
            .UpdateCommunityMembershipAsync(communityMembership);
        return communityMembership;
    }
}
