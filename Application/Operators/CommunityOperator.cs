using FireplaceApi.Application.Enums;
using FireplaceApi.Application.Extensions;
using FireplaceApi.Application.Identifiers;
using FireplaceApi.Application.Interfaces;
using FireplaceApi.Application.Models;
using FireplaceApi.Application.Tools;
using FireplaceApi.Application.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Operators;

public class CommunityOperator
{
    private readonly ILogger<CommunityOperator> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly ICommunityRepository _communityRepository;
    private readonly UserOperator _userOperator;

    public CommunityOperator(ILogger<CommunityOperator> logger,
        IServiceProvider serviceProvider, ICommunityRepository communityRepository,
        UserOperator userOperator)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _communityRepository = communityRepository;
        _userOperator = userOperator;
    }

    public async Task<QueryResult<Community>> ListCommunitiesAsync(string search, CommunitySortType? sort = null)
    {
        sort ??= default;
        var communities = await _communityRepository.ListCommunitiesAsync(search, sort.Value);
        var queryResult = new QueryResult<Community>(communities);
        return queryResult;
    }

    public async Task<QueryResult<Community>> ListJoinedCommunitiesAsync(ulong userId, CommunitySortType? sort = null)
    {
        sort ??= default;
        var communityMembershipOperator = _serviceProvider.GetService<CommunityMembershipOperator>();
        var communityMemberships = await communityMembershipOperator.SearchCommunityMembershipsAsync(
            userIdentifier: UserIdentifier.OfId(userId), includeCommunity: true);
        var communities = communityMemberships.Select(cm => cm.Community).ToList();
        var queryResult = new QueryResult<Community>(communities);
        return queryResult;
    }

    public async Task<List<Community>> ListCommunitiesByIdsAsync(List<ulong> ids)
    {
        if (ids.IsNullOrEmpty())
            return null;

        var communities = await _communityRepository
            .ListCommunitiesByIdsAsync(ids);
        return communities;
    }

    public async Task<Community> GetCommunityByIdentifierAsync(
        CommunityIdentifier identifier, bool includeCreator)
    {
        var community = await _communityRepository.GetCommunityByIdentifierAsync(
            identifier, includeCreator);
        if (community == null)
            return community;

        return community;
    }

    public async Task<string> GetNameByIdAsync(ulong id)
    {
        var communityName = await _communityRepository.GetNameByIdAsync(id);
        return communityName;
    }

    public async Task<ulong> GetIdByNameAsync(string name)
    {
        var communityId = await _communityRepository.GetIdByNameAsync(name);
        return communityId;
    }

    public async Task<Community> CreateCommunityAsync(ulong userId, string name,
        string username = null)
    {
        var id = await IdGenerator.GenerateNewIdAsync(
            id => DoesCommunityIdentifierExistAsync(CommunityIdentifier.OfId(id)));
        username ??= await _userOperator.GetUsernameByIdAsync(userId);
        var community = await _communityRepository.CreateCommunityAsync(
            id, name, userId, username);
        var communityMembershipOperator = _serviceProvider.GetService<CommunityMembershipOperator>();
        var membership = await communityMembershipOperator
            .CreateCommunityMembershipAsync(userId, CommunityIdentifier.OfId(community.Id));
        return community;
    }

    public async Task<Community> PatchCommunityByIdentifierAsync(CommunityIdentifier identifier,
        string name = null)
    {
        var community = await _communityRepository.GetCommunityByIdentifierAsync(identifier);
        return await PatchCommunityByIdentifierAsync(community, name);
    }

    public async Task<Community> PatchCommunityByIdentifierAsync(Community community,
        string name = null)
    {
        community = await ApplyCommunityChangesAsync(community, name);
        community = await GetCommunityByIdentifierAsync(
            CommunityIdentifier.OfId(community.Id), false);
        return community;
    }

    public async Task DeleteCommunityByIdentifierAsync(CommunityIdentifier identifier)
    {
        await _communityRepository.DeleteCommunityByIdentifierAsync(identifier);
    }

    public async Task<bool> DoesCommunityIdentifierExistAsync(CommunityIdentifier identifier)
    {
        var communityIdExists = await _communityRepository
            .DoesCommunityIdentifierExistAsync(identifier);
        return communityIdExists;
    }

    public async Task<Community> ApplyCommunityChangesAsync(Community community, string name = null)
    {
        var foundAnyChange = false;
        if (name != null)
        {
            community.Name = name;
            await _communityRepository.UpdateCommunityNameAsync(community.Id, name);
        }

        if (foundAnyChange)
            community = await _communityRepository.UpdateCommunityAsync(community);
        return community;
    }
}
