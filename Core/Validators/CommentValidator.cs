using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.ValueObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Validators
{
    public class CommentValidator : ApiValidator
    {
        private readonly ILogger<CommentValidator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly CommentOperator _commentOperator;
        private readonly QueryResultValidator _queryResultValidator;
        private readonly PostValidator _postValidator;

        public CommentValidator(ILogger<CommentValidator> logger, IConfiguration configuration,
            IServiceProvider serviceProvider, CommentOperator commentOperator,
            QueryResultValidator queryResultValidator, PostValidator postValidator)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _commentOperator = commentOperator;
            _queryResultValidator = queryResultValidator;
            _postValidator = postValidator;
        }

        public async Task ValidateListSelfCommentsInputParametersAsync(User requesterUser,
            PaginationInputParameters paginationInputParameters, SortType? sort,
            string stringOfSort)
        {
            await _queryResultValidator.ValidatePaginationInputParameters(paginationInputParameters,
                ModelName.COMMENT);

            ValidateInputEnum(sort, stringOfSort, nameof(sort), ErrorName.INPUT_SORT_IS_NOT_VALID);
        }

        public async Task ValidateListPostCommentsInputParametersAsync(User requesterUser,
            PaginationInputParameters paginationInputParameters, long postId,
            SortType? sort, string stringOfSort)
        {
            await _queryResultValidator.ValidatePaginationInputParameters(paginationInputParameters,
                ModelName.COMMENT);

            ValidateInputEnum(sort, stringOfSort, nameof(sort), ErrorName.INPUT_SORT_IS_NOT_VALID);
            var post = await _postValidator.ValidatePostExistsAsync(postId);
        }

        public async Task ValidateListChildCommentsAsyncInputParametersAsync(
            User requesterUser, long parentCommentId)
        {
            var parentComment = await ValidateCommentExists(parentCommentId);
        }

        public async Task ValidateGetCommentByIdInputParametersAsync(User requesterUser,
            long id, bool? includeAuthor, bool? includePost)
        {
            var comment = await ValidateCommentExists(id);
        }

        public async Task ValidateReplyToPostInputParametersAsync(User requesterUser,
            long postId, string content)
        {
            ValidateCommentContentFormat(content);
            var post = await _postValidator.ValidatePostExistsAsync(postId);
        }

        public async Task ValidateReplyToCommentInputParametersAsync(User requesterUser,
            long parentCommentId, string content)
        {
            ValidateCommentContentFormat(content);
            var parentComment = await ValidateCommentExists(parentCommentId);
        }

        public async Task ValidateVoteCommentInputParametersAsync(User requesterUser,
            long id, bool? isUpvote)
        {
            var comment = await ValidateCommentExists(id);
        }

        public async Task ValidateToggleVoteForCommentInputParametersAsync(User requesterUser,
            long id)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateDeleteVoteForCommentInputParametersAsync(User requesterUser,
            long id)
        {
            await Task.CompletedTask;
        }

        public async Task ValidatePatchCommentByIdInputParametersAsync(User requesterUser,
            long id, string content)
        {
            ValidateCommentContentFormat(content);
            var comment = await ValidateCommentExists(id);
            ValidateRequesterUserCanAlterComment(requesterUser, comment);
        }

        public async Task ValidateDeleteCommentByIdInputParametersAsync(User requesterUser,
            long id)
        {
            var comment = await ValidateCommentExists(id);
            ValidateRequesterUserCanAlterComment(requesterUser, comment);
        }

        public async Task<Comment> ValidateCommentExists(long id)
        {
            var comment = await _commentOperator.GetCommentByIdAsync(id, true, true);
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

        public void ValidateRequesterUserCanAlterComment(User requesterUser, Comment comment)
        {
            if (requesterUser.Id != comment.AuthorId)
            {
                var serverMessage = $"requesterUser {requesterUser.Id} can't alter the comment {comment.Id}";
                throw new ApiException(ErrorName.USER_CAN_NOT_ALTER_COMMENT, serverMessage);
            }
        }
    }
}
