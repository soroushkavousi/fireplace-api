using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Validators
{
    public class CommentValidator : ApiValidator
    {
        private readonly ILogger<CommentValidator> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly CommentOperator _commentOperator;
        private readonly QueryResultValidator _queryResultValidator;
        private readonly PostValidator _postValidator;

        public CommentValidator(ILogger<CommentValidator> logger,
            IServiceProvider serviceProvider, CommentOperator commentOperator,
            QueryResultValidator queryResultValidator, PostValidator postValidator)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _commentOperator = commentOperator;
            _queryResultValidator = queryResultValidator;
            _postValidator = postValidator;
        }

        public async Task ValidateListSelfCommentsInputParametersAsync(User requestingUser,
            PaginationInputParameters paginationInputParameters, string sort)
        {
            await _queryResultValidator.ValidatePaginationInputParameters(paginationInputParameters,
                ModelName.COMMENT);

            ValidateInputEnum<SortType>(sort, nameof(sort), ErrorName.INPUT_SORT_IS_NOT_VALID);
        }

        public async Task ValidateListPostCommentsInputParametersAsync(User requestingUser,
            PaginationInputParameters paginationInputParameters, string encodedPostId,
            string sort)
        {
            await _queryResultValidator.ValidatePaginationInputParameters(paginationInputParameters,
                ModelName.COMMENT);

            ValidateInputEnum<SortType>(sort, nameof(sort), ErrorName.INPUT_SORT_IS_NOT_VALID);
            var postId = ValidateEncodedIdFormat(encodedPostId, nameof(encodedPostId)).Value;
            var post = await _postValidator.ValidatePostExistsAsync(postId);
        }

        public async Task ValidateListChildCommentsAsyncInputParametersAsync(
            User requestingUser, string encodedParentCommentId)
        {
            var parentCommentId = ValidateEncodedIdFormat(encodedParentCommentId,
                nameof(encodedParentCommentId)).Value;
            var parentComment = await ValidateCommentExistsAsync(parentCommentId);
        }

        public async Task ValidateGetCommentByIdInputParametersAsync(User requestingUser,
            string encodedId, bool? includeAuthor, bool? includePost)
        {
            var id = ValidateEncodedIdFormat(encodedId, nameof(encodedId)).Value;
            var comment = await ValidateCommentExistsAsync(id);
        }

        public async Task ValidateReplyToPostInputParametersAsync(User requestingUser,
            string encodedPostId, string content)
        {
            ValidateCommentContentFormat(content);
            var postId = ValidateEncodedIdFormat(encodedPostId, nameof(encodedPostId)).Value;
            var post = await _postValidator.ValidatePostExistsAsync(postId);
        }

        public async Task ValidateReplyToCommentInputParametersAsync(User requestingUser,
            string encodedParentCommentId, string content)
        {
            ValidateCommentContentFormat(content);
            var parentCommentId = ValidateEncodedIdFormat(encodedParentCommentId,
                nameof(encodedParentCommentId)).Value;
            var parentComment = await ValidateCommentExistsAsync(parentCommentId);
        }

        public async Task ValidateVoteCommentInputParametersAsync(User requestingUser,
            string encodedId, bool? isUpvote)
        {
            ValidateParameterIsNotMissing(isUpvote, nameof(isUpvote), ErrorName.IS_UPVOTE_IS_MISSING);
            var id = ValidateEncodedIdFormat(encodedId, nameof(encodedId)).Value;
            var comment = await ValidateCommentExistsAsync(id, requestingUser);
            ValidateCommentIsNotVotedByUser(comment, requestingUser);
        }

        public async Task ValidateToggleVoteForCommentInputParametersAsync(User requestingUser,
            string encodedId)
        {
            var id = ValidateEncodedIdFormat(encodedId, nameof(encodedId)).Value;
            var comment = await ValidateCommentExistsAsync(id, requestingUser);
            ValidateCommentVoteExists(comment, requestingUser);
        }

        public async Task ValidateDeleteVoteForCommentInputParametersAsync(User requestingUser,
            string encodedId)
        {
            var id = ValidateEncodedIdFormat(encodedId, nameof(encodedId)).Value;
            var comment = await ValidateCommentExistsAsync(id, requestingUser);
            ValidateCommentVoteExists(comment, requestingUser);
        }

        public async Task ValidatePatchCommentByIdInputParametersAsync(User requestingUser,
            string encodedId, string content)
        {
            var id = ValidateEncodedIdFormat(encodedId, nameof(encodedId)).Value;
            ValidateCommentContentFormat(content);
            var comment = await ValidateCommentExistsAsync(id);
            ValidateRequestingUserCanAlterComment(requestingUser, comment);
        }

        public async Task ValidateDeleteCommentByIdInputParametersAsync(User requestingUser,
            string encodedId)
        {
            var id = ValidateEncodedIdFormat(encodedId, nameof(encodedId)).Value;
            var comment = await ValidateCommentExistsAsync(id);
            ValidateRequestingUserCanAlterComment(requestingUser, comment);
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
