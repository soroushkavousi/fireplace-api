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
    public class PostValidator : ApiValidator
    {
        private readonly ILogger<PostValidator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly PostOperator _postOperator;
        private readonly QueryResultValidator _queryResultValidator;

        public PostValidator(ILogger<PostValidator> logger, IConfiguration configuration,
            IServiceProvider serviceProvider, PostOperator postOperator,
            QueryResultValidator queryResultValidator)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _postOperator = postOperator;
            _queryResultValidator = queryResultValidator;
        }

        public async Task ValidateListPostsInputParametersAsync(User requesterUser,
            PaginationInputParameters paginationInputParameters, bool? self,
            bool? joined, long? communityId, string communityName,
            string search, SortType? sort, string stringOfSort)
        {
            await _queryResultValidator.ValidatePaginationInputParameters(
                paginationInputParameters, ModelName.POST);

            ValidateInputEnum(sort, stringOfSort, nameof(sort), ErrorName.INPUT_SORT_IS_NOT_VALID);
        }

        public async Task ValidateGetPostByIdInputParametersAsync(User requesterUser, long? id,
            bool? includeAuthor, bool? includeCommunity)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateCreatePostInputParametersAsync(User requesterUser,
            long? communityId, string communityName, string content)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateVotePostInputParametersAsync(User requesterUser,
            long id, bool? isUpvote)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateToggleVoteForPostInputParametersAsync(User requesterUser,
            long id)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateDeleteVoteForPostInputParametersAsync(User requesterUser,
            long id)
        {
            await Task.CompletedTask;
        }
        public async Task ValidatePatchPostByIdInputParametersAsync(User requesterUser,
            long? id, string content)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateDeletePostByIdInputParametersAsync(User requesterUser, long? id)
        {
            await Task.CompletedTask;
        }
    }
}
