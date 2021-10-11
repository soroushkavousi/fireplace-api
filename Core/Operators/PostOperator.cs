using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Models;
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
        private readonly PageOperator _pageOperator;
        private readonly UserOperator _userOperator;
        private readonly CommunityOperator _communityOperator;

        public PostOperator(ILogger<PostOperator> logger,
            IConfiguration configuration, IServiceProvider serviceProvider,
            IPostRepository postRepository,
            PageOperator pageOperator, UserOperator userOperator,
            CommunityOperator communityOperator)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _postRepository = postRepository;
            _pageOperator = pageOperator;
            _userOperator = userOperator;
            _communityOperator = communityOperator;
        }

        public async Task<Page<Post>> ListPostsAsync(User requesterUser,
            PaginationInputParameters paginationInputParameters, bool? self,
            bool? joined, long? communityId, string communityName,
            string search, SortType? sort)
        {
            Page<Post> resultPage = default;
            if (string.IsNullOrWhiteSpace(paginationInputParameters.Pointer))
            {
                var postIds = await _postRepository.ListPostIdsAsync(
                    requesterUser.Id, self, joined, communityId, 
                    communityName, search, sort);
                resultPage = await _pageOperator.CreatePageWithoutPointerAsync(
                    ModelName.COMMUNITY_MEMBERSHIP, paginationInputParameters, postIds,
                    _postRepository.ListPostsAsync);
            }
            else
            {
                resultPage = await _pageOperator.CreatePageWithPointerAsync(
                    ModelName.COMMUNITY_MEMBERSHIP, paginationInputParameters,
                    _postRepository.ListPostsAsync);
            }
            return resultPage;
        }

        public async Task<Post> GetPostByIdAsync(long id,
            bool includeAuthor, bool includeCommunity)
        {
            var post = await _postRepository
                .GetPostByIdAsync(id, includeAuthor, includeCommunity);
            if (post == null)
                return post;

            return post;
        }

        public async Task<Post> CreatePostAsync(User requesterUser,
            Identifier communityIdentifier, string content)
        {
            switch (communityIdentifier.State)
            {
                case IdentifierState.HasId:
                    communityIdentifier.Name = await _communityOperator
                        .GetNameByIdAsync(communityIdentifier.Id.Value);
                    break;
                case IdentifierState.HasName:
                    communityIdentifier.Id = await _communityOperator
                        .GetIdByNameAsync(communityIdentifier.Name);
                    break;
            }
            var post = await _postRepository
                .CreatePostAsync(requesterUser.Id, requesterUser.Username, 
                    communityIdentifier.Id.Value, communityIdentifier.Name,
                    content);
            return post;
        }

        public async Task<Post> PatchPostByIdAsync(long id, string content)
        {
            var post = await _postRepository
                .GetPostByIdAsync(id);
            post = await ApplyPostChangesAsync(post, content);
            post = await GetPostByIdAsync(post.Id,
                false, false);
            return post;
        }

        public async Task DeletePostByIdAsync(long id)
        {
            await _postRepository.DeletePostByIdAsync(id);
        }

        public async Task<bool> DoesPostIdExistAsync(long id)
        {
            var postIdExists = await _postRepository
                .DoesPostIdExistAsync(id);
            return postIdExists;
        }

        public async Task<Post> ApplyPostChangesAsync(
            Post post, string content)
        {
            if (content != null)
            {
                post.Content = content;
            }

            post = await _postRepository
                .UpdatePostAsync(post);
            return post;
        }
    }
}
