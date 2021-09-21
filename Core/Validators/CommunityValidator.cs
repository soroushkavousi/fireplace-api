using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Tools;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.ValueObjects;

namespace FireplaceApi.Core.Validators
{
    public class CommunityValidator : ApiValidator
    {
        private readonly ILogger<CommunityValidator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly CommunityOperator _communityOperator;


        public CommunityValidator(ILogger<CommunityValidator> logger, IConfiguration configuration,
            IServiceProvider serviceProvider, CommunityOperator communityOperator)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _communityOperator = communityOperator;
        }

        public async Task ValidateListCommunitiesInputParametersAsync(User requesterUser)
        {
            await Task.CompletedTask;
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
