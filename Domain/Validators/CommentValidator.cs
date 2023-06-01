using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Exceptions;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Operators;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Validators;

public class CommentValidator : DomainValidator
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
        SortType? sort, User requestingUser)
    {
        Post = await _postValidator.ValidatePostExistsAsync(postId);
    }

    public async Task ValidateListCommentsByIdsInputParametersAsync(List<ulong> ids,
        SortType? sort, User requestingUser)
    {
        await Task.CompletedTask;
    }

    public async Task ValidateListSelfCommentsInputParametersAsync(User requestingUser,
        SortType? sort)
    {
        await Task.CompletedTask;
    }

    public async Task ValidateListChildCommentsAsyncInputParametersAsync(
        ulong parentCommentId, SortType? sort, User requestingUser)
    {
        ParentComment = await ValidateCommentExistsAsync(parentCommentId);
    }

    public async Task ValidateGetCommentByIdInputParametersAsync(ulong id,
        bool? includeAuthor, bool? includePost, User requestingUser)
    {
        Comment = await ValidateCommentExistsAsync(id);
    }

    public async Task ValidateReplyToPostInputParametersAsync(User requestingUser,
        ulong postId, string content)
    {
        Post = await _postValidator.ValidatePostExistsAsync(postId);
    }

    public async Task ValidateReplyToCommentInputParametersAsync(User requestingUser,
        ulong parentCommentId, string content)
    {
        ParentComment = await ValidateCommentExistsAsync(parentCommentId);
    }

    public async Task ValidateVoteCommentInputParametersAsync(User requestingUser,
        ulong id, bool? isUpvote)
    {
        Comment = await ValidateCommentExistsAsync(id, requestingUser);
        ValidateCommentIsNotVotedByUser(Comment, requestingUser);
    }

    public async Task ValidateToggleVoteForCommentInputParametersAsync(User requestingUser,
        ulong id)
    {
        Comment = await ValidateCommentExistsAsync(id, requestingUser);
        ValidateCommentVoteExists(Comment, requestingUser);
    }

    public async Task ValidateDeleteVoteForCommentInputParametersAsync(User requestingUser,
        ulong id)
    {
        Comment = await ValidateCommentExistsAsync(id, requestingUser);
        ValidateCommentVoteExists(Comment, requestingUser);
    }

    public async Task ValidatePatchCommentByIdInputParametersAsync(User requestingUser,
        ulong id, string content)
    {
        Comment = await ValidateCommentExistsAsync(id);
        ValidateRequestingUserCanAlterComment(requestingUser, Comment);
    }

    public async Task ValidateDeleteCommentByIdInputParametersAsync(User requestingUser,
        ulong id)
    {
        Comment = await ValidateCommentExistsAsync(id);
        ValidateRequestingUserCanAlterComment(requestingUser, Comment);
    }

    public void ValidateCommentIsNotVotedByUser(Comment comment, User requestingUser)
    {
        if (comment.RequestingUserVote != 0)
            throw new CommentVoteAlreadyExistsException(requestingUser.Id, comment.Id);
    }

    public void ValidateCommentVoteExists(Comment comment, User requestingUser)
    {
        if (comment.RequestingUserVote == 0)
            throw new CommentVoteNotExistException(requestingUser.Id, comment.Id);
    }

    public async Task<Comment> ValidateCommentExistsAsync(ulong id,
        User requestingUser = null)
    {
        var comment = await _commentOperator.GetCommentByIdAsync(id, true,
            true, requestingUser);

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

    public void ValidateRequestingUserCanAlterComment(User requestingUser, Comment comment)
    {
        if (requestingUser.Id != comment.AuthorId)
            throw new CommentAccessDeniedException(requestingUser.Id, comment.Id);
    }
}
