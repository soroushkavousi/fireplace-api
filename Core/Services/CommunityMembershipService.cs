using Microsoft.Extensions.Logging;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FireplaceApi.Core.Operators;
using System.Net;
using FireplaceApi.Core.ValueObjects;

namespace FireplaceApi.Core.Services
{
    public class CommunityMembershipService
    {
        private readonly ILogger<CommunityMembershipService> _logger;
        private readonly CommunityMembershipValidator _communityMembershipValidator;
        private readonly CommunityMembershipOperator _communityMembershipOperator;
        
        public CommunityMembershipService(ILogger<CommunityMembershipService> logger, 
            CommunityMembershipValidator communityMembershipValidator, CommunityMembershipOperator communityMembershipOperator)
        {
            _logger = logger;
            _communityMembershipValidator = communityMembershipValidator;
            _communityMembershipOperator = communityMembershipOperator;
        }

        public async Task<Page<CommunityMembership>> ListCommunityMembershipsAsync(
            User requesterUser, PaginationInputParameters paginationInputParameters)
        {
            await _communityMembershipValidator.ValidateListCommunityMembershipsInputParametersAsync(requesterUser,
                paginationInputParameters);
            var page = await _communityMembershipOperator.ListCommunityMembershipsAsync(requesterUser,
                paginationInputParameters);
            return page;
        } 

        public async Task<CommunityMembership> GetCommunityMembershipByIdAsync(
            User requesterUser, long? id, bool? includeCreator, bool? includeCommunity)
        {
            await _communityMembershipValidator.ValidateGetCommunityMembershipByIdInputParametersAsync(
                requesterUser, id, includeCreator, includeCommunity);
            var communityMembership = await _communityMembershipOperator.GetCommunityMembershipByIdAsync(id.Value, 
                includeCreator.Value, includeCommunity.Value);
            return communityMembership;
        }

        public async Task<CommunityMembership> CreateCommunityMembershipAsync(
            User requesterUser, long? communityId, string communityName)
        {
            await _communityMembershipValidator
                .ValidateCreateCommunityMembershipInputParametersAsync(
                requesterUser, communityId, communityName);
            return await _communityMembershipOperator
                .CreateCommunityMembershipAsync(requesterUser, 
                    new Identifier(communityId, communityName));
        }

        public async Task<CommunityMembership> PatchCommunityMembershipByIdAsync(
            User requesterUser, long? id)
        {
            await _communityMembershipValidator
                .ValidatePatchCommunityMembershipByIdInputParametersAsync(requesterUser, id);
            var communityMembership = await _communityMembershipOperator
                .PatchCommunityMembershipByIdAsync(id.Value);
            return communityMembership;
        }

        public async Task DeleteCommunityMembershipByIdAsync(User requesterUser, long? id)
        {
            await _communityMembershipValidator
                .ValidateDeleteCommunityMembershipByIdInputParametersAsync(requesterUser, id);
            await _communityMembershipOperator
                .DeleteCommunityMembershipByIdAsync(id.Value);
        }
    }
}
