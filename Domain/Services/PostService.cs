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
        SortType? sort, User requestingUser = null)
    {
        await _postValidator.ValidateListCommunityPostsInputParametersAsync(
            communityIdentifier, sort, requestingUser);
        return await _postOperator.ListCommunityPostsAsync(communityIdentifier,
            sort, requestingUser);
    }

    public async Task<QueryResult<Post>> ListPostsAsync(string search, SortType? sort, User requestingUser = null)
    {
        await _postValidator.ValidateListPostsInputParametersAsync(search, sort, requestingUser);
        return await _postOperator.ListPostsAsync(search, sort, requestingUser);
    }

    public async Task<List<Post>> ListPostsByIdsAsync(List<ulong> ids, User requestingUser = null)
    {
        await _postValidator.ValidateListPostsByIdsInputParametersAsync(ids, requestingUser);
        var communities = await _postOperator.ListPostsByIdsAsync(ids, requestingUser);
        return communities;
    }

    public async Task<QueryResult<Post>> ListSelfPostsAsync(User requestingUser,
        SortType? sort)
    {
        await _postValidator.ValidateListSelfPostsInputParametersAsync(requestingUser, sort);
        var queryResult = await _postOperator.ListSelfPostsAsync(requestingUser, sort);
        return queryResult;
    }

    public async Task<Post> GetPostByIdAsync(ulong id, bool? includeAuthor,
        bool? includeCommunity, User requestingUser = null)
    {
        await _postValidator.ValidateGetPostByIdInputParametersAsync(
            id, includeAuthor, includeCommunity, requestingUser);
        var post = await _postOperator.GetPostByIdAsync(id,
            includeAuthor.Value, includeCommunity.Value, requestingUser);
        return post;
    }

    public async Task<Post> CreatePostAsync(User requestingUser,
        CommunityIdentifier communityIdentifier, string content)
    {
        await _postValidator.ValidateCreatePostInputParametersAsync(
                requestingUser, communityIdentifier, content);
        return await _postOperator.CreatePostAsync(requestingUser,
            communityIdentifier, content);
    }

    public async Task<Post> VotePostAsync(User requestingUser,
        ulong id, bool isUpvote)
    {
        await _postValidator.ValidateVotePostInputParametersAsync(
            requestingUser, id, isUpvote);
        var post = await _postOperator.VotePostAsync(
            requestingUser, id, isUpvote);
        return post;
    }

    public async Task<Post> ToggleVoteForPostAsync(User requestingUser,
        ulong id)
    {
        await _postValidator.ValidateToggleVoteForPostInputParametersAsync(
            requestingUser, id);
        var post = await _postOperator.ToggleVoteForPostAsync(
            requestingUser, id);
        return post;
    }

    public async Task<Post> DeleteVoteForPostAsync(User requestingUser,
        ulong id)
    {
        await _postValidator.ValidateDeleteVoteForPostInputParametersAsync(
            requestingUser, id);
        var post = await _postOperator.DeleteVoteForPostAsync(
            requestingUser, id);
        return post;
    }

    public async Task<Post> PatchPostByIdAsync(User requestingUser,
        ulong id, string content)
    {
        await _postValidator.ValidatePatchPostByIdInputParametersAsync(
            requestingUser, id, content);
        var post = await _postOperator.PatchPostByIdAsync(requestingUser,
            id, content, null);
        return post;
    }

    public async Task DeletePostByIdAsync(User requestingUser, ulong id)
    {
        await _postValidator.ValidateDeletePostByIdInputParametersAsync(
            requestingUser, id);
        await _postOperator.DeletePostByIdAsync(id);
    }
}
