using FireplaceApi.Application.Comments;
using FireplaceApi.Domain.Comments;
using FireplaceApi.Domain.Errors;
using FireplaceApi.Domain.Users;
using FireplaceApi.Infrastructure.Converters;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace FireplaceApi.Infrastructure.Repositories;

public class CommentRepository : ICommentRepository, IRepository<ICommentRepository>
{
    private readonly ILogger<CommentRepository> _logger;
    private readonly ApiDbContext _dbContext;
    private readonly IIdGenerator _idGenerator;
    private readonly DbSet<CommentEntity> _commentEntities;

    public CommentRepository(ILogger<CommentRepository> logger, ApiDbContext dbContext,
        IIdGenerator idGenerator)
    {
        _logger = logger;
        _dbContext = dbContext;
        _idGenerator = idGenerator;
        _commentEntities = dbContext.CommentEntities;
    }

    public async Task<List<Comment>> ListPostCommentsAsync(ulong postId,
        CommentSortType sort, ulong? userId = null)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new
        {
            postId,
            sort,
            userId
        });
        var sw = Stopwatch.StartNew();
        var commentEntities = await _commentEntities
            .AsNoTracking()
            .Search(
                authorId: null,
                postId: postId,
                search: null,
                isRoot: true
            )
            .Include(
                authorEntity: false,
                postEntity: false,
                childCommentEntities: true,
                userId: userId,
                sort: sort
            )
            .Sort(sort)
            .Take(Configs.Current.QueryResult.TotalLimit)
            .ToListAsync();

        if (userId != null)
            commentEntities.ForEach(e => e.CheckRequestingUserVote(userId));

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT",
            parameters: new { commentEntities = commentEntities.Select(e => e.Id) });
        return commentEntities.Select(CommentConverter.ToModel).ToList();
    }

    public async Task<List<Comment>> ListChildCommentAsync(ulong id, CommentSortType sort,
        ulong? userId = null)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new
        {
            id,
            sort,
            userId
        });
        var sw = Stopwatch.StartNew();
        var commentEntity = await _commentEntities
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Include(
                authorEntity: false,
                postEntity: false,
                childCommentEntities: true,
                userId: userId,
                sort: sort
            )
            .SingleOrDefaultAsync();

        if (commentEntity != null && userId != null)
            commentEntity.CheckRequestingUserVote(userId);

        var childCommentEntities = commentEntity?.ChildCommentEntities;
        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT",
            parameters: new { childCommentEntities = childCommentEntities?.Select(e => e.Id) });
        return childCommentEntities.Select(CommentConverter.ToModel).ToList();
    }

    public async Task<List<Comment>> ListCommentsByIdsAsync(List<ulong> Ids,
        CommentSortType sort, ulong? userId = null)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT",
            parameters: new
            {
                Ids,
                sort,
                userId
            });
        var sw = Stopwatch.StartNew();
        var commentEntities = await _commentEntities
            .AsNoTracking()
            .Where(e => Ids.Contains(e.Id))
            .Include(
                authorEntity: false,
                postEntity: false,
                childCommentEntities: true,
                userId: userId,
                sort: sort
            )
            .ToListAsync();

        if (userId != null)
            commentEntities.ForEach(e => e.CheckRequestingUserVote(userId));

        // To preserve the order of input Ids
        var commentEntityDictionary = new Dictionary<ulong, CommentEntity>();
        commentEntities.ForEach(e => commentEntityDictionary[e.Id] = e);
        commentEntities = new List<CommentEntity>();
        Ids.ForEach(id => commentEntities.Add(commentEntityDictionary[id]));

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT",
            parameters: new { commentEntities = commentEntities.Select(e => e.Id) });
        return commentEntities.Select(CommentConverter.ToModel).ToList();
    }

    public async Task<List<Comment>> ListSelfCommentsAsync(ulong authorId,
        CommentSortType sort)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new
        {
            authorId,
            sort
        });
        var sw = Stopwatch.StartNew();
        var commentEntities = await _commentEntities
            .AsNoTracking()
            .Search(
                authorId: authorId,
                postId: null,
                search: null,
                isRoot: null
            )
            .Include(
                authorEntity: false,
                postEntity: false,
                childCommentEntities: false,
                userId: authorId,
                sort: sort
            )
            .Sort(sort)
            .Take(Configs.Current.QueryResult.TotalLimit)
            .ToListAsync();

        commentEntities.ForEach(e => e.CheckRequestingUserVote(authorId));

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT",
            parameters: new { commentEntities = commentEntities.Select(e => e.Id) });
        return commentEntities.Select(CommentConverter.ToModel).ToList();
    }

    public async Task<Comment> GetCommentByIdAsync(ulong id,
        bool includeAuthor = false, bool includePost = false,
        ulong? userId = null)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new
        {
            id,
            includeAuthor,
            includePost,
            userId
        });
        var sw = Stopwatch.StartNew();
        var commentEntity = await _commentEntities
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Include(
                authorEntity: includeAuthor,
                postEntity: includePost,
                childCommentEntities: false,
                userId: userId,
                sort: null
            )
            .SingleOrDefaultAsync();

        if (commentEntity != null && userId != null)
            commentEntity.CheckRequestingUserVote(userId);

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { commentEntity });
        return commentEntity.ToModel();
    }

    public async Task<Comment> CreateCommentAsync(ulong authorUserId,
        Username authorUsername, ulong postId, string content,
        ulong? parentCommentId = null)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new
        {
            authorUserId,
            authorUsername,
            postId,
            content,
            parentCommentId
        });
        var sw = Stopwatch.StartNew();
        var id = _idGenerator.GenerateNewId();
        var commentEntity = new CommentEntity(id, authorUserId,
            authorUsername.Value, postId, content, parentCommentId);
        _commentEntities.Add(commentEntity);
        await _dbContext.SaveChangesAsync();
        _dbContext.DetachAllEntries();

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT",
            parameters: new { commentEntity });
        return commentEntity.ToModel();
    }

    public async Task<Comment> UpdateCommentAsync(Comment comment)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new { comment });
        var sw = Stopwatch.StartNew();
        var commentEntity = comment.ToEntity();
        _commentEntities.Update(commentEntity);
        try
        {
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachAllEntries();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new InternalServerException("Can't update the commentEntity DbUpdateConcurrencyException!",
                parameters: commentEntity, systemException: ex);
        }

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { commentEntity });
        return commentEntity.ToModel();
    }

    public async Task DeleteCommentByIdAsync(ulong id)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new { id });
        var sw = Stopwatch.StartNew();
        var commentEntity = await _commentEntities
            .Where(e => e.Id == id)
            .SingleOrDefaultAsync();

        _commentEntities.Remove(commentEntity);
        await _dbContext.SaveChangesAsync();
        _dbContext.DetachAllEntries();

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { commentEntity });
    }

    public async Task<bool> DoesCommentIdExistAsync(ulong id)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new { id });
        var sw = Stopwatch.StartNew();
        var doesExist = await _commentEntities
            .AsNoTracking()
            .Where(e => e.Id == id)
            .AnyAsync();

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { doesExist });
        return doesExist;
    }

    public void InjectDependency(ServiceCollection services)
    {
        services.AddScoped<ICommentRepository, CommentRepository>();
    }
}

