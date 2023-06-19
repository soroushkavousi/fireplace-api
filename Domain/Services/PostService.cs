using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Identifiers;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Operators;
using FireplaceApi.Domain.Validators;
using FireplaceApi.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Services;

public class PostService
{
    private readonly ILogger<PostService> _logger;
    private readonly PostValidator _postValidator;
    private readonly PostOperator _postOperator;

    public PostService(ILogger<PostService> logger,
        PostValidator postValidator, PostOperator postOperator)
    {
        _logger = logger;
        _postValidator = postValidator;
        _postOperator = postOperator;
    }

    public async Task<QueryResult<Post>> ListCommunityPostsAsync(CommunityIdentifier communityIdentifier,
        SortType? sort, ulong? userId = null)
    {
        await _postValidator.ValidateListCommunityPostsInputParametersAsync(
            communityIdentifier, sort, userId);
        return await _postOperator.ListCommunityPostsAsync(communityIdentifier,
            sort, userId);
    }

    public async Task<QueryResult<Post>> ListPostsAsync(string search, SortType? sort, ulong? userId = null)
    {
        await _postValidator.ValidateListPostsInputParametersAsync(search, sort, userId);
        return await _postOperator.ListPostsAsync(search, sort, userId);
    }

    public async Task<List<Post>> ListPostsByIdsAsync(List<ulong> ids, ulong? userId = null)
    {
        await _postValidator.ValidateListPostsByIdsInputParametersAsync(ids, userId);
        var communities = await _postOperator.ListPostsByIdsAsync(ids, userId);
        return communities;
    }

    public async Task<QueryResult<Post>> ListSelfPostsAsync(ulong userId,
        SortType? sort)
    {
        await _postValidator.ValidateListSelfPostsInputParametersAsync(userId, sort);
        var queryResult = await _postOperator.ListSelfPostsAsync(userId, sort);
        return queryResult;
    }

    public async Task<Post> GetPostByIdAsync(ulong id, bool? includeAuthor,
        bool? includeCommunity, ulong? userId = null)
    {
        await _postValidator.ValidateGetPostByIdInputParametersAsync(
            id, includeAuthor, includeCommunity, userId);
        var post = await _postOperator.GetPostByIdAsync(id,
            includeAuthor.Value, includeCommunity.Value, userId);
        return post;
    }

    public async Task<Post> CreatePostAsync(ulong userId,
        CommunityIdentifier communityIdentifier, string content)
    {
        await _postValidator.ValidateCreatePostInputParametersAsync(
                userId, communityIdentifier, content);
        return await _postOperator.CreatePostAsync(userId,
            communityIdentifier, content);
    }

    public async Task<Post> VotePostAsync(ulong userId,
        ulong id, bool isUpvote)
    {
        await _postValidator.ValidateVotePostInputParametersAsync(
            userId, id, isUpvote);
        var post = await _postOperator.VotePostAsync(
            userId, id, isUpvote);
        return post;
    }

    public async Task<Post> ToggleVoteForPostAsync(ulong userId,
        ulong id)
    {
        await _postValidator.ValidateToggleVoteForPostInputParametersAsync(
            userId, id);
        var post = await _postOperator.ToggleVoteForPostAsync(
            userId, id);
        return post;
    }

    public async Task<Post> DeleteVoteForPostAsync(ulong userId,
        ulong id)
    {
        await _postValidator.ValidateDeleteVoteForPostInputParametersAsync(
            userId, id);
        var post = await _postOperator.DeleteVoteForPostAsync(
            userId, id);
        return post;
    }

    public async Task<Post> PatchPostByIdAsync(ulong userId,
        ulong id, string content)
    {
        await _postValidator.ValidatePatchPostByIdInputParametersAsync(
            userId, id, content);
        var post = await _postOperator.PatchPostByIdAsync(userId,
            id, content, null);
        return post;
    }

    public async Task DeletePostByIdAsync(ulong userId, ulong id)
    {
        await _postValidator.ValidateDeletePostByIdInputParametersAsync(
            userId, id);
        await _postOperator.DeletePostByIdAsync(id);
    }
}
