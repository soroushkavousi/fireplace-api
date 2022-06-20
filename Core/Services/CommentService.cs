using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Extensions;
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

        public async Task<Page<Comment>> ListSelfCommentsAsync(User requestingUser,
            PaginationInputParameters paginationInputParameters, string sort)
        {
            await _commentValidator.ValidateListSelfCommentsInputParametersAsync(requestingUser,
                paginationInputParameters, sort);
            var page = await _commentOperator.ListSelfCommentsAsync(requestingUser,
                paginationInputParameters, sort.ToNullableEnum<SortType>());
            return page;
        }

        public async Task<Page<Comment>> ListPostCommentsAsync(User requestingUser,
            PaginationInputParameters paginationInputParameters, string encodedPostId,
            string sort)
        {
            await _commentValidator.ValidateListPostCommentsInputParametersAsync(requestingUser,
                paginationInputParameters, encodedPostId, sort);
            var postId = encodedPostId.IdDecode();
            var page = await _commentOperator.ListPostCommentsAsync(requestingUser,
                paginationInputParameters, postId, sort.ToNullableEnum<SortType>());
            return page;
        }

        public async Task<List<Comment>> ListChildCommentsAsync(User requestingUser,
            string encodedParentCommentId)
        {
            await _commentValidator.ValidateListChildCommentsAsyncInputParametersAsync(
                requestingUser, encodedParentCommentId);
            var parentCommentId = encodedParentCommentId.IdDecode();
            var page = await _commentOperator.ListChildCommentsAsync(requestingUser,
                parentCommentId);
            return page;
        }

        public async Task<Comment> GetCommentByIdAsync(User requestingUser, string encodedId,
            bool? includeAuthor, bool? includePost)
        {
            await _commentValidator.ValidateGetCommentByIdInputParametersAsync(
                requestingUser, encodedId, includeAuthor, includePost);
            var id = encodedId.IdDecode();
            var comment = await _commentOperator.GetCommentByIdAsync(id,
                includeAuthor.Value, includePost.Value, requestingUser);
            return comment;
        }

        public async Task<Comment> ReplyToPostAsync(User requestingUser, string encodedPostId,
            string content)
        {
            await _commentValidator.ValidateReplyToPostInputParametersAsync(
                requestingUser, encodedPostId, content);
            var postId = encodedPostId.IdDecode();
            return await _commentOperator.ReplyToPostAsync(requestingUser,
                postId, content);
        }

        public async Task<Comment> ReplyToCommentAsync(User requestingUser,
            string encodedCommentId, string content)
        {
            await _commentValidator.ValidateReplyToCommentInputParametersAsync(
                requestingUser, encodedCommentId, content);
            var commentId = encodedCommentId.IdDecode();
            return await _commentOperator.ReplyToCommentAsync(requestingUser,
                commentId, content);
        }

        public async Task<Comment> VoteCommentAsync(User requestingUser,
            string encodedId, bool? isUpvote)
        {
            await _commentValidator.ValidateVoteCommentInputParametersAsync(
                requestingUser, encodedId, isUpvote);
            var id = encodedId.IdDecode();
            var comment = await _commentOperator.VoteCommentAsync(
                requestingUser, id, isUpvote.Value);
            return comment;
        }

        public async Task<Comment> ToggleVoteForCommentAsync(User requestingUser,
            string encodedId)
        {
            await _commentValidator.ValidateToggleVoteForCommentInputParametersAsync(
                requestingUser, encodedId);
            var id = encodedId.IdDecode();
            var comment = await _commentOperator.ToggleVoteForCommentAsync(
                requestingUser, id);
            return comment;
        }

        public async Task<Comment> DeleteVoteForCommentAsync(User requestingUser,
            string encodedId)
        {
            await _commentValidator.ValidateDeleteVoteForCommentInputParametersAsync(
                requestingUser, encodedId);
            var id = encodedId.IdDecode();
            var comment = await _commentOperator.DeleteVoteForCommentAsync(
                requestingUser, id);
            return comment;
        }

        public async Task<Comment> PatchCommentByIdAsync(User requestingUser,
            string encodedId, string content)
        {
            await _commentValidator
                .ValidatePatchCommentByIdInputParametersAsync(requestingUser,
                    encodedId, content);
            var id = encodedId.IdDecode();
            var comment = await _commentOperator.PatchCommentByIdAsync(
                requestingUser, id, content, null);
            return comment;
        }

        public async Task DeleteCommentByIdAsync(User requestingUser, string encodedId)
        {
            await _commentValidator
                .ValidateDeleteCommentByIdInputParametersAsync(requestingUser, encodedId);
            var id = encodedId.IdDecode();
            await _commentOperator.DeleteCommentByIdAsync(id);
        }
    }
}