public static class CommentRepositoryExtensions
{
    public static IQueryable<CommentEntity> Include(
        [NotNull] this IQueryable<CommentEntity> q,
        bool authorEntity, bool postEntity, bool childCommentEntities,
        ulong? userId, CommentSortType? sort)
    {
        if (authorEntity)
            q = q.Include(e => e.AuthorEntity);

        if (postEntity)
            q = q.Include(e => e.PostEntity);

        if (childCommentEntities)
        {
            IIncludableQueryable<CommentEntity, IOrderedEnumerable<CommentEntity>> z = null;
            for (int i = 0; i < Configs.Current.QueryResult.DepthLimit; i++)
            {
                switch (sort)
                {
                    case CommentSortType.TOP:
                        z = q.Include(
                            e => e.ChildCommentEntities.OrderByDescending(e => e.Vote));
                        break;
                    case CommentSortType.NEW:
                        z = q.Include(
                            e => e.ChildCommentEntities.OrderByDescending(e => e.CreationDate));
                        break;
                    case CommentSortType.OLD:
                        z = q.Include(
                            e => e.ChildCommentEntities.OrderBy(e => e.CreationDate));
                        break;
                    default:
                        break;
                }

                for (int j = 0; j < i; j++)
                {
                    switch (sort)
                    {
                        case CommentSortType.TOP:
                            z = z.ThenInclude(
                                e => e.ChildCommentEntities.OrderByDescending(e => e.Vote));
                            break;
                        case CommentSortType.NEW:
                            z = z.ThenInclude(
                                e => e.ChildCommentEntities.OrderByDescending(e => e.CreationDate));
                            break;
                        case CommentSortType.OLD:
                            z = z.ThenInclude(
                                e => e.ChildCommentEntities.OrderBy(e => e.CreationDate));
                            break;
                        default:
                            break;
                    }
                }

                if (userId != null)
                {
                    q = z.ThenInclude(e => e.CommentVoteEntities
                                        .Where(cv => cv.VoterEntityId == userId));
                }
                else
                    q = z;
            }
        }

        if (userId != null)
        {
            q = q.Include(
                c => c.CommentVoteEntities
                    .Where(cv => cv.VoterEntityId == userId)
            );
        }

        return q;
    }

    public static IQueryable<CommentEntity> Search(
        [NotNull] this IQueryable<CommentEntity> q, ulong? authorId,
        ulong? postId, string search, bool? isRoot)
    {
        if (authorId.HasValue)
            q = q.Where(e => e.AuthorEntityId == authorId.Value);

        if (postId.HasValue)
            q = q.Where(e => e.PostEntityId == postId.Value);

        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(e => EF.Functions
                .ILike(EF.Functions.Collate(e.Content, "default"), $"%{search}%"));

        if (isRoot.HasValue && isRoot.Value)
            q = q.Where(e => e.ParentCommentEntityId == null);

        return q;
    }

    public static IQueryable<CommentEntity> Sort(
        [NotNull] this IQueryable<CommentEntity> q, CommentSortType sort)
    {
        q = sort switch
        {
            CommentSortType.TOP => q.OrderByDescending(e => e.Vote),
            CommentSortType.NEW => q.OrderByDescending(e => e.CreationDate),
            CommentSortType.OLD => q.OrderBy(e => e.CreationDate),
            _ => q.OrderByDescending(e => e.Vote),
        };

        return q;
    }
}
