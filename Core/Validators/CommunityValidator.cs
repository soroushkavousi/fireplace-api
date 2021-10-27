using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.ValueObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Validators
{
    public class CommunityValidator : ApiValidator
    {
        private readonly ILogger<CommunityValidator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly CommunityOperator _communityOperator;
        private readonly QueryResultValidator _queryResultValidator;


        public CommunityValidator(ILogger<CommunityValidator> logger, IConfiguration configuration,
            IServiceProvider serviceProvider, CommunityOperator communityOperator,
            QueryResultValidator queryResultValidator)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _communityOperator = communityOperator;
            _queryResultValidator = queryResultValidator;
        }

        public async Task ValidateListCommunitiesInputParametersAsync(User requesterUser,
            PaginationInputParameters paginationInputParameters, string name)
        {
            await _queryResultValidator.ValidatePaginationInputParameters(paginationInputParameters,
                ModelName.COMMUNITY);
        }

        public async Task ValidateGetCommunityByIdInputParametersAsync(User requesterUser, long? id,
            bool? includeCreator)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateGetCommunityByNameInputParametersAsync(User requesterUser, string name,
            bool? includeCreator)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateCreateCommunityInputParametersAsync(string name, long creatorId)
        {
            await Task.CompletedTask;
        }

        public async Task ValidatePatchCommunityByIdInputParametersAsync(User requesterUser, long? id)
        {
            await Task.CompletedTask;
        }

        public async Task ValidatePatchCommunityByNameInputParametersAsync(User requesterUser, string name)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateDeleteCommunityByIdInputParametersAsync(User requesterUser, long? id)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateDeleteCommunityByNameInputParametersAsync(User requesterUser, string name)
        {
            await Task.CompletedTask;
        }
    }
}
