using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Exceptions;
using FireplaceApi.Domain.Identifiers;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Operators;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Validators
{
    public class PostValidator : DomainValidator
    {
        private readonly ILogger<PostValidator> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly PostOperator _postOperator;
        private readonly CommunityValidator _communityValidator;

        public Post Post { get; private set; }

        public PostValidator(ILogger<PostValidator> logger,
            IServiceProvider serviceProvider, PostOperator postOperator,
            CommunityValidator communityValidator)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _postOperator = postOperator;
            _communityValidator = communityValidator;
        }

        public async Task ValidateListCommunityPostsInputParametersAsync(CommunityIdentifier communityIdentifier,
            SortType? sort, User requestingUser)
        {
            await _communityValidator.ValidateCommunityIdentifierExists(communityIdentifier);
        }

        public async Task ValidateListPostsInputParametersAsync(string search, SortType? sort, User requestingUser)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateListPostsByIdsInputParametersAsync(List<ulong> ids, User requestingUser)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateListSelfPostsInputParametersAsync(User requestingUser,
            SortType? sort)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateGetPostByIdInputParametersAsync(ulong id,
            bool? includeAuthor, bool? includeCommunity, User requestingUser = null)
        {
            Post = await ValidatePostExistsAsync(id);
        }

        public async Task ValidateCreatePostInputParametersAsync(
            User requestingUser, CommunityIdentifier communityIdentifier, string content)
        {
            await _communityValidator.ValidateCommunityIdentifierExists(communityIdentifier);
        }

        public async Task ValidateVotePostInputParametersAsync(User requestingUser,
            ulong id, bool isUpvote)
        {
            Post = await ValidatePostExistsAsync(id, requestingUser);
            ValidatePostIsNotVotedByUser(Post, requestingUser);
        }

        public async Task ValidateToggleVoteForPostInputParametersAsync(User requestingUser,
            ulong id)
        {
            Post = await ValidatePostExistsAsync(id, requestingUser);
            ValidatePostVoteExists(Post, requestingUser);
        }

        public async Task ValidateDeleteVoteForPostInputParametersAsync(User requestingUser,
            ulong id)
        {
            Post = await ValidatePostExistsAsync(id, requestingUser);
            ValidatePostVoteExists(Post, requestingUser);
        }

        public async Task ValidatePatchPostByIdInputParametersAsync(User requestingUser,
            ulong id, string content)
        {
            Post = await ValidatePostExistsAsync(id);
            ValidateRequestingUserCanAlterPost(requestingUser, Post);
        }

        public async Task ValidateDeletePostByIdInputParametersAsync(User requestingUser,
            ulong id)
        {
            Post = await ValidatePostExistsAsync(id);
            ValidateRequestingUserCanAlterPost(requestingUser, Post);
        }

        public async Task<Post> ValidatePostExistsAsync(ulong id, User requestingUser = null)
        {
            var post = await _postOperator.GetPostByIdAsync(id, true, true,
                requestingUser);

            if (post == null)
                throw new PostNotExistException(id);

            return post;
        }

        public void ValidatePostContentFormat(string content)
        {
            var maximumLength = 2000;
            if (content.Length > maximumLength)
                throw new PostContentInvalidFormatException(content);
        }

        public void ValidateRequestingUserCanAlterPost(User requestingUser,
            Post post)
        {
            if (requestingUser.Id != post.AuthorId)
                throw new PostAccessDeniedException(requestingUser.Id, post.Id);
        }

        public void ValidatePostIsNotVotedByUser(Post post, User requestingUser)
        {
            if (post.RequestingUserVote != 0)
                throw new PostVoteAlreadyExistsException(requestingUser.Id, post.Id);
        }

        public void ValidatePostVoteExists(Post post, User requestingUser)
        {
            if (post.RequestingUserVote == 0)
                throw new PostVoteNotExistException(requestingUser.Id, post.Id);
        }
    }
}
