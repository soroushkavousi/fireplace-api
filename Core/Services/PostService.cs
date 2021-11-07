using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.Validators;
using FireplaceApi.Core.ValueObjects;
using Microsoft.Extensions.Logging;
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

        public async Task<Page<Post>> ListPostsAsync(User requesterUser,
            PaginationInputParameters paginationInputParameters, bool? self,
            bool? joined, string encodedCommunityId, string communityName,
            string search, SortType? sort, string stringOfSort)
        {
            await _postValidator.ValidateListPostsInputParametersAsync(requesterUser,
                paginationInputParameters, self, joined, encodedCommunityId, communityName,
                search, sort, stringOfSort);
            var communityId = encodedCommunityId.DecodeIdOrDefault();
            var page = await _postOperator.ListPostsAsync(requesterUser,
                paginationInputParameters, self, joined, communityId, communityName,
                search, sort);
            return page;
        }

        public async Task<Post> GetPostByIdAsync(User requesterUser, string encodedId,
            bool? includeAuthor, bool? includeCommunity)
        {
            await _postValidator.ValidateGetPostByIdInputParametersAsync(
                requesterUser, encodedId, includeAuthor, includeCommunity);
            var id = encodedId.Decode();
            var post = await _postOperator.GetPostByIdAsync(id,
                includeAuthor.Value, includeCommunity.Value, requesterUser);
            return post;
        }

        public async Task<Post> CreatePostAsync(User requesterUser, string encodedCommunityId,
            string communityName, string content)
        {
            await _postValidator.ValidateCreatePostInputParametersAsync(
                    requesterUser, encodedCommunityId, communityName, content);
            var communityId = encodedCommunityId.DecodeIdOrDefault();
            var communityIdentifier = new Identifier(communityId, communityName);
            return await _postOperator
                .CreatePostAsync(requesterUser, communityIdentifier,
                    content);
        }

        public async Task<Post> VotePostAsync(User requesterUser,
            string encodedId, bool? isUpvote)
        {
            await _postValidator.ValidateVotePostInputParametersAsync(
                requesterUser, encodedId, isUpvote);
            var id = encodedId.Decode();
            var post = await _postOperator.VotePostAsync(
                requesterUser, id, isUpvote.Value);
            return post;
        }

        public async Task<Post> ToggleVoteForPostAsync(User requesterUser,
            string encodedId)
        {
            await _postValidator.ValidateToggleVoteForPostInputParametersAsync(
                requesterUser, encodedId);
            var id = encodedId.Decode();
            var post = await _postOperator.ToggleVoteForPostAsync(
                requesterUser, id);
            return post;
        }

        public async Task<Post> DeleteVoteForPostAsync(User requesterUser,
            string encodedId)
        {
            await _postValidator.ValidateDeleteVoteForPostInputParametersAsync(
                requesterUser, encodedId);
            var id = encodedId.Decode();
            var post = await _postOperator.DeleteVoteForPostAsync(
                requesterUser, id);
            return post;
        }

        public async Task<Post> PatchPostByIdAsync(User requesterUser,
            string encodedId, string content)
        {
            await _postValidator.ValidatePatchPostByIdInputParametersAsync(
                requesterUser, encodedId, content);
            var id = encodedId.Decode();
            var post = await _postOperator.PatchPostByIdAsync(requesterUser,
                id, content, null);
            return post;
        }

        public async Task DeletePostByIdAsync(User requesterUser, string encodedId)
        {
            await _postValidator.ValidateDeletePostByIdInputParametersAsync(
                requesterUser, encodedId);
            var id = encodedId.Decode();
            await _postOperator.DeletePostByIdAsync(id);
        }
    }
}
