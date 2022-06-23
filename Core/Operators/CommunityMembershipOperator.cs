using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Identifiers;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Tools;
using FireplaceApi.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Operators
{
    public class CommunityMembershipOperator
    {
        private readonly ILogger<CommunityMembershipOperator> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly ICommunityMembershipRepository _communityMembershipRepository;
        private readonly PageOperator _pageOperator;
        private readonly UserOperator _userOperator;
        private readonly CommunityOperator _communityOperator;

        public CommunityMembershipOperator(ILogger<CommunityMembershipOperator> logger,
            IServiceProvider serviceProvider, ICommunityMembershipRepository communityMembershipRepository,
            PageOperator pageOperator, UserOperator userOperator, CommunityOperator communityOperator)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _communityMembershipRepository = communityMembershipRepository;
            _pageOperator = pageOperator;
            _userOperator = userOperator;
            _communityOperator = communityOperator;
        }

        public async Task<Page<CommunityMembership>> ListCommunityMembershipsAsync(User requestingUser,
            PaginationInputParameters paginationInputParameters)
        {
            Page<CommunityMembership> resultPage = default;
            if (string.IsNullOrWhiteSpace(paginationInputParameters.Pointer))
            {
                var communityMembershipIds = await _communityMembershipRepository
                    .ListCommunityMembershipIdsAsync(UserIdentifier.OfId(requestingUser.Id));
                resultPage = await _pageOperator.CreatePageWithoutPointerAsync(
                    ModelName.COMMUNITY_MEMBERSHIP, paginationInputParameters, communityMembershipIds,
                    _communityMembershipRepository.ListCommunityMembershipsAsync);
            }
            else
            {
                resultPage = await _pageOperator.CreatePageWithPointerAsync(
                    ModelName.COMMUNITY_MEMBERSHIP, paginationInputParameters,
                    _communityMembershipRepository.ListCommunityMembershipsAsync);
            }
            return resultPage;
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
}
