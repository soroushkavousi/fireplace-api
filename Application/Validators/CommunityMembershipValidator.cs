using FireplaceApi.Application.Exceptions;
using FireplaceApi.Application.Identifiers;
using FireplaceApi.Application.Models;
using FireplaceApi.Application.Operators;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Validators;

public class CommunityMembershipValidator : ApplicationValidator
{
    private readonly ILogger<CommunityMembershipValidator> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly CommunityMembershipOperator _communityMembershipOperator;
    private readonly CommunityValidator _communityValidator;

    public CommunityMembershipIdentifier CommunityMembershipIdentifier { get; private set; }
    public UserIdentifier UserIdentifier { get; private set; }

    public CommunityMembershipValidator(ILogger<CommunityMembershipValidator> logger,
        IServiceProvider serviceProvider, CommunityMembershipOperator communityMembershipOperator,
        CommunityValidator communityValidator)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _communityMembershipOperator = communityMembershipOperator;
        _communityValidator = communityValidator;
    }

    public async Task ValidateCreateCommunityMembershipInputParametersAsync(ulong userId,
        CommunityIdentifier communityIdentifier)
    {
        await _communityValidator.ValidateCommunityIdentifierExistsAsync(communityIdentifier);
        UserIdentifier = UserIdentifier.OfId(userId);
        CommunityMembershipIdentifier = CommunityMembershipIdentifier
            .OfUserAndCommunity(UserIdentifier, communityIdentifier);
        await ValidateCommunityMembershipDoesNotAlreadyExist(CommunityMembershipIdentifier);
    }

    public async Task ValidateDeleteCommunityMembershipInputParametersAsync(ulong userId,
        CommunityIdentifier communityIdentifier)
    {
        var community = await _communityValidator.ValidateCommunityIdentifierExistsByGettingAsync(communityIdentifier);
        ValidateUserIsNotTheOwnerOfTheCommunity(userId, community);
        UserIdentifier = UserIdentifier.OfId(userId);
        CommunityMembershipIdentifier = CommunityMembershipIdentifier
            .OfUserAndCommunity(UserIdentifier, communityIdentifier);
        await ValidateCommunityMembershipAlreadyExists(CommunityMembershipIdentifier);
    }

    public async Task<bool> ValidateCommunityMembershipDoesNotAlreadyExist(
        CommunityMembershipIdentifier identifier, bool throwException = true)
    {
        if (await _communityMembershipOperator
            .DoesCommunityMembershipIdentifierExistAsync(identifier) == false)
            return true;

        if (throwException)
            throw new CommunityMembershipAlreadyExistsException(identifier);

        return false;
    }

    public async Task<bool> ValidateCommunityMembershipAlreadyExists(
        CommunityMembershipIdentifier identifier, bool throwException = true)
    {
        if (await _communityMembershipOperator
            .DoesCommunityMembershipIdentifierExistAsync(identifier))
            return true;

        if (throwException)
            throw new CommunityMembershipNotExistException(identifier);

        return false;
    }

    public void ValidateUserIsNotTheOwnerOfTheCommunity(ulong userId, Community community)
    {
        if (community.CreatorId == userId)
            throw new CommunityMembershipAccessDeniedException(userId, community.Id);

        return;
    }
}
