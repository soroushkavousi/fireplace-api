using FireplaceApi.Core.Enums;
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

        public async Task<Page<Post>> ListPostsAsync(User requesterUser,
            PaginationInputParameters paginationInputParameters, bool? self,
            bool? joined, ulong? communityId, string communityName,
            string search, SortType? sort)
        {
            Page<Post> resultPage = default;
            if (string.IsNullOrWhiteSpace(paginationInputParameters.Pointer))
            {
                var postIds = await _postRepository.ListPostIdsAsync(
                    requesterUser.Id, self, joined, communityId,
                    communityName, search, sort);
                resultPage = await _pageOperator.CreatePageWithoutPointerAsync(
                    ModelName.POST, paginationInputParameters, postIds,
                    _postRepository.ListPostsAsync, requesterUser);
            }
            else
            {
                resultPage = await _pageOperator.CreatePageWithPointerAsync(
                    ModelName.POST, paginationInputParameters,
                    _postRepository.ListPostsAsync, requesterUser);
            }
            return resultPage;
        }

        public async Task<Post> GetPostByIdAsync(ulong id,
            bool includeAuthor, bool includeCommunity, User requesterUser)
        {
            var post = await _postRepository.GetPostByIdAsync(
                id, includeAuthor, includeCommunity, requesterUser);
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
            var id = await IdGenerator.GenerateNewIdAsync(DoesPostIdExistAsync);
            var post = await _postRepository
                .CreatePostAsync(id, requesterUser.Id, requesterUser.Username,
                    communityIdentifier.Id.Value, communityIdentifier.Name,
                    content);
            return post;
        }

        public async Task<Post> VotePostAsync(User requesterUser,
            ulong id, bool isUp)
        {
            var postVoteId = await IdGenerator.GenerateNewIdAsync(
                DoesPostVoteIdExistAsync);
            var postVote = await _postVoteRepository.CreatePostVoteAsync(
                postVoteId, requesterUser.Id, requesterUser.Username,
                id, isUp);
            var voteChange = postVote.IsUp ? +1 : -1;
            var post = await PatchPostByIdAsync(requesterUser,
                id, null, voteChange: voteChange);
            return post;
        }

        public async Task<Post> ToggleVoteForPostAsync(User requesterUser,
            ulong id)
        {
            var postVote = await _postVoteRepository.GetPostVoteAsync(
                requesterUser.Id, id, includePost: true);
            postVote.IsUp = !postVote.IsUp;
            await _postVoteRepository.UpdatePostVoteAsync(postVote);
            var voteChange = postVote.IsUp ? +2 : -2;
            var post = await ApplyPostChangesAsync(postVote.Post,
                null, voteChange: voteChange);
            post = await GetPostByIdAsync(post.Id,
                false, false, requesterUser);
            return post;
        }

        public async Task<Post> DeleteVoteForPostAsync(User requesterUser,
            ulong id)
        {
            var postVote = await _postVoteRepository.GetPostVoteAsync(
                requesterUser.Id, id, includePost: true);
            var voteChange = postVote.IsUp ? -1 : +1;
            await _postVoteRepository.DeletePostVoteByIdAsync(
                postVote.Id);
            var post = await ApplyPostChangesAsync(postVote.Post,
                null, voteChange: voteChange);
            return post;
        }

        public async Task<Post> PatchPostByIdAsync(User requesterUser,
            ulong id, string content, int? voteChange)
        {
            var post = await _postRepository
                .GetPostByIdAsync(id);
            post = await ApplyPostChangesAsync(post, content,
                voteChange);
            post = await GetPostByIdAsync(post.Id,
                false, false, requesterUser);
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
