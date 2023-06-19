using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Exceptions;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Identifiers;
using FireplaceApi.Domain.Interfaces;
using FireplaceApi.Domain.Models;
using FireplaceApi.Infrastructure.Converters;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace FireplaceApi.Infrastructure.Repositories;

public class PostRepository : IPostRepository
{
    private readonly ILogger<PostRepository> _logger;
    private readonly ProjectDbContext _dbContext;
    private readonly DbSet<PostEntity> _postEntities;

    public PostRepository(ILogger<PostRepository> logger, ProjectDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
        _postEntities = dbContext.PostEntities;
    }

    public async Task<List<Post>> ListCommunityPostsAsync(CommunityIdentifier communityIdentifier,
        SortType sort, ulong? userId = null)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new
        {
            communityIdentifier,
            sort,
            userId
        });
        var sw = Stopwatch.StartNew();
        var postEntities = await _postEntities
            .AsNoTracking()
            .Search(
                authorId: null,
                joined: null,
                communityIdentifier: communityIdentifier,
                search: null
            )
            .Include(
                authorEntity: false,
                communityEntity: false,
                userId: userId
            )
            .Sort(sort)
            .Take(Configs.Current.QueryResult.TotalLimit)
            .ToListAsync();

        if (userId != null)
            postEntities.ForEach(e => e.CheckRequestingUserVote(userId));

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT",
            parameters: new { postEntities = postEntities.Select(e => e.Id) });
        return postEntities.Select(PostConverter.ToModel).ToList();
    }

    public async Task<List<Post>> ListPostsAsync(string search, SortType sort,
        ulong? userId = null)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new
        {
            search,
            sort,
            userId
        });
        var sw = Stopwatch.StartNew();
        var postEntities = await _postEntities
            .AsNoTracking()
            .Search(
                authorId: null,
                joined: null,
                communityIdentifier: null,
                search: search
            )
            .Include(
                authorEntity: false,
                communityEntity: false,
                userId: userId
            )
            .Sort(sort)
            .Take(Configs.Current.QueryResult.TotalLimit)
            .ToListAsync();

        if (userId != null)
            postEntities.ForEach(e => e.CheckRequestingUserVote(userId));

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT",
            parameters: new { postEntities = postEntities.Select(e => e.Id) });
        return postEntities.Select(PostConverter.ToModel).ToList();
    }

    public async Task<List<Post>> ListPostsByIdsAsync(List<ulong> Ids,
        ulong? userId = null)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new
        {
            Ids,
            userId
        });
        var sw = Stopwatch.StartNew();
        var postEntities = await _postEntities
            .AsNoTracking()
            .Where(e => Ids.Contains(e.Id))
            .Include(
                authorEntity: false,
                communityEntity: false,
                userId: userId
            )
            .ToListAsync();

        var postEntityDictionary = new Dictionary<ulong, PostEntity>();
        postEntities.ForEach(e => postEntityDictionary[e.Id] = e);
        postEntities = new List<PostEntity>();
        Ids.ForEach(id => postEntities.Add(postEntityDictionary[id]));

        if (userId != null)
            postEntities.ForEach(e => e.CheckRequestingUserVote(userId));

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT",
            parameters: new { postEntities = postEntities.Select(e => e.Id) });
        return postEntities.Select(PostConverter.ToModel).ToList();
    }

    public async Task<List<Post>> ListSelfPostsAsync(ulong authorId,
        SortType sort)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new
        {
            authorId,
            sort
        });
        var sw = Stopwatch.StartNew();
        var postEntities = await _postEntities
            .AsNoTracking()
            .Search(
                authorId: authorId,
                joined: null,
                communityIdentifier: null,
                search: null
            )
            .Include(
                authorEntity: false,
                communityEntity: false,
                userId: authorId
            )
            .Sort(sort)
            .Take(Configs.Current.QueryResult.TotalLimit)
            .ToListAsync();

        postEntities.ForEach(e => e.CheckRequestingUserVote(authorId));

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT",
            parameters: new { postEntities = postEntities.Select(e => e.Id) });
        return postEntities.Select(PostConverter.ToModel).ToList();
    }

    public async Task<Post> GetPostByIdAsync(ulong id,
        bool includeAuthor = false, bool includeCommunity = false,
        ulong? userId = null)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new
        {
            id,
            includeAuthor,
            includeCommunity,
            userId = userId != null
        });
        var sw = Stopwatch.StartNew();
        var postEntity = await _postEntities
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Include(
                authorEntity: includeAuthor,
                communityEntity: includeCommunity,
                userId: userId
            )
            .SingleOrDefaultAsync();

        if (postEntity != null && userId != null)
            postEntity.CheckRequestingUserVote(userId);

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { postEntity });
        return postEntity.ToModel();
    }

    public async Task<Post> CreatePostAsync(ulong id, ulong authorUserId,
        string authorUsername, ulong communityId, string communityName,
        string content)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new
        {
            id,
            authorUserId,
            authorUsername,
            communityId,
            communityName,
            content
        });
        var sw = Stopwatch.StartNew();
        var postEntity = new PostEntity(id, authorUserId, authorUsername,
            communityId, communityName, content);
        _postEntities.Add(postEntity);
        await _dbContext.SaveChangesAsync();
        _dbContext.DetachAllEntries();

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT",
            parameters: new { postEntity });
        return postEntity.ToModel();
    }

    public async Task<Post> UpdatePostAsync(Post post)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { post });
        var sw = Stopwatch.StartNew();
        var postEntity = post.ToEntity();
        _postEntities.Update(postEntity);
        try
        {
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachAllEntries();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new InternalServerException("Can't update the postEntity DbUpdateConcurrencyException!",
                parameters: postEntity, systemException: ex);
        }

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { postEntity });
        return PostConverter.ToModel(postEntity);
    }

    public async Task DeletePostByIdAsync(ulong id)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { id });
        var sw = Stopwatch.StartNew();
        var postEntity = await _postEntities
            .Where(e => e.Id == id)
            .SingleOrDefaultAsync();

        _postEntities.Remove(postEntity);
        await _dbContext.SaveChangesAsync();
        _dbContext.DetachAllEntries();

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { postEntity });
    }

    public async Task<bool> DoesPostIdExistAsync(ulong id)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { id });
        var sw = Stopwatch.StartNew();
        var doesExist = await _postEntities
            .AsNoTracking()
            .Where(e => e.Id == id)
            .AnyAsync();

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { doesExist });
        return doesExist;
    }
}

