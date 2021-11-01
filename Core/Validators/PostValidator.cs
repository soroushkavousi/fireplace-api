using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
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
        private readonly CommunityValidator _communityValidator;

        public PostValidator(ILogger<PostValidator> logger, IConfiguration configuration,
            IServiceProvider serviceProvider, PostOperator postOperator,
            QueryResultValidator queryResultValidator,
            CommunityValidator communityValidator)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _postOperator = postOperator;
            _queryResultValidator = queryResultValidator;
            _communityValidator = communityValidator;
        }

        public async Task ValidateListPostsInputParametersAsync(User requesterUser,
            PaginationInputParameters paginationInputParameters, bool? self,
            bool? joined, long? communityId, string communityName,
            string search, SortType? sort, string stringOfSort)
        {
            await _queryResultValidator.ValidatePaginationInputParameters(
                paginationInputParameters, ModelName.POST);

            ValidateInputEnum(sort, stringOfSort, nameof(sort), ErrorName.INPUT_SORT_IS_NOT_VALID);
            Community community;
            if (communityId.HasValue || !string.IsNullOrWhiteSpace(communityName))
                community = await _communityValidator.ValidateCommunityExistsAsync(
                    communityId, communityName);
        }

        public async Task ValidateGetPostByIdInputParametersAsync(User requesterUser, long id,
            bool? includeAuthor, bool? includeCommunity)
        {
            var post = await ValidatePostExistsAsync(id);
        }

        public async Task ValidateCreatePostInputParametersAsync(User requesterUser,
            long? communityId, string communityName, string content)
        {
            ValidatePostContentFormat(content);
            var community = await _communityValidator.ValidateCommunityExistsAsync(
                communityId, communityName);
        }

        public async Task ValidateVotePostInputParametersAsync(User requesterUser,
            long id, bool? isUpvote)
        {
            var post = await ValidatePostExistsAsync(id);
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
            long id, string content)
        {
            ValidatePostContentFormat(content);
            var post = await ValidatePostExistsAsync(id);
            ValidateRequesterUserCanAlterPost(requesterUser, post);
        }

        public async Task ValidateDeletePostByIdInputParametersAsync(User requesterUser,
            long id)
        {
            var post = await ValidatePostExistsAsync(id);
            ValidateRequesterUserCanAlterPost(requesterUser, post);
        }

        public async Task<Post> ValidatePostExistsAsync(long id)
        {
            var post = await _postOperator.GetPostByIdAsync(id, true, true);
            if (post == null)
            {
                var serverMessage = $"Post id {id} doesn't exist!";
                throw new ApiException(ErrorName.POST_DOES_NOT_EXIST, serverMessage);
            }
            return post;
        }

        public void ValidatePostContentFormat(string content)
        {
            var maximumLength = 2000;
            if (content.Length > maximumLength)
            {
                var serverMessage = $"POST content exceeds the maximum length! "
                    + new { maximumLength, ContentLength = content.Length }.ToJson();
                throw new ApiException(ErrorName.POST_CONTENT_MAX_LENGTH, serverMessage);
            }
        }

        public void ValidateRequesterUserCanAlterPost(User requesterUser,
            Post post)
        {
            if (requesterUser.Id != post.AuthorId)
            {
                var serverMessage = $"requesterUser {requesterUser.Id} can't alter " +
                    $"post {post.Id}";
                throw new ApiException(ErrorName.USER_CAN_NOT_ALTER_POST,
                    serverMessage);
            }
        }
    }
}
