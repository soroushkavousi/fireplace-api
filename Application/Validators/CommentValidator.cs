using FireplaceApi.Application.Enums;
using FireplaceApi.Application.Exceptions;
using FireplaceApi.Application.Models;
using FireplaceApi.Application.Operators;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Validators;

public class CommentValidator : ApplicationValidator
{
    private readonly ILogger<CommentValidator> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly CommentOperator _commentOperator;
    private readonly PostValidator _postValidator;

    public Comment Comment { get; private set; }
    public Comment ParentComment { get; private set; }
    public Post Post { get; private set; }

    public CommentValidator(ILogger<CommentValidator> logger,
        IServiceProvider serviceProvider, CommentOperator commentOperator,
        PostValidator postValidator)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _commentOperator = commentOperator;
        _postValidator = postValidator;
    }

    public async Task ValidateListPostCommentsInputParametersAsync(ulong postId,
        SortType? sort, ulong? userId)
    {
        Post = await _postValidator.ValidatePostExistsAsync(postId);
    }

    public async Task ValidateListCommentsByIdsInputParametersAsync(List<ulong> ids,
        SortType? sort, ulong? userId)
    {
        await Task.CompletedTask;
    }

    public async Task ValidateListSelfCommentsInputParametersAsync(ulong userId,
        SortType? sort)
    {
        await Task.CompletedTask;
    }

    public async Task ValidateListChildCommentsAsyncInputParametersAsync(
        ulong parentCommentId, SortType? sort, ulong? userId)
    {
        ParentComment = await ValidateCommentExistsAsync(parentCommentId);
    }

    public async Task ValidateGetCommentByIdInputParametersAsync(ulong id,
        bool? includeAuthor, bool? includePost, ulong? userId)
    {
        Comment = await ValidateCommentExistsAsync(id);
    }

    public async Task ValidateReplyToPostInputParametersAsync(ulong userId,
        ulong postId, string content)
    {
        Post = await _postValidator.ValidatePostExistsAsync(postId);
    }

    public async Task ValidateReplyToCommentInputParametersAsync(ulong userId,
        ulong parentCommentId, string content)
    {
        ParentComment = await ValidateCommentExistsAsync(parentCommentId);
    }

    public async Task ValidateVoteCommentInputParametersAsync(ulong userId,
        ulong id, bool? isUpvote)
    {
        Comment = await ValidateCommentExistsAsync(id, userId);
        ValidateCommentIsNotVotedByUser(Comment, userId);
    }

    public async Task ValidateToggleVoteForCommentInputParametersAsync(ulong userId,
        ulong id)
    {
        Comment = await ValidateCommentExistsAsync(id, userId);
        ValidateCommentVoteExists(Comment, userId);
    }

    public async Task ValidateDeleteVoteForCommentInputParametersAsync(ulong userId,
        ulong id)
    {
        Comment = await ValidateCommentExistsAsync(id, userId);
        ValidateCommentVoteExists(Comment, userId);
    }

    public async Task ValidatePatchCommentByIdInputParametersAsync(ulong userId,
        ulong id, string content)
    {
        Comment = await ValidateCommentExistsAsync(id);
        ValidateRequestingUserCanAlterComment(userId, Comment);
    }

    public async Task ValidateDeleteCommentByIdInputParametersAsync(ulong userId,
        ulong id)
    {
        Comment = await ValidateCommentExistsAsync(id);
        ValidateRequestingUserCanAlterComment(userId, Comment);
    }

    public void ValidateCommentIsNotVotedByUser(Comment comment, ulong userId)
    {
        if (comment.RequestingUserVote != 0)
            throw new CommentVoteAlreadyExistsException(userId, comment.Id);
    }

    public void ValidateCommentVoteExists(Comment comment, ulong userId)
    {
        if (comment.RequestingUserVote == 0)
            throw new CommentVoteNotExistException(userId, comment.Id);
    }

    public async Task<Comment> ValidateCommentExistsAsync(ulong id,
        ulong? userId = null)
    {
        var comment = await _commentOperator.GetCommentByIdAsync(id, true,
            true, userId);

        if (comment == null)
            throw new CommentNotExistException(id);

        return comment;
    }

    public void ValidateCommentContentFormat(string content)
    {
        var maximumLength = 3000;
        if (content.Length > maximumLength)
            throw new CommentContentInvalidException(content);
    }

    public void ValidateRequestingUserCanAlterComment(ulong userId, Comment comment)
    {
        if (userId != comment.AuthorId)
            throw new CommentAccessDeniedException(userId, comment.Id);
    }
}
