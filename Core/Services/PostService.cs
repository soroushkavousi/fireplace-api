using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.Tools;
using FireplaceApi.Core.Validators;
using FireplaceApi.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Services
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
            await _postValidator
                .ValidateListPostsByIdsInputParametersAsync(encodedIds, requestingUser);
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
            var id = encodedId.IdDecode();
            var post = await _postOperator.GetPostByIdAsync(id,
                includeAuthor.Value, includeCommunity.Value, requestingUser);
            return post;
        }

        public async Task<Post> CreatePostAsync(User requestingUser, string encodedCommunityId,
            string communityName, string content)
        {
            var communityIdentifier = await _postValidator.ValidateCreatePostInputParametersAsync(
                    requestingUser, encodedCommunityId, communityName, content);
            return await _postOperator.CreatePostAsync(requestingUser, communityIdentifier,
                    content);
        }

        public async Task<Post> VotePostAsync(User requestingUser,
            string encodedId, bool? isUpvote)
        {
            await _postValidator.ValidateVotePostInputParametersAsync(
                requestingUser, encodedId, isUpvote);
            var id = encodedId.IdDecode();
            var post = await _postOperator.VotePostAsync(
                requestingUser, id, isUpvote.Value);
            return post;
        }

        public async Task<Post> ToggleVoteForPostAsync(User requestingUser,
            string encodedId)
        {
            await _postValidator.ValidateToggleVoteForPostInputParametersAsync(
                requestingUser, encodedId);
            var id = encodedId.IdDecode();
            var post = await _postOperator.ToggleVoteForPostAsync(
                requestingUser, id);
            return post;
        }

        public async Task<Post> DeleteVoteForPostAsync(User requestingUser,
            string encodedId)
        {
            await _postValidator.ValidateDeleteVoteForPostInputParametersAsync(
                requestingUser, encodedId);
            var id = encodedId.IdDecode();
            var post = await _postOperator.DeleteVoteForPostAsync(
                requestingUser, id);
            return post;
        }

        public async Task<Post> PatchPostByIdAsync(User requestingUser,
            string encodedId, string content)
        {
            await _postValidator.ValidatePatchPostByIdInputParametersAsync(
                requestingUser, encodedId, content);
            var id = encodedId.IdDecode();
            var post = await _postOperator.PatchPostByIdAsync(requestingUser,
                id, content, null);
            return post;
        }

        public async Task DeletePostByIdAsync(User requestingUser, string encodedId)
        {
            await _postValidator.ValidateDeletePostByIdInputParametersAsync(
                requestingUser, encodedId);
            var id = encodedId.IdDecode();
            await _postOperator.DeletePostByIdAsync(id);
        }
    }
}
