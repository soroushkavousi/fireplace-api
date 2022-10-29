﻿using FireplaceApi.Core.Models;
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

        public async Task<QueryResult<Comment>> ListPostCommentsAsync(string encodedPostId,
            string sort, User requestingUser = null)
        {
            await _commentValidator.ValidateListPostCommentsInputParametersAsync(
                encodedPostId, sort, requestingUser);
            var queryResult = await _commentOperator.ListPostCommentsAsync(
                _commentValidator.PostId, _commentValidator.Sort, requestingUser);
            return queryResult;
        }

        public async Task<List<Comment>> ListCommentsByIdsAsync(string encodedIds,
            string sort, User requestingUser = null)
        {
            await _commentValidator.ValidateListCommentsByIdsInputParametersAsync(
                encodedIds, sort, requestingUser);
            var comments = await _commentOperator.ListCommentsByIdsAsync(
                _commentValidator.Ids, _commentValidator.Sort, requestingUser);
            return comments;
        }

        public async Task<QueryResult<Comment>> ListSelfCommentsAsync(User requestingUser,
            string sort)
        {
            await _commentValidator.ValidateListSelfCommentsInputParametersAsync(requestingUser,
                sort);
            var queryResult = await _commentOperator.ListSelfCommentsAsync(requestingUser,
                _commentValidator.Sort);
            return queryResult;
        }

        public async Task<QueryResult<Comment>> ListChildCommentsAsync(
            string encodedParentCommentId, User requestingUser = null)
        {
            await _commentValidator.ValidateListChildCommentsAsyncInputParametersAsync(
                encodedParentCommentId, requestingUser);
            var queryResult = await _commentOperator.ListChildCommentsAsync(requestingUser,
                _commentValidator.ParentCommentId);
            return queryResult;
        }

        public async Task<Comment> GetCommentByIdAsync(string encodedId,
            bool? includeAuthor, bool? includePost, User requestingUser = null)
        {
            await _commentValidator.ValidateGetCommentByIdInputParametersAsync(
                encodedId, includeAuthor, includePost, requestingUser);
            var comment = await _commentOperator.GetCommentByIdAsync(
                _commentValidator.CommentId, includeAuthor.Value, includePost.Value,
                requestingUser);
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
