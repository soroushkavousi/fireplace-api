using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.Validators;
using FireplaceApi.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Services
{
    public class CommentService
    {
        private readonly ILogger<CommentService> _logger;
        private readonly CommentValidator _commentValidator;
        private readonly CommentOperator _commentOperator;

        public CommentService(ILogger<CommentService> logger,
            CommentValidator commentValidator, CommentOperator commentOperator)
        {
            _logger = logger;
            _commentValidator = commentValidator;
            _commentOperator = commentOperator;
        }

        public async Task<Page<Comment>> ListSelfCommentsAsync(User requesterUser,
            PaginationInputParameters paginationInputParameters, SortType? sort,
            string stringOfSort)
        {
            await _commentValidator.ValidateListSelfCommentsInputParametersAsync(requesterUser,
                paginationInputParameters, sort, stringOfSort);
            var page = await _commentOperator.ListSelfCommentsAsync(requesterUser,
                paginationInputParameters, sort);
            return page;
        }

        public async Task<Page<Comment>> ListPostCommentsAsync(User requesterUser,
            PaginationInputParameters paginationInputParameters, string encodedPostId,
            SortType? sort, string stringOfSort)
        {
            await _commentValidator.ValidateListPostCommentsInputParametersAsync(requesterUser,
                paginationInputParameters, encodedPostId, sort, stringOfSort);
            var postId = encodedPostId.Decode();
            var page = await _commentOperator.ListPostCommentsAsync(requesterUser,
                paginationInputParameters, postId, sort);
            return page;
        }

        public async Task<List<Comment>> ListChildCommentsAsync(User requesterUser,
            string encodedParentCommentId)
        {
            await _commentValidator.ValidateListChildCommentsAsyncInputParametersAsync(
                requesterUser, encodedParentCommentId);
            var parentCommentId = encodedParentCommentId.Decode();
            var page = await _commentOperator.ListChildCommentsAsync(requesterUser,
                parentCommentId);
            return page;
        }

        public async Task<Comment> GetCommentByIdAsync(User requesterUser, string encodedId,
            bool? includeAuthor, bool? includePost)
        {
            await _commentValidator.ValidateGetCommentByIdInputParametersAsync(
                requesterUser, encodedId, includeAuthor, includePost);
            var id = encodedId.Decode();
            var comment = await _commentOperator.GetCommentByIdAsync(id,
                includeAuthor.Value, includePost.Value, requesterUser);
            return comment;
        }

        public async Task<Comment> ReplyToPostAsync(User requesterUser, string encodedPostId,
            string content)
        {
            await _commentValidator.ValidateReplyToPostInputParametersAsync(
                requesterUser, encodedPostId, content);
            var postId = encodedPostId.Decode();
            return await _commentOperator.ReplyToPostAsync(requesterUser,
                postId, content);
        }

        public async Task<Comment> ReplyToCommentAsync(User requesterUser,
            string encodedCommentId, string content)
        {
            await _commentValidator.ValidateReplyToCommentInputParametersAsync(
                requesterUser, encodedCommentId, content);
            var commentId = encodedCommentId.Decode();
            return await _commentOperator.ReplyToCommentAsync(requesterUser,
                commentId, content);
        }

        public async Task<Comment> VoteCommentAsync(User requesterUser,
            string encodedId, bool? isUpvote)
        {
            await _commentValidator.ValidateVoteCommentInputParametersAsync(
                requesterUser, encodedId, isUpvote);
            var id = encodedId.Decode();
            var comment = await _commentOperator.VoteCommentAsync(
                requesterUser, id, isUpvote.Value);
            return comment;
        }

        public async Task<Comment> ToggleVoteForCommentAsync(User requesterUser,
            string encodedId)
        {
            await _commentValidator.ValidateToggleVoteForCommentInputParametersAsync(
                requesterUser, encodedId);
            var id = encodedId.Decode();
            var comment = await _commentOperator.ToggleVoteForCommentAsync(
                requesterUser, id);
            return comment;
        }

        public async Task<Comment> DeleteVoteForCommentAsync(User requesterUser,
            string encodedId)
        {
            await _commentValidator.ValidateDeleteVoteForCommentInputParametersAsync(
                requesterUser, encodedId);
            var id = encodedId.Decode();
            var comment = await _commentOperator.DeleteVoteForCommentAsync(
                requesterUser, id);
            return comment;
        }

        public async Task<Comment> PatchCommentByIdAsync(User requesterUser,
            string encodedId, string content)
        {
            await _commentValidator
                .ValidatePatchCommentByIdInputParametersAsync(requesterUser,
                    encodedId, content);
            var id = encodedId.Decode();
            var comment = await _commentOperator.PatchCommentByIdAsync(
                requesterUser, id, content, null);
            return comment;
        }

        public async Task DeleteCommentByIdAsync(User requesterUser, string encodedId)
        {
            await _commentValidator
                .ValidateDeleteCommentByIdInputParametersAsync(requesterUser, encodedId);
            var id = encodedId.Decode();
            await _commentOperator.DeleteCommentByIdAsync(id);
        }
    }
}
