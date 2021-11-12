﻿using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Identifiers;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Tools;
using FireplaceApi.Core.ValueObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Operators
{
    public class PostOperator
    {
        private readonly ILogger<PostOperator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly IPostRepository _postRepository;
        private readonly IPostVoteRepository _postVoteRepository;
        private readonly PageOperator _pageOperator;
        private readonly UserOperator _userOperator;
        private readonly CommunityOperator _communityOperator;

        public PostOperator(ILogger<PostOperator> logger,
            IConfiguration configuration, IServiceProvider serviceProvider,
            IPostRepository postRepository,
            IPostVoteRepository postVoteRepository,
            PageOperator pageOperator, UserOperator userOperator,
            CommunityOperator communityOperator)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _postRepository = postRepository;
            _postVoteRepository = postVoteRepository;
            _pageOperator = pageOperator;
            _userOperator = userOperator;
            _communityOperator = communityOperator;
        }

        public async Task<Page<Post>> ListPostsAsync(User requestingUser,
            PaginationInputParameters paginationInputParameters, bool? self,
            bool? joined, CommunityIdentifier communityIdentifier,
            string search, SortType? sort)
        {
            Page<Post> resultPage = default;
            if (string.IsNullOrWhiteSpace(paginationInputParameters.Pointer))
            {
                var postIds = await _postRepository.ListPostIdsAsync(
                    requestingUser.Id, self, joined, communityIdentifier, search, sort);
                resultPage = await _pageOperator.CreatePageWithoutPointerAsync(
                    ModelName.POST, paginationInputParameters, postIds,
                    _postRepository.ListPostsAsync, requestingUser);
            }
            else
            {
                resultPage = await _pageOperator.CreatePageWithPointerAsync(
                    ModelName.POST, paginationInputParameters,
                    _postRepository.ListPostsAsync, requestingUser);
            }
            return resultPage;
        }

        public async Task<Post> GetPostByIdAsync(ulong id,
            bool includeAuthor, bool includeCommunity, User requestingUser)
        {
            var post = await _postRepository.GetPostByIdAsync(
                id, includeAuthor, includeCommunity, requestingUser);
            if (post == null)
                return post;

            return post;
        }

        public async Task<Post> CreatePostAsync(User requestingUser,
            CommunityIdentifier communityIdentifier, string content)
        {
            ulong communityId = default;
            string communityName = default;
            switch (communityIdentifier)
            {
                case CommunityIdIdentifier idIdentifier:
                    communityId = idIdentifier.Id;
                    communityName = await _communityOperator
                        .GetNameByIdAsync(communityId);
                    break;
                case CommunityNameIdentifier nameIdentifier:
                    communityName = nameIdentifier.Name;
                    communityId = await _communityOperator
                        .GetIdByNameAsync(communityName);
                    break;
            }
            var id = await IdGenerator.GenerateNewIdAsync(DoesPostIdExistAsync);
            var post = await _postRepository.CreatePostAsync(
                id, requestingUser.Id, requestingUser.Username,
                communityId, communityName, content);
            return post;
        }

        public async Task<Post> VotePostAsync(User requestingUser,
            ulong id, bool isUp)
        {
            var postVoteId = await IdGenerator.GenerateNewIdAsync(
                DoesPostVoteIdExistAsync);
            var postVote = await _postVoteRepository.CreatePostVoteAsync(
                postVoteId, requestingUser.Id, requestingUser.Username,
                id, isUp);
            var voteChange = postVote.IsUp ? +1 : -1;
            var post = await PatchPostByIdAsync(requestingUser,
                id, null, voteChange: voteChange);
            return post;
        }

        public async Task<Post> ToggleVoteForPostAsync(User requestingUser,
            ulong id)
        {
            var postVote = await _postVoteRepository.GetPostVoteAsync(
                requestingUser.Id, id, includePost: true);
            postVote.IsUp = !postVote.IsUp;
            await _postVoteRepository.UpdatePostVoteAsync(postVote);
            var voteChange = postVote.IsUp ? +2 : -2;
            var post = await ApplyPostChangesAsync(postVote.Post,
                null, voteChange: voteChange);
            post = await GetPostByIdAsync(post.Id,
                false, false, requestingUser);
            return post;
        }

        public async Task<Post> DeleteVoteForPostAsync(User requestingUser,
            ulong id)
        {
            var postVote = await _postVoteRepository.GetPostVoteAsync(
                requestingUser.Id, id, includePost: true);
            var voteChange = postVote.IsUp ? -1 : +1;
            await _postVoteRepository.DeletePostVoteByIdAsync(
                postVote.Id);
            var post = await ApplyPostChangesAsync(postVote.Post,
                null, voteChange: voteChange);
            return post;
        }

        public async Task<Post> PatchPostByIdAsync(User requestingUser,
            ulong id, string content, int? voteChange)
        {
            var post = await _postRepository
                .GetPostByIdAsync(id);
            post = await ApplyPostChangesAsync(post, content,
                voteChange);
            post = await GetPostByIdAsync(post.Id,
                false, false, requestingUser);
            return post;
        }

        public async Task DeletePostByIdAsync(ulong id)
        {
            await _postRepository.DeletePostByIdAsync(id);
        }

        public async Task<bool> DoesPostIdExistAsync(ulong id)
        {
            var postIdExists = await _postRepository
                .DoesPostIdExistAsync(id);
            return postIdExists;
        }

        public async Task<bool> DoesPostVoteIdExistAsync(ulong id)
        {
            var postVoteIdExists = await _postVoteRepository
                .DoesPostVoteIdExistAsync(id);
            return postVoteIdExists;
        }

        public async Task<Post> ApplyPostChangesAsync(
            Post post, string content, int? voteChange)
        {
            if (content != null)
            {
                post.Content = content;
            }

            if (voteChange.HasValue)
            {
                post.Vote += voteChange.Value;
            }

            post = await _postRepository
                .UpdatePostAsync(post);
            return post;
        }
    }
}
