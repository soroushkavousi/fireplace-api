using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Exceptions;
using FireplaceApi.Domain.Identifiers;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Operators;
using FireplaceApi.Domain.Tools;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Validators;

public class CommunityValidator : DomainValidator
{
    private readonly ILogger<CommunityValidator> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly CommunityOperator _communityOperator;

    public Community Community { get; private set; }

    public CommunityValidator(ILogger<CommunityValidator> logger,
        IServiceProvider serviceProvider, CommunityOperator communityOperator)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _communityOperator = communityOperator;
    }

    public async Task ValidateListCommunitiesInputParametersAsync(string search, CommunitySortType? sort)
    {
        await Task.CompletedTask;
    }

    public async Task ValidateListJoinedCommunitiesInputParametersAsync(User requestingUser, CommunitySortType? sort)
    {
        await Task.CompletedTask;
    }

    public async Task ValidateListCommunitiesByIdsInputParametersAsync(List<ulong> ids)
    {
        await Task.CompletedTask;
    }

    public async Task ValidateGetCommunityByIdentifierInputParametersAsync(
        CommunityIdentifier identifier)
    {
        await ValidateCommunityIdentifierExistsAsync(identifier);
    }

    public async Task ValidateCreateCommunityInputParametersAsync(User requestingUser, string name)
    {
        await ValidateCommunityIdentifierDoesNotExistAsync(CommunityIdentifier.OfName(name));
    }

    public async Task ValidatePatchCommunityByIdentifierInputParametersAsync(User requestingUser,
        CommunityIdentifier identifier, string newName)
    {
        await ValidateCommunityIdentifierExistsAsync(identifier);
        Community = await _communityOperator.GetCommunityByIdentifierAsync(
            identifier, false);
        ValidateRequestingUserCanAlterCommunity(requestingUser, Community);
        await ValidateCommunityIdentifierDoesNotExistAsync(CommunityIdentifier.OfName(newName));
    }

    public async Task ValidateDeleteCommunityByIdentifierInputParametersAsync(User requestingUser,
        CommunityIdentifier identifier)
    {
        await ValidateCommunityIdentifierExistsAsync(identifier);
        Community = await _communityOperator.GetCommunityByIdentifierAsync(
            identifier, false);
        ValidateRequestingUserCanAlterCommunity(requestingUser, Community);
    }

    public bool ValidateCommunityNameFormat(string communityName, bool throwException = true)
    {
        if (Regexes.CommunityNameMinLength.IsMatch(communityName) == false)
            return throwException ? throw new CommunityNameInvalidFormatException(communityName,
                "The community search doesn't have the minimum length!") : false;

        if (Regexes.CommunityNameMaxLength.IsMatch(communityName) == false)
            return throwException ? throw new CommunityNameInvalidFormatException(communityName,
                "The community search has more characters than maximum length!") : false;

        if (Regexes.CommunityNameValidCharacters.IsMatch(communityName) == false)
            return throwException ? throw new CommunityNameInvalidFormatException(communityName,
                "The community search doesn't have valid characters!") : false;

        return true;
    }

    public async Task<bool> ValidateCommunityIdentifierExistsAsync(
        CommunityIdentifier identifier, bool throwException = true)
    {
        if (await _communityOperator.DoesCommunityIdentifierExistAsync(identifier))
            return true;

        if (throwException)
            throw new CommunityNotExistException(identifier);

        return false;
    }

    public async Task<Community> ValidateCommunityIdentifierExistsByGettingAsync(
        CommunityIdentifier identifier)
    {
        var community = await _communityOperator.GetCommunityByIdentifierAsync(identifier, false);
        if (community == null)
            throw new CommunityNotExistException(identifier);
        return community;
    }

    public async Task<bool> ValidateCommunityIdentifierDoesNotExistAsync(
        CommunityIdentifier identifier, bool throwException = true)
    {
        if (await _communityOperator.DoesCommunityIdentifierExistAsync(identifier) == false)
            return true;

        if (throwException)
            throw new CommunityAlreadyExistsException(identifier);

        return false;
    }

    public void ValidateRequestingUserCanAlterCommunity(User requestingUser,
        Community community)
    {
        if (requestingUser.Id != community.CreatorId)
            throw new CommunityAccessDeniedException(requestingUser.Id, community.Id);
    }
}
