using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Operators;
using FireplaceApi.Domain.Validators;
using FireplaceApi.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Services
{
    public class PostService
    {
        private readonly ILogger<PostService> _logger;
        private readonly PostValidator _postValidator;
        private readonly PostOperator _postOperator;

        public PostService(ILogger<PostService> logger,
            PostValidator postValidator, PostOperator postOperator)
        {
            _logger = logger;
            _postValidator = postValidator;
            _postOperator = postOperator;
        }

        public async Task<QueryResult<Post>> ListCommunityPostsAsync(string communityIdOrName, string sort,
            User requestingUser = null)
        {
            await _postValidator.ValidateListCommunityPostsInputParametersAsync(
                communityIdOrName, sort, requestingUser);
            return await _postOperator.ListCommunityPostsAsync(_postValidator.CommunityIdentifier,
                _postValidator.Sort, requestingUser);
        }

        public async Task<QueryResult<Post>> ListPostsAsync(string search, string sort, User requestingUser = null)
        {
            await _postValidator.ValidateListPostsInputParametersAsync(search, sort, requestingUser);
            return await _postOperator.ListPostsAsync(search, _postValidator.Sort, requestingUser);
        }

        public async Task<List<Post>> ListPostsByIdsAsync(string encodedIds, User requestingUser = null)
        {
            await _postValidator.ValidateListPostsByIdsInputParametersAsync(encodedIds, requestingUser);
            var communities = await _postOperator.ListPostsByIdsAsync(
                _postValidator.Ids, requestingUser);
            return communities;
        }

        public async Task<QueryResult<Post>> ListSelfPostsAsync(User requestingUser,
            string sort)
        {
            await _postValidator.ValidateListSelfPostsInputParametersAsync(requestingUser,
                sort);
            var queryResult = await _postOperator.ListSelfPostsAsync(requestingUser,
                _postValidator.Sort);
            return queryResult;
        }

        public async Task<Post> GetPostByIdAsync(User requestingUser, string encodedId,
            bool? includeAuthor, bool? includeCommunity)
        {
            await _postValidator.ValidateGetPostByIdInputParametersAsync(
                requestingUser, encodedId, includeAuthor, includeCommunity);
            var post = await _postOperator.GetPostByIdAsync(_postValidator.PostId,
                includeAuthor.Value, includeCommunity.Value, requestingUser);
            return post;
        }

        public async Task<Post> CreatePostAsync(User requestingUser,
            string communityEncodedIdOrName, string content)
        {
            await _postValidator.ValidateCreatePostInputParametersAsync(
                    requestingUser, communityEncodedIdOrName, content);
            return await _postOperator.CreatePostAsync(requestingUser,
                _postValidator.CommunityIdentifier, content);
        }

        public async Task<Post> VotePostAsync(User requestingUser,
            string encodedId, bool? isUpvote)
        {
            await _postValidator.ValidateVotePostInputParametersAsync(
                requestingUser, encodedId, isUpvote);
            var post = await _postOperator.VotePostAsync(
                requestingUser, _postValidator.PostId, isUpvote.Value);
            return post;
        }

        public async Task<Post> ToggleVoteForPostAsync(User requestingUser,
            string encodedId)
        {
            await _postValidator.ValidateToggleVoteForPostInputParametersAsync(
                requestingUser, encodedId);
            var post = await _postOperator.ToggleVoteForPostAsync(
                requestingUser, _postValidator.PostId);
            return post;
        }

        public async Task<Post> DeleteVoteForPostAsync(User requestingUser,
            string encodedId)
        {
            await _postValidator.ValidateDeleteVoteForPostInputParametersAsync(
                requestingUser, encodedId);
            var post = await _postOperator.DeleteVoteForPostAsync(
                requestingUser, _postValidator.PostId);
            return post;
        }

        public async Task<Post> PatchPostByIdAsync(User requestingUser,
            string encodedId, string content)
        {
            await _postValidator.ValidatePatchPostByIdInputParametersAsync(
                requestingUser, encodedId, content);
            var post = await _postOperator.PatchPostByIdAsync(requestingUser,
                _postValidator.PostId, content, null);
            return post;
        }

        public async Task DeletePostByIdAsync(User requestingUser, string encodedId)
        {
            await _postValidator.ValidateDeletePostByIdInputParametersAsync(
                requestingUser, encodedId);
            await _postOperator.DeletePostByIdAsync(_postValidator.PostId);
        }
    }
}
