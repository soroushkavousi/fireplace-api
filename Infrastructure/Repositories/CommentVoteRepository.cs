using FireplaceApi.Application.Comments;
using FireplaceApi.Domain.Comments;
using FireplaceApi.Domain.Errors;
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

public class CommentVoteRepository : ICommentVoteRepository
{
    private readonly ILogger<CommentVoteRepository> _logger;
    private readonly ProjectDbContext _dbContext;
    private readonly DbSet<CommentVoteEntity> _commentVoteEntities;

    public CommentVoteRepository(ILogger<CommentVoteRepository> logger,
        ProjectDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
        _commentVoteEntities = dbContext.CommentVoteEntities;
    }

    public async Task<List<CommentVote>> ListCommentVotesAsync(List<ulong> Ids)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { Ids });
        var sw = Stopwatch.StartNew();
        var commentVoteEntities = await _commentVoteEntities
            .AsNoTracking()
            .Where(e => Ids.Contains(e.Id))
            .ToListAsync();

        var commentVoteEntityDictionary = new Dictionary<ulong, CommentVoteEntity>();
        commentVoteEntities.ForEach(e => commentVoteEntityDictionary[e.Id] = e);
        commentVoteEntities = new List<CommentVoteEntity>();
        Ids.ForEach(id => commentVoteEntities.Add(commentVoteEntityDictionary[id]));

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT",
            parameters: new { commentEntities = commentVoteEntities.Select(e => e.Id) });
        return commentVoteEntities.Select(CommentVoteConverter.ToModel).ToList();
    }

    public async Task<CommentVote> GetCommentVoteByIdAsync(ulong id,
        bool includeVoter = false, bool includeComment = false)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new
        {
            id,
            includeVoter,
            includeComment
        });
        var sw = Stopwatch.StartNew();
        var commentVoteEntity = await _commentVoteEntities
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Include(
                voterEntity: includeVoter,
                commentEntity: includeComment
            )
            .SingleOrDefaultAsync();

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { commentVoteEntity });
        return commentVoteEntity.ToModel();
    }

    public async Task<CommentVote> GetCommentVoteAsync(ulong voterId,
        ulong commentId, bool includeVoter = false, bool includeComment = false)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new
        {
            voterId,
            commentId,
            includeVoter,
            includeComment
        });
        var sw = Stopwatch.StartNew();
        var commentVoteEntity = await _commentVoteEntities
            .AsNoTracking()
            .Where(e => e.VoterEntityId == voterId
                && e.CommentEntityId == commentId)
            .Include(
                voterEntity: includeVoter,
                commentEntity: includeComment
            )
            .SingleOrDefaultAsync();

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { commentVoteEntity });
        return commentVoteEntity.ToModel();
    }

    public async Task<CommentVote> CreateCommentVoteAsync(ulong id, ulong voterUserId,
        string voterUsername, ulong commentId, bool isUp)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new
        {
            id,
            voterUserId,
            voterUsername,
            commentId,
            isUp
        });
        var sw = Stopwatch.StartNew();
        var commentVoteEntity = new CommentVoteEntity(id, voterUserId,
            voterUsername, commentId, isUp);
        _commentVoteEntities.Add(commentVoteEntity);
        await _dbContext.SaveChangesAsync();
        _dbContext.DetachAllEntries();

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT",
            parameters: new { commentVoteEntity });
        return commentVoteEntity.ToModel();
    }

    public async Task<CommentVote> UpdateCommentVoteAsync(CommentVote commentvote)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { commentvote });
        var sw = Stopwatch.StartNew();
        var commentVoteEntity = commentvote.ToEntity();
        _commentVoteEntities.Update(commentVoteEntity);
        try
        {
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachAllEntries();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new InternalServerException("Can't update the commentVoteEntity DbUpdateConcurrencyException!",
                parameters: commentVoteEntity, systemException: ex);
        }

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { commentVoteEntity });
        return commentVoteEntity.ToModel();
    }

    public async Task DeleteCommentVoteByIdAsync(ulong id)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { id });
        var sw = Stopwatch.StartNew();
        var commentVoteEntity = await _commentVoteEntities
            .Where(e => e.Id == id)
            .SingleOrDefaultAsync();

        _commentVoteEntities.Remove(commentVoteEntity);
        await _dbContext.SaveChangesAsync();
        _dbContext.DetachAllEntries();

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { commentVoteEntity });
    }

    public async Task<bool> DoesCommentVoteIdExistAsync(ulong id)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { id });
        var sw = Stopwatch.StartNew();
        var doesExist = await _commentVoteEntities
            .AsNoTracking()
            .Where(e => e.Id == id)
            .AnyAsync();

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { doesExist });
        return doesExist;
    }

    public async Task<bool> DoesCommentVoteIdExistAsync(ulong voterId, ulong commentId)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { voterId, commentId });
        var sw = Stopwatch.StartNew();
        var doesExist = await _commentVoteEntities
            .AsNoTracking()
            .Where(e => e.VoterEntityId == voterId
                && e.CommentEntityId == commentId)
            .AnyAsync();

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { doesExist });
        return doesExist;
    }
}

public static class CommentVoteRepositoryExtensions
{
    public static IQueryable<CommentVoteEntity> Include(
        [NotNull] this IQueryable<CommentVoteEntity> q,
        bool voterEntity, bool commentEntity)
    {
        if (voterEntity)
            q = q.Include(e => e.VoterEntity);

        if (commentEntity)
            q = q.Include(e => e.CommentEntity);

        return q;
    }

    public static IQueryable<CommentVoteEntity> Search(
        [NotNull] this IQueryable<CommentVoteEntity> q,
        ulong? voterId, ulong? commentId)
    {
        if (voterId.HasValue)
            q = q.Where(e => e.VoterEntityId == voterId.Value);

        if (commentId.HasValue)
            q = q.Where(e => e.CommentEntityId == commentId.Value);

        q = q.OrderByDescending(e => e.CreationDate);

        return q;
    }
}
