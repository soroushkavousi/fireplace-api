using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using FireplaceApi.Core.Enums;
using System;
using System.Threading.Tasks;
using FireplaceApi.Core.ValueObjects;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Interfaces;

namespace FireplaceApi.Core.Operators
{
    public class CommunityMembershipOperator
    {
        private readonly ILogger<CommunityMembershipOperator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly ICommunityMembershipRepository _communityMembershipRepository;
        private readonly PageOperator _pageOperator;
        private readonly UserOperator _userOperator;
        private readonly CommunityOperator _communityOperator;

        public CommunityMembershipOperator(ILogger<CommunityMembershipOperator> logger, 
            IConfiguration configuration, IServiceProvider serviceProvider, 
            ICommunityMembershipRepository communityMembershipRepository, 
            PageOperator pageOperator, UserOperator userOperator,
            CommunityOperator communityOperator)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _communityMembershipRepository = communityMembershipRepository;
            _pageOperator = pageOperator;
            _userOperator = userOperator;
            _communityOperator = communityOperator;
        }

        public async Task<Page<CommunityMembership>> ListCommunityMembershipsAsync(User requesterUser,
            PaginationInputParameters paginationInputParameters)
        {
            Page<CommunityMembership> resultPage = default;
            if (string.IsNullOrWhiteSpace(paginationInputParameters.Pointer))
            {
                var communityMembershipIds = await _communityMembershipRepository
                    .ListCommunityMembershipIdsAsync(requesterUser.Id);
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

        public async Task<CommunityMembership> GetCommunityMembershipByIdAsync(long id, 
            bool includeCreator, bool includeCommunity)
        {
            var communityMembership = await _communityMembershipRepository
                .GetCommunityMembershipByIdAsync(id, includeCreator, includeCommunity);
            if (communityMembership == null)
                return communityMembership;

            return communityMembership;
        }

        public async Task<CommunityMembership> CreateCommunityMembershipAsync(User requesterUser, 
            Identifier communityIdentifier)
        {
            switch (communityIdentifier.State)
            {
                case IdentifierState.HasId:
                    communityIdentifier.Name = await _communityOperator
                        .GetNameByIdAsync(communityIdentifier.Id.Value);
                    break;
                case IdentifierState.HasName:
                    communityIdentifier.Id = await _communityOperator
                        .GetIdByNameAsync(communityIdentifier.Name);
                    break;
            }
            var communityMembership = await _communityMembershipRepository
                .CreateCommunityMembershipAsync(requesterUser.Id,
                    requesterUser.Username, communityIdentifier.Id.Value, communityIdentifier.Name);
            return communityMembership;
        }

        public async Task<CommunityMembership> PatchCommunityMembershipByIdAsync(long id)
        {
            var communityMembership = await _communityMembershipRepository
                .GetCommunityMembershipByIdAsync(id);
            communityMembership = await ApplyCommunityMembershipChangesAsync(communityMembership);
            communityMembership = await GetCommunityMembershipByIdAsync(communityMembership.Id, 
                false, false);
            return communityMembership;
        }

        public async Task DeleteCommunityMembershipByIdAsync(long id)
        {
            await _communityMembershipRepository.DeleteCommunityMembershipByIdAsync(id);
        }

        public async Task<bool> DoesCommunityMembershipIdExistAsync(long id)
        {
            var communityMembershipIdExists = await _communityMembershipRepository
                .DoesCommunityMembershipIdExistAsync(id);
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
