using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Identifiers;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Validators
{
    public class PostValidator : ApiValidator
    {
        private readonly ILogger<PostValidator> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly PostOperator _postOperator;
        private readonly CommunityValidator _communityValidator;

        public CommunityIdentifier CommunityIdentifier { get; set; }
        public List<ulong> Ids { get; private set; }
        public SortType? Sort { get; private set; }

        public PostValidator(ILogger<PostValidator> logger,
            IServiceProvider serviceProvider, PostOperator postOperator,
            CommunityValidator communityValidator)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _postOperator = postOperator;
            _communityValidator = communityValidator;
        }

        public async Task ValidateListCommunityPostsInputParametersAsync(string communityEncodedIdOrName,
            string sort, User requestingUser)
        {
            Sort = ValidateInputEnum<SortType>(sort, nameof(sort), ErrorName.INPUT_SORT_IS_NOT_VALID);
            CommunityIdentifier = await _communityValidator
                .ValidateMultipleIdentifiers(communityEncodedIdOrName, communityEncodedIdOrName);
        }

        public async Task ValidateListPostsInputParametersAsync(string search, string sort, User requestingUser)
        {
            Sort = ValidateInputEnum<SortType>(sort, nameof(sort), ErrorName.INPUT_SORT_IS_NOT_VALID);
            await Task.CompletedTask;
        }

        public async Task ValidateListPostsByIdsInputParametersAsync(string encodedIds, User requestingUser)
        {
            Ids = ValidateIdsFormat(encodedIds);
            await Task.CompletedTask;
        }

        public async Task ValidateListSelfPostsInputParametersAsync(User requestingUser,
            string sort)
        {
            Sort = ValidateInputEnum<SortType>(sort, nameof(sort), ErrorName.INPUT_SORT_IS_NOT_VALID);
            await Task.CompletedTask;
        }

        public async Task ValidateGetPostByIdInputParametersAsync(User requestingUser, string encodedId,
            bool? includeAuthor, bool? includeCommunity)
        {
            var id = ValidateEncodedIdFormat(encodedId, nameof(encodedId)).Value;
            var post = await ValidatePostExistsAsync(id);
        }

        public async Task ValidateCreatePostInputParametersAsync(
            User requestingUser, string communityEncodedIdOrName, string content)
        {
            ValidatePostContentFormat(content);
            CommunityIdentifier = await _communityValidator
                .ValidateMultipleIdentifiers(communityEncodedIdOrName, communityEncodedIdOrName);
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
