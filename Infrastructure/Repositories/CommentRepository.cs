using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Tools;
using FireplaceApi.Infrastructure.Converters;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace FireplaceApi.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ILogger<CommentRepository> _logger;
        private readonly FireplaceApiDbContext _dbContext;
        private readonly DbSet<CommentEntity> _commentEntities;
        private readonly CommentConverter _commentConverter;

        public CommentRepository(ILogger<CommentRepository> logger,
            FireplaceApiDbContext dbContext, CommentConverter commentConverter)
        {
            _logger = logger;
            _dbContext = dbContext;
            _commentEntities = dbContext.CommentEntities;
            _commentConverter = commentConverter;
        }

        public async Task<List<Comment>> ListPostCommentsAsync(ulong postId,
            SortType? sort, User requestingUser = null)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new
            {
                postId,
                sort,
                requestingUserId = requestingUser?.Id
            });
            var sw = Stopwatch.StartNew();
            var commentEntities = await _commentEntities
                .AsNoTracking()
                .Search(
                    authorId: null,
                    self: null,
                    postId: postId,
                    search: null,
                    sort: sort,
                    isRoot: true
                )
                .Include(
                    authorEntity: false,
                    postEntity: false,
                    childCommentEntities: true,
                    requestingUser: requestingUser,
                    sort: sort
                )
                .Take(Configs.Current.QueryResult.TotalLimit)
                .ToListAsync();

            if (requestingUser != null)
                commentEntities.ForEach(e => e.CheckRequestingUserVote(requestingUser));

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT",
                parameters: new { commentEntities = commentEntities.Select(e => e.Id) });
            return commentEntities.Select(_commentConverter.ConvertToModel).ToList();
        }

        public async Task<List<Comment>> ListCommentsByIdsAsync(List<ulong> Ids,
            SortType? sort, User requestingUser = null)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT",
                parameters: new
                {
                    Ids,
                    sort,
                    requestingUserId = requestingUser?.Id
                });
            var sw = Stopwatch.StartNew();
            var commentEntities = await _commentEntities
                .AsNoTracking()
                .Where(e => Ids.Contains(e.Id))
                .Include(
                    authorEntity: false,
                    postEntity: false,
                    childCommentEntities: true,
                    requestingUser: requestingUser,
                    sort: sort
                )
                .ToListAsync();

            if (requestingUser != null)
                commentEntities.ForEach(e => e.CheckRequestingUserVote(requestingUser));

            // To preserve the order of input Ids
            var commentEntityDictionary = new Dictionary<ulong, CommentEntity>();
            commentEntities.ForEach(e => commentEntityDictionary[e.Id] = e);
            commentEntities = new List<CommentEntity>();
            Ids.ForEach(id => commentEntities.Add(commentEntityDictionary[id]));

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT",
                parameters: new { commentEntities = commentEntities.Select(e => e.Id) });
            return commentEntities.Select(_commentConverter.ConvertToModel).ToList();
        }

        public async Task<List<Comment>> ListSelfCommentsAsync(User author,
            SortType? sort)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new
            {
                authorId = author.Id,
                sort
            });
            var sw = Stopwatch.StartNew();
            var commentEntities = await _commentEntities
                .AsNoTracking()
                .Search(
                    authorId: author.Id,
                    self: true,
                    postId: null,
                    search: null,
                    sort: sort,
                    isRoot: null
                )
                .Include(
                    authorEntity: false,
                    postEntity: false,
                    childCommentEntities: false,
                    requestingUser: author,
                    sort: sort
                )
                .Take(Configs.Current.QueryResult.TotalLimit)
                .ToListAsync();

            commentEntities.ForEach(e => e.CheckRequestingUserVote(author));

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT",
                parameters: new { commentEntities = commentEntities.Select(e => e.Id) });
            return commentEntities.Select(_commentConverter.ConvertToModel).ToList();
        }

        public async Task<Comment> GetCommentByIdAsync(ulong id,
            bool includeAuthor = false, bool includePost = false,
            bool includeChildComments = false, SortType? sort = null,
            User requestingUser = null)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new
            {
                id,
                includeAuthor,
                includePost,
                includeChildComments,
                sort,
                requestingUserId = requestingUser?.Id
            });
            var sw = Stopwatch.StartNew();
            var commentEntity = await _commentEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Include(
                    authorEntity: includeAuthor,
                    postEntity: includePost,
                    childCommentEntities: includeChildComments,
                    requestingUser: requestingUser,
                    sort: sort
                )
                .SingleOrDefaultAsync();

            if (requestingUser != null)
                commentEntity.CheckRequestingUserVote(requestingUser);

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { commentEntity });
            return _commentConverter.ConvertToModel(commentEntity);
        }

        public async Task<Comment> CreateCommentAsync(ulong id, ulong authorUserId,
            string authorUsername, ulong postId, string content,
            ulong? parentCommentId = null)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new
            {
                id,
                authorUserId,
                authorUsername,
                postId,
                content,
                parentCommentId
            });
            var sw = Stopwatch.StartNew();
            var commentEntity = new CommentEntity(id, authorUserId,
                authorUsername, postId, content, parentCommentId);
            _commentEntities.Add(commentEntity);
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachAllEntries();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT",
                parameters: new { commentEntity });
            return _commentConverter.ConvertToModel(commentEntity);
        }

        public async Task<Comment> UpdateCommentAsync(Comment comment)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { comment });
            var sw = Stopwatch.StartNew();
            var commentEntity = _commentConverter.ConvertToEntity(comment);
            _commentEntities.Update(commentEntity);
            try
            {
                await _dbContext.SaveChangesAsync();
                _dbContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var serverMessage = $"Can't update the commentEntity DbUpdateConcurrencyException. {commentEntity.ToJson()}";
                throw new ApiException(ErrorName.INTERNAL_SERVER, serverMessage, systemException: ex);
            }

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { commentEntity });
            return _commentConverter.ConvertToModel(commentEntity);
        }

        public async Task DeleteCommentByIdAsync(ulong id)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { id });
            var sw = Stopwatch.StartNew();
            var commentEntity = await _commentEntities
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync();

            _commentEntities.Remove(commentEntity);
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachAllEntries();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { commentEntity });
        }

        public async Task<bool> DoesCommentIdExistAsync(ulong id)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { id });
            var sw = Stopwatch.StartNew();
            var doesExist = await _commentEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .AnyAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { doesExist });
            return doesExist;
        }
    }

    public static class CommentRepositoryExtensions
    {
        public static IQueryable<CommentEntity> Include(
            [NotNull] this IQueryable<CommentEntity> q,
            bool authorEntity, bool postEntity, bool childCommentEntities,
            User requestingUser, SortType? sort)
        {
            if (authorEntity)
                q = q.Include(e => e.AuthorEntity);

            if (postEntity)
                q = q.Include(e => e.PostEntity);

            if (childCommentEntities)
            {
                sort ??= Constants.DefaultSort;


                IIncludableQueryable<CommentEntity, IOrderedEnumerable<CommentEntity>> z = null;
                for (int i = 0; i < Configs.Current.QueryResult.DepthLimit; i++)
                {
                    switch (sort)
                    {
                        case SortType.TOP:
                            z = q.Include(
                                e => e.ChildCommentEntities.OrderByDescending(e => e.Vote));
                            break;
                        case SortType.NEW:
                            z = q.Include(
                                e => e.ChildCommentEntities.OrderByDescending(e => e.CreationDate));
                            break;
                        case SortType.OLD:
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
                            case SortType.TOP:
                                z = z.ThenInclude(
                                    e => e.ChildCommentEntities.OrderByDescending(e => e.Vote));
                                break;
                            case SortType.NEW:
                                z = z.ThenInclude(
                                    e => e.ChildCommentEntities.OrderByDescending(e => e.CreationDate));
                                break;
                            case SortType.OLD:
                                z = z.ThenInclude(
                                    e => e.ChildCommentEntities.OrderBy(e => e.CreationDate));
                                break;
                            default:
                                break;
                        }
                    }

                    if (requestingUser != null)
                    {
                        q = z.ThenInclude(e => e.CommentVoteEntities
                                            .Where(cv => cv.VoterEntityId == requestingUser.Id));
                    }
                    else
                        q = z;
                }
            }

            if (requestingUser != null)
            {
                q = q.Include(
                    c => c.CommentVoteEntities
                        .Where(cv => cv.VoterEntityId == requestingUser.Id)
                );
            }

            return q;
        }

        public static IQueryable<CommentEntity> Search(
            [NotNull] this IQueryable<CommentEntity> q, bool? self,
            ulong? authorId, ulong? postId, string search, SortType? sort,
            bool? isRoot)
        {
            if (self.HasValue && self.Value)
                q = q.Where(e => e.AuthorEntityId == authorId.Value);

            if (postId.HasValue)
                q = q.Where(e => e.PostEntityId == postId.Value);

            if (!string.IsNullOrWhiteSpace(search))
                q = q.Where(e => EF.Functions
                    .ILike(EF.Functions.Collate(e.Content, "default"), $"%{search}%"));

            if (isRoot.HasValue && isRoot.Value)
                q = q.Where(e => e.ParentCommentEntityId == null);

            if (sort.HasValue)
            {
                q = sort switch
                {
                    SortType.TOP => q.OrderByDescending(e => e.Vote),
                    SortType.NEW => q.OrderByDescending(e => e.CreationDate),
                    SortType.OLD => q.OrderBy(e => e.CreationDate),
                    _ => q.OrderByDescending(e => e.Vote),
                };
            }
            else
                q = q.OrderByDescending(e => e.Vote);

            return q;
        }
    }
}