public static class PostRepositoryExtensions
{
    public static IQueryable<PostEntity> Include(
        [NotNull] this IQueryable<PostEntity> q,
        bool authorEntity, bool communityEntity,
        ulong? userId)
    {
        if (authorEntity)
            q = q.Include(e => e.AuthorEntity);

        if (communityEntity)
            q = q.Include(e => e.CommunityEntity);

        if (userId != null)
        {
            q = q.Include(
                c => c.PostVoteEntities
                    .Where(cv => cv.VoterEntityId == userId)
            );
        }

        return q;
    }

    public static IQueryable<PostEntity> Search(
        [NotNull] this IQueryable<PostEntity> q, bool? joined,
        ulong? authorId, CommunityIdentifier communityIdentifier,
        string search)
    {
        if (authorId.HasValue)
            q = q.Where(e => e.AuthorEntityId == authorId.Value);

        if (communityIdentifier != null)
        {
            switch (communityIdentifier)
            {
                case CommunityIdIdentifier idIdentifier:
                    q = q.Where(e => e.CommunityEntityId == idIdentifier.Id);
                    break;
                case CommunityNameIdentifier nameIdentifier:
                    q = q.Where(e => e.CommunityEntityName == nameIdentifier.Name);
                    break;
            }
        }

        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(e => EF.Functions
                .ILike(EF.Functions.Collate(e.Content, "default"), $"%{search}%"));

        if (joined.HasValue)
            q = q.Where(
                e => e.CommunityEntity.CommunityMemberEntities.Select(jc => jc.UserEntityId)
                    .Contains(e.CommunityEntityId));

        return q;
    }

    public static IQueryable<PostEntity> Sort(
        [NotNull] this IQueryable<PostEntity> q, SortType sort)
    {
        q = sort switch
        {
            SortType.TOP => q.OrderByDescending(e => e.Vote),
            SortType.NEW => q.OrderByDescending(e => e.CreationDate),
            SortType.OLD => q.OrderBy(e => e.CreationDate),
            _ => q.OrderByDescending(e => e.Vote),
        };

        return q;
    }
}
