using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.ValueObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Validators
{
    public class CommunityMembershipValidator : ApiValidator
    {
        private readonly ILogger<CommunityMembershipValidator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly CommunityMembershipOperator _communityMembershipOperator;

        public CommunityMembershipValidator(ILogger<CommunityMembershipValidator> logger, IConfiguration configuration,
            IServiceProvider serviceProvider, CommunityMembershipOperator communityMembershipOperator)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _communityMembershipOperator = communityMembershipOperator;
        }

        public async Task ValidateListCommunityMembershipsInputParametersAsync(User requesterUser,
            PaginationInputParameters paginationInputParameters)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateGetCommunityMembershipByIdInputParametersAsync(User requesterUser, long? id,
            bool? includeCreator, bool? includeCommunity)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateCreateCommunityMembershipInputParametersAsync(User requesterUser,
            long? communityId, string communityName)
        {
            await Task.CompletedTask;
        }

        public async Task ValidatePatchCommunityMembershipByIdInputParametersAsync(User requesterUser, long? id)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateDeleteCommunityMembershipByIdInputParametersAsync(User requesterUser, long? id)
        {
            await Task.CompletedTask;
        }
    }
}
