﻿using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Exceptions;
using FireplaceApi.Domain.Identifiers;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Operators;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Validators;

public class PostValidator : DomainValidator
{
    private readonly ILogger<PostValidator> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly PostOperator _postOperator;
    private readonly CommunityValidator _communityValidator;

    public Post Post { get; private set; }

    public PostValidator(ILogger<PostValidator> logger,
        IServiceProvider serviceProvider, PostOperator postOperator,
        CommunityValidator communityValidator)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _postOperator = postOperator;
        _communityValidator = communityValidator;
    }

    public async Task ValidateListCommunityPostsInputParametersAsync(CommunityIdentifier communityIdentifier,
        SortType? sort, ulong? userId)
    {
        await _communityValidator.ValidateCommunityIdentifierExistsAsync(communityIdentifier);
    }

    public async Task ValidateListPostsInputParametersAsync(string search, SortType? sort, ulong? userId)
    {
        await Task.CompletedTask;
    }

    public async Task ValidateListPostsByIdsInputParametersAsync(List<ulong> ids, ulong? userId)
    {
        await Task.CompletedTask;
    }

    public async Task ValidateListSelfPostsInputParametersAsync(ulong userId,
        SortType? sort)
    {
        await Task.CompletedTask;
    }

    public async Task ValidateGetPostByIdInputParametersAsync(ulong id,
        bool? includeAuthor, bool? includeCommunity, ulong? userId = null)
    {
        Post = await ValidatePostExistsAsync(id);
    }

    public async Task ValidateCreatePostInputParametersAsync(
        ulong userId, CommunityIdentifier communityIdentifier, string content)
    {
        await _communityValidator.ValidateCommunityIdentifierExistsAsync(communityIdentifier);
    }

    public async Task ValidateVotePostInputParametersAsync(ulong userId,
        ulong id, bool isUpvote)
    {
        Post = await ValidatePostExistsAsync(id, userId);
        ValidatePostIsNotVotedByUser(Post, userId);
    }

    public async Task ValidateToggleVoteForPostInputParametersAsync(ulong userId,
        ulong id)
    {
        Post = await ValidatePostExistsAsync(id, userId);
        ValidatePostVoteExists(Post, userId);
    }

    public async Task ValidateDeleteVoteForPostInputParametersAsync(ulong userId,
        ulong id)
    {
        Post = await ValidatePostExistsAsync(id, userId);
        ValidatePostVoteExists(Post, userId);
    }

    public async Task ValidatePatchPostByIdInputParametersAsync(ulong userId,
        ulong id, string content)
    {
        Post = await ValidatePostExistsAsync(id);
        ValidateRequestingUserCanAlterPost(userId, Post);
    }

    public async Task ValidateDeletePostByIdInputParametersAsync(ulong userId,
        ulong id)
    {
        Post = await ValidatePostExistsAsync(id);
        ValidateRequestingUserCanAlterPost(userId, Post);
    }

    public async Task<Post> ValidatePostExistsAsync(ulong id, ulong? userId = null)
    {
        var post = await _postOperator.GetPostByIdAsync(id, true, true,
            userId);

        if (post == null)
            throw new PostNotExistException(id);

        return post;
    }

    public void ValidatePostContentFormat(string content)
    {
        var maximumLength = 2000;
        if (content.Length > maximumLength)
            throw new PostContentInvalidFormatException(content);
    }

    public void ValidateRequestingUserCanAlterPost(ulong userId,
        Post post)
    {
        if (userId != post.AuthorId)
            throw new PostAccessDeniedException(userId, post.Id);
    }

    public void ValidatePostIsNotVotedByUser(Post post, ulong userId)
    {
        if (post.RequestingUserVote != 0)
            throw new PostVoteAlreadyExistsException(userId, post.Id);
    }

    public void ValidatePostVoteExists(Post post, ulong userId)
    {
        if (post.RequestingUserVote == 0)
            throw new PostVoteNotExistException(userId, post.Id);
    }
}
