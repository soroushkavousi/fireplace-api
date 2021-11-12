using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Identifiers;
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

        public async Task<CommunityIdentifier> ValidateListPostsInputParametersAsync(User requestingUser,
            PaginationInputParameters paginationInputParameters, bool? self,
            bool? joined, string encodedCommunityId, string communityName,
            string search, SortType? sort, string stringOfSort)
        {
            await _queryResultValidator.ValidatePaginationInputParameters(
                paginationInputParameters, ModelName.POST);

            ValidateInputEnum(sort, stringOfSort, nameof(sort), ErrorName.INPUT_SORT_IS_NOT_VALID);
            var communityIdentifier = await _communityValidator
                .ValidateMultipleIdentifiers(encodedCommunityId, communityName, false);
            return communityIdentifier;
        }

        public async Task ValidateGetPostByIdInputParametersAsync(User requestingUser, string encodedId,
            bool? includeAuthor, bool? includeCommunity)
        {
            var id = ValidateEncodedIdFormat(encodedId, nameof(encodedId)).Value;
            var post = await ValidatePostExistsAsync(id);
        }

        public async Task<CommunityIdentifier> ValidateCreatePostInputParametersAsync(
            User requestingUser, string encodedCommunityId, string communityName, string content)
        {
            ValidatePostContentFormat(content);
            var communityIdentifier = await _communityValidator
                .ValidateMultipleIdentifiers(encodedCommunityId, communityName);
            return communityIdentifier;
        }

        public async Task ValidateVotePostInputParametersAsync(User requestingUser,
            string encodedId, bool? isUpvote)
        {
            ValidateParameterIsNotMissing(isUpvote, nameof(isUpvote), ErrorName.IS_UPVOTE_IS_MISSING);
            var id = ValidateEncodedIdFormat(encodedId, nameof(encodedId)).Value;
            var post = await ValidatePostExistsAsync(id, requestingUser);
            ValidatePostIsNotVotedByUser(post, requestingUser);
        }

        public async Task ValidateToggleVoteForPostInputParametersAsync(User requestingUser,
            string encodedId)
        {
            var id = ValidateEncodedIdFormat(encodedId, nameof(encodedId)).Value;
            var post = await ValidatePostExistsAsync(id, requestingUser);
            ValidatePostVoteExists(post, requestingUser);
        }

        public async Task ValidateDeleteVoteForPostInputParametersAsync(User requestingUser,
            string encodedId)
        {
            var id = ValidateEncodedIdFormat(encodedId, nameof(encodedId)).Value;
            var post = await ValidatePostExistsAsync(id, requestingUser);
            ValidatePostVoteExists(post, requestingUser);
        }

        public async Task ValidatePatchPostByIdInputParametersAsync(User requestingUser,
            string encodedId, string content)
        {
            var id = ValidateEncodedIdFormat(encodedId, nameof(encodedId)).Value;
            ValidatePostContentFormat(content);
            var post = await ValidatePostExistsAsync(id);
            ValidateRequestingUserCanAlterPost(requestingUser, post);
        }

        public async Task ValidateDeletePostByIdInputParametersAsync(User requestingUser,
            string encodedId)
        {
            var id = ValidateEncodedIdFormat(encodedId, nameof(encodedId)).Value;
            var post = await ValidatePostExistsAsync(id);
            ValidateRequestingUserCanAlterPost(requestingUser, post);
        }

        public async Task<Post> ValidatePostExistsAsync(ulong id, User requestingUser = null)
        {
            var post = await _postOperator.GetPostByIdAsync(id, true, true,
                requestingUser);
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

        public void ValidateRequestingUserCanAlterPost(User requestingUser,
            Post post)
        {
            if (requestingUser.Id != post.AuthorId)
            {
                var serverMessage = $"requestingUser {requestingUser.Id} can't alter " +
                    $"post {post.Id}";
                throw new ApiException(ErrorName.USER_CAN_NOT_ALTER_POST,
                    serverMessage);
            }
        }

        public void ValidatePostIsNotVotedByUser(Post post, User requestingUser)
        {
            if (post.RequestingUserVote != 0)
            {
                var serverMessage = $"Post id {post.Id} is already voted by user {requestingUser.Username}!";
                throw new ApiException(ErrorName.POST_VOTE_ALREADY_EXISTS, serverMessage);
            }
        }

        public void ValidatePostVoteExists(Post post, User requestingUser)
        {
            if (post.RequestingUserVote == 0)
            {
                var serverMessage = $"Post id {post.Id} is not voted by user {requestingUser.Username}!";
                throw new ApiException(ErrorName.POST_VOTE_DOES_NOT_EXIST, serverMessage);
            }
        }
    }
}
