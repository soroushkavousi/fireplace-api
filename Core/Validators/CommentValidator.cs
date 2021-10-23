using FireplaceApi.Core.Enums;
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

        public CommentValidator(ILogger<CommentValidator> logger, IConfiguration configuration,
            IServiceProvider serviceProvider, CommentOperator commentOperator)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _commentOperator = commentOperator;
        }

        public async Task ValidateListSelfCommentsInputParametersAsync(User requesterUser,
            PaginationInputParameters paginationInputParameters, SortType? sort)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateListPostCommentsInputParametersAsync(User requesterUser,
            PaginationInputParameters paginationInputParameters, long postId,
            SortType? sort)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateListChildCommentsAsyncInputParametersAsync(
            User requesterUser, long parentCommentId)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateGetCommentByIdInputParametersAsync(User requesterUser,
            long? id, bool? includeAuthor, bool? includePost)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateReplyToPostInputParametersAsync(User requesterUser,
            long postId, string content)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateReplyToCommentInputParametersAsync(User requesterUser,
            long commentId, string content)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateVoteCommentInputParametersAsync(User requesterUser,
            long id, bool? isUpvote)
        {
            await Task.CompletedTask;
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
            long? id, string content)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateDeleteCommentByIdInputParametersAsync(User requesterUser,
            long? id)
        {
            await Task.CompletedTask;
        }


    }
}
