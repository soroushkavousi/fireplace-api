using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Identifiers;
using FireplaceApi.Domain.Interfaces;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Tools;
using FireplaceApi.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Operators;

public class PostOperator
{
    private readonly ILogger<PostOperator> _logger;
    private readonly IPostRepository _postRepository;
    private readonly IPostVoteRepository _postVoteRepository;
    private readonly UserOperator _userOperator;
    private readonly CommunityOperator _communityOperator;

    public PostOperator(ILogger<PostOperator> logger,
        IPostRepository postRepository,
        IPostVoteRepository postVoteRepository,
        UserOperator userOperator,
        CommunityOperator communityOperator)
    {
        _logger = logger;
        _postRepository = postRepository;
        _postVoteRepository = postVoteRepository;
        _userOperator = userOperator;
        _communityOperator = communityOperator;
    }

    public async Task<QueryResult<Post>> ListCommunityPostsAsync(CommunityIdentifier communityIdentifier,
        SortType? sort = null, ulong? userId = null)
    {
        sort ??= default;
        var communityPosts = await _postRepository.ListCommunityPostsAsync(
            communityIdentifier, sort.Value, userId);
        var queryResult = new QueryResult<Post>(communityPosts);
        return queryResult;
    }

    public async Task<QueryResult<Post>> ListPostsAsync(string search, SortType? sort, ulong? userId = null)
    {
        sort ??= default;
        var posts = await _postRepository.ListPostsAsync(search, sort.Value, userId);
        var queryResult = new QueryResult<Post>(posts);
        return queryResult;
    }

    public async Task<List<Post>> ListPostsByIdsAsync(List<ulong> ids, ulong? userId = null)
    {
        if (ids.IsNullOrEmpty())
            return null;

        var posts = await _postRepository
            .ListPostsByIdsAsync(ids, userId);
        return posts;
    }

    public async Task<QueryResult<Post>> ListSelfPostsAsync(ulong authorId,
        SortType? sort = null)
    {
        sort ??= default;
        var selfPosts = await _postRepository.ListSelfPostsAsync(
            authorId, sort.Value);

        var queryResult = new QueryResult<Post>(selfPosts);
        return queryResult;
    }

    public async Task<Post> GetPostByIdAsync(ulong id,
        bool includeAuthor, bool includeCommunity, ulong? userId = null)
    {
        var post = await _postRepository.GetPostByIdAsync(
            id, includeAuthor, includeCommunity, userId);
        if (post == null)
            return post;

        return post;
    }

    public async Task<Post> CreatePostAsync(ulong userId,
        CommunityIdentifier communityIdentifier, string content)
    {
        ulong communityId = default;
        string communityName = default;
        switch (communityIdentifier)
        {
            case CommunityIdIdentifier idIdentifier:
                communityId = idIdentifier.Id;
                communityName = await _communityOperator
                    .GetNameByIdAsync(communityId);
                break;
            case CommunityNameIdentifier nameIdentifier:
                communityName = nameIdentifier.Name;
                communityId = await _communityOperator
                    .GetIdByNameAsync(communityName);
                break;
        }
        var post = await CreatePostAsync(userId, communityId,
            communityName, content);
        return post;
    }

    public async Task<Post> CreatePostAsync(ulong userId,
        ulong communityId, string communityName, string content,
        string username = null)
    {
        var id = await IdGenerator.GenerateNewIdAsync(DoesPostIdExistAsync);
        username ??= await _userOperator.GetUsernameByIdAsync(userId);
        var post = await _postRepository.CreatePostAsync(
            id, userId, username,
            communityId, communityName, content);
        return post;
    }

    public async Task<Post> VotePostAsync(ulong userId,
        ulong id, bool isUp, string username = null)
    {
        var postVoteId = await IdGenerator.GenerateNewIdAsync(
            DoesPostVoteIdExistAsync);
        username ??= await _userOperator.GetUsernameByIdAsync(userId);
        var postVote = await _postVoteRepository.CreatePostVoteAsync(
            postVoteId, userId, username,
            id, isUp);
        var voteChange = postVote.IsUp ? +1 : -1;
        var post = await PatchPostByIdAsync(userId,
            id, null, voteChange: voteChange);
        return post;
    }

    public async Task<Post> ToggleVoteForPostAsync(ulong userId,
        ulong id)
    {
        var postVote = await _postVoteRepository.GetPostVoteAsync(
            userId, id, includePost: true);
        postVote.IsUp = !postVote.IsUp;
        await _postVoteRepository.UpdatePostVoteAsync(postVote);
        var voteChange = postVote.IsUp ? +2 : -2;
        var post = await ApplyPostChangesAsync(postVote.Post,
            null, voteChange: voteChange);
        post = await GetPostByIdAsync(post.Id,
            false, false, userId);
        return post;
    }

    public async Task<Post> DeleteVoteForPostAsync(ulong userId,
        ulong id)
    {
        var postVote = await _postVoteRepository.GetPostVoteAsync(
            userId, id, includePost: true);
        var voteChange = postVote.IsUp ? -1 : +1;
        await _postVoteRepository.DeletePostVoteByIdAsync(
            postVote.Id);
        var post = await ApplyPostChangesAsync(postVote.Post,
            null, voteChange: voteChange);
        return post;
    }

    public async Task<Post> PatchPostByIdAsync(ulong userId,
        ulong id, string content, int? voteChange)
    {
        var post = await _postRepository
            .GetPostByIdAsync(id);
        post = await ApplyPostChangesAsync(post, content,
            voteChange);
        post = await GetPostByIdAsync(post.Id,
            false, false, userId);
        return post;
    }

    public async Task DeletePostByIdAsync(ulong id)
    {
        await _postRepository.DeletePostByIdAsync(id);
    }

    public async Task<bool> DoesPostIdExistAsync(ulong id)
    {
        var postIdExists = await _postRepository
            .DoesPostIdExistAsync(id);
        return postIdExists;
    }

    public async Task<bool> DoesPostVoteIdExistAsync(ulong id)
    {
        var postVoteIdExists = await _postVoteRepository
            .DoesPostVoteIdExistAsync(id);
        return postVoteIdExists;
    }

    public async Task<Post> ApplyPostChangesAsync(
        Post post, string content, int? voteChange)
    {
        if (content != null)
        {
            post.Content = content;
        }

        if (voteChange.HasValue)
        {
            post.Vote += voteChange.Value;
        }

        post = await _postRepository
            .UpdatePostAsync(post);
        return post;
    }
}
