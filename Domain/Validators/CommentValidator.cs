using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Exceptions;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Operators;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Validators
{
    public class CommentValidator : CoreValidator
    {
        private readonly ILogger<CommentValidator> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly CommentOperator _commentOperator;
        private readonly PostValidator _postValidator;

        public ulong CommentId { get; private set; }
        public Comment Comment { get; private set; }
        public ulong ParentCommentId { get; private set; }
        public Comment ParentComment { get; private set; }
        public ulong PostId { get; private set; }
        public Post Post { get; private set; }
        public List<ulong> Ids { get; private set; }
        public SortType? Sort { get; private set; }

        public CommentValidator(ILogger<CommentValidator> logger,
            IServiceProvider serviceProvider, CommentOperator commentOperator,
            PostValidator postValidator)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _commentOperator = commentOperator;
            _postValidator = postValidator;
        }

        public async Task ValidateListPostCommentsInputParametersAsync(string encodedPostId,
            string sort, User requestingUser)
        {
            Sort = ValidateInputEnum<SortType>(sort, nameof(sort), ErrorName.INPUT_SORT_IS_NOT_VALID);
            PostId = ValidateEncodedIdFormat(encodedPostId, nameof(encodedPostId)).Value;
            Post = await _postValidator.ValidatePostExistsAsync(PostId);
        }

        public async Task ValidateListCommentsByIdsInputParametersAsync(string encodedIds,
            string sort, User requestingUser)
        {
            Ids = ValidateIdsFormat(encodedIds);
            Sort = ValidateInputEnum<SortType>(sort, nameof(sort), ErrorName.INPUT_SORT_IS_NOT_VALID);
            await Task.CompletedTask;
        }

        public async Task ValidateListSelfCommentsInputParametersAsync(User requestingUser,
            string sort)
        {
            Sort = ValidateInputEnum<SortType>(sort, nameof(sort), ErrorName.INPUT_SORT_IS_NOT_VALID);
            await Task.CompletedTask;
        }

        public async Task ValidateListChildCommentsAsyncInputParametersAsync(
            string encodedParentCommentId, User requestingUser)
        {
            ParentCommentId = ValidateEncodedIdFormat(encodedParentCommentId,
                nameof(encodedParentCommentId)).Value;
            ParentComment = await ValidateCommentExistsAsync(ParentCommentId);
        }

        public async Task ValidateGetCommentByIdInputParametersAsync(string encodedId,
            bool? includeAuthor, bool? includePost, User requestingUser)
        {
            CommentId = ValidateEncodedIdFormat(encodedId, nameof(encodedId)).Value;
            Comment = await ValidateCommentExistsAsync(CommentId);
        }

        public async Task ValidateReplyToPostInputParametersAsync(User requestingUser,
            string encodedPostId, string content)
        {
            ValidateCommentContentFormat(content);
            PostId = ValidateEncodedIdFormat(encodedPostId, nameof(encodedPostId)).Value;
            Post = await _postValidator.ValidatePostExistsAsync(PostId);
        }

        public async Task ValidateReplyToCommentInputParametersAsync(User requestingUser,
            string encodedParentCommentId, string content)
        {
            ValidateCommentContentFormat(content);
            ParentCommentId = ValidateEncodedIdFormat(encodedParentCommentId,
                nameof(encodedParentCommentId)).Value;
            ParentComment = await ValidateCommentExistsAsync(ParentCommentId);
        }

        public async Task ValidateVoteCommentInputParametersAsync(User requestingUser,
            string encodedId, bool? isUpvote)
        {
            ValidateParameterIsNotMissing(isUpvote, nameof(isUpvote), ErrorName.IS_UPVOTE_IS_MISSING);
            CommentId = ValidateEncodedIdFormat(encodedId, nameof(encodedId)).Value;
            Comment = await ValidateCommentExistsAsync(CommentId, requestingUser);
            ValidateCommentIsNotVotedByUser(Comment, requestingUser);
        }

        public async Task ValidateToggleVoteForCommentInputParametersAsync(User requestingUser,
            string encodedId)
        {
            CommentId = ValidateEncodedIdFormat(encodedId, nameof(encodedId)).Value;
            Comment = await ValidateCommentExistsAsync(CommentId, requestingUser);
            ValidateCommentVoteExists(Comment, requestingUser);
        }

        public async Task ValidateDeleteVoteForCommentInputParametersAsync(User requestingUser,
            string encodedId)
        {
            CommentId = ValidateEncodedIdFormat(encodedId, nameof(encodedId)).Value;
            Comment = await ValidateCommentExistsAsync(CommentId, requestingUser);
            ValidateCommentVoteExists(Comment, requestingUser);
        }

        public async Task ValidatePatchCommentByIdInputParametersAsync(User requestingUser,
            string encodedId, string content)
        {
            CommentId = ValidateEncodedIdFormat(encodedId, nameof(encodedId)).Value;
            ValidateCommentContentFormat(content);
            Comment = await ValidateCommentExistsAsync(CommentId);
            ValidateRequestingUserCanAlterComment(requestingUser, Comment);
        }

        public async Task ValidateDeleteCommentByIdInputParametersAsync(User requestingUser,
            string encodedId)
        {
            CommentId = ValidateEncodedIdFormat(encodedId, nameof(encodedId)).Value;
            Comment = await ValidateCommentExistsAsync(CommentId);
            ValidateRequestingUserCanAlterComment(requestingUser, Comment);
        }

        public void ValidateCommentIsNotVotedByUser(Comment comment, User requestingUser)
        {
            if (comment.RequestingUserVote != 0)
            {
                var serverMessage = $"Comment id {comment.Id} is already voted by user {requestingUser.Username}!";
                throw new ApiException(ErrorName.COMMENT_VOTE_ALREADY_EXISTS, serverMessage);
            }
        }

        public void ValidateCommentVoteExists(Comment comment, User requestingUser)
        {
            if (comment.RequestingUserVote == 0)
            {
                var serverMessage = $"Comment id {comment.Id} is not voted by user {requestingUser.Username}!";
                throw new ApiException(ErrorName.COMMENT_VOTE_DOES_NOT_EXIST, serverMessage);
            }
        }

        public async Task<Comment> ValidateCommentExistsAsync(ulong id,
            User requestingUser = null)
        {
            var comment = await _commentOperator.GetCommentByIdAsync(id, true,
                true, requestingUser);
            if (comment == null)
            {
                var serverMessage = $"Comment id {id} doesn't exist!";
                throw new ApiException(ErrorName.COMMENT_DOES_NOT_EXIST, serverMessage);
            }
            return comment;
        }

        public void ValidateCommentContentFormat(string content)
        {
            var maximumLength = 3000;
            if (content.Length > maximumLength)
            {
                var serverMessage = $"Comment content exceeds the maximum length! "
                    + new { maximumLength, ContentLength = content.Length }.ToJson();
                throw new ApiException(ErrorName.COMMENT_CONTENT_MAX_LENGTH, serverMessage);
            }
        }

        public void ValidateRequestingUserCanAlterComment(User requestingUser, Comment comment)
        {
            if (requestingUser.Id != comment.AuthorId)
            {
                var serverMessage = $"requestingUser {requestingUser.Id} can't alter the comment {comment.Id}";
                throw new ApiException(ErrorName.USER_CAN_NOT_ALTER_COMMENT, serverMessage);
            }
        }
    }
}
