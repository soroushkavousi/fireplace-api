using FireplaceApi.Domain.Communities;
using FireplaceApi.Domain.Errors;
using FireplaceApi.Domain.Posts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Posts;

public class PostValidator : ApplicationValidator
{
    private readonly IServerLogger<PostValidator> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly PostOperator _postOperator;

    public Post Post { get; private set; }

    public PostValidator(IServerLogger<PostValidator> logger,
        IServiceProvider serviceProvider, PostOperator postOperator)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _postOperator = postOperator;
    }

    public async Task ValidateListCommunityPostsInputParametersAsync(CommunityIdentifier communityIdentifier,
        PostSortType? sort, ulong? userId)
    {
        //await _communityValidator.ValidateCommunityExistsAsync(communityIdentifier);
    }

    public async Task ValidateListPostsInputParametersAsync(string search, PostSortType? sort, ulong? userId)
    {
        await Task.CompletedTask;
    }

    public async Task ValidateListPostsByIdsInputParametersAsync(List<ulong> ids, ulong? userId)
    {
        await Task.CompletedTask;
    }

    public async Task ValidateListSelfPostsInputParametersAsync(ulong userId,
        PostSortType? sort)
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
        //await _communityValidator.ValidateCommunityExistsAsync(communityIdentifier);
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
