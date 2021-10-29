using FireplaceApi.Core.Enums;
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
            bool? joined, long? communityId, string communityName,
            string search, SortType? sort, string stringOfSort)
        {
            await _postValidator.ValidateListPostsInputParametersAsync(requesterUser,
                paginationInputParameters, self, joined, communityId, communityName,
                search, sort, stringOfSort);
            var page = await _postOperator.ListPostsAsync(requesterUser,
                paginationInputParameters, self, joined, communityId, communityName,
                search, sort);
            return page;
        }

        public async Task<Post> GetPostByIdAsync(User requesterUser, long? id,
            bool? includeAuthor, bool? includeCommunity)
        {
            await _postValidator.ValidateGetPostByIdInputParametersAsync(
                requesterUser, id, includeAuthor, includeCommunity);
            var post = await _postOperator.GetPostByIdAsync(id.Value,
                includeAuthor.Value, includeCommunity.Value);
            return post;
        }

        public async Task<Post> CreatePostAsync(User requesterUser, long? communityId,
            string communityName, string content)
        {
            await _postValidator
                .ValidateCreatePostInputParametersAsync(
                    requesterUser, communityId, communityName, content);
            var communityIdentifier = new Identifier(communityId, communityName);
            return await _postOperator
                .CreatePostAsync(requesterUser, communityIdentifier,
                    content);
        }

        public async Task<Post> VotePostAsync(User requesterUser,
            long id, bool? isUpvote)
        {
            await _postValidator.ValidateVotePostInputParametersAsync(
                requesterUser, id, isUpvote);
            var post = await _postOperator.VotePostAsync(
                requesterUser, id, isUpvote.Value);
            return post;
        }

        public async Task<Post> ToggleVoteForPostAsync(User requesterUser,
            long id)
        {
            await _postValidator.ValidateToggleVoteForPostInputParametersAsync(
                requesterUser, id);
            var post = await _postOperator.ToggleVoteForPostAsync(
                requesterUser, id);
            return post;
        }

        public async Task<Post> DeleteVoteForPostAsync(User requesterUser,
            long id)
        {
            await _postValidator.ValidateDeleteVoteForPostInputParametersAsync(
                requesterUser, id);
            var post = await _postOperator.DeleteVoteForPostAsync(
                requesterUser, id);
            return post;
        }

        public async Task<Post> PatchPostByIdAsync(User requesterUser,
            long? id, string content)
        {
            await _postValidator
                .ValidatePatchPostByIdInputParametersAsync(requesterUser, id, content);
            var post = await _postOperator
                .PatchPostByIdAsync(id.Value, content, null);
            return post;
        }

        public async Task DeletePostByIdAsync(User requesterUser, long? id)
        {
            await _postValidator
                .ValidateDeletePostByIdInputParametersAsync(requesterUser, id);
            await _postOperator
                .DeletePostByIdAsync(id.Value);
        }
    }
}
