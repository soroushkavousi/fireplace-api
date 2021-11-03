using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using FireplaceApi.Infrastructure.Converters;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;
        private readonly FireplaceApiContext _fireplaceApiContext;
        private readonly DbSet<CommentEntity> _commentEntities;
        private readonly CommentConverter _commentConverter;

        public CommentRepository(ILogger<CommentRepository> logger, IConfiguration configuration,
            FireplaceApiContext fireplaceApiContext, CommentConverter commentConverter)
        {
            _logger = logger;
            _configuration = configuration;
            _fireplaceApiContext = fireplaceApiContext;
            _commentEntities = fireplaceApiContext.CommentEntities;
            _commentConverter = commentConverter;
        }

        public async Task<List<Comment>> ListCommentsAsync(List<long> Ids,
            User requesterUser = null)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { Ids });
            var sw = Stopwatch.StartNew();
            var commentEntities = await _commentEntities
                .AsNoTracking()
                .Where(e => Ids.Contains(e.Id.Value))
                .Include(
                    fireplaceApiContext: _fireplaceApiContext,
                    authorEntity: false,
                    postEntity: false,
                    requesterUser: requesterUser
                )
                .ToListAsync();

            Dictionary<long, CommentEntity> commentEntityDictionary = new Dictionary<long, CommentEntity>();
            commentEntities.ForEach(e => commentEntityDictionary[e.Id.Value] = e);
            commentEntities = new List<CommentEntity>();
            Ids.ForEach(id => commentEntities.Add(commentEntityDictionary[id]));

            _logger.LogIOInformation(sw, "Database | Output", new { commentEntities });
            return commentEntities.Select(e => _commentConverter.ConvertToModel(e)).ToList();
        }

        public async Task<List<long>> ListSelfCommentIdsAsync(long authorId,
            SortType? sort)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new
            {
                authorId, sort
            });
            var sw = Stopwatch.StartNew();
            var commentEntityIds = await _commentEntities
                .AsNoTracking()
                .Search(
                    authorId: authorId,
                    self: true,
                    postId: null,
                    search: null,
                    sort: sort,
                    parentCommentIds: null,
                    parentCommentId: null,
                    isRoot: null
                )
                .Take(GlobalOperator.GlobalValues.Pagination.TotalItemsCount)
                .Select(e => e.Id.Value)
                .ToListAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { commentEntityIds });
            return commentEntityIds;
        }

        public async Task<List<long>> ListPostCommentIdsAsync(long postId,
            SortType? sort)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new
            {
                postId, sort
            });
            var sw = Stopwatch.StartNew();
            var commentEntityIds = await _commentEntities
                .AsNoTracking()
                .Search(
                    authorId: null,
                    self: null,
                    postId: postId,
                    search: null,
                    sort: sort,
                    parentCommentIds: null,
                    parentCommentId: null,
                    isRoot: true
                )
                .Take(GlobalOperator.GlobalValues.Pagination.TotalItemsCount)
                .Select(e => e.Id.Value)
                .ToListAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { commentEntityIds });
            return commentEntityIds;
        }

        public async Task<List<Comment>> ListChildCommentsAsync(long postId,
            List<long> parentCommentIds, User requesterUser = null)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new
            {
                postId, parentCommentIds
            });
            var sw = Stopwatch.StartNew();
            var commentEntities = await _commentEntities
                .AsNoTracking()
                .Search(
                    authorId: null,
                    self: null,
                    postId: postId,
                    search: null,
                    sort: null,
                    parentCommentIds: parentCommentIds,
                    parentCommentId: null,
                    isRoot: null
                )
                .Include(
                    fireplaceApiContext: _fireplaceApiContext,
                    authorEntity: false,
                    postEntity: false,
                    requesterUser: requesterUser
                )
                .ToListAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { commentEntities });
            return commentEntities.Select(e => _commentConverter.ConvertToModel(e)).ToList();
        }

        public async Task<List<Comment>> ListChildCommentsAsync(long postId,
            long parentCommentId, User requesterUser = null)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new
            {
                postId, parentCommentId
            });
            var sw = Stopwatch.StartNew();
            var commentEntities = await _commentEntities
                .AsNoTracking()
                .Search(
                    authorId: null,
                    self: null,
                    postId: postId,
                    search: null,
                    sort: null,
                    parentCommentIds: null,
                    parentCommentId: parentCommentId,
                    isRoot: null
                )
                .Include(
                    fireplaceApiContext: _fireplaceApiContext,
                    authorEntity: false,
                    postEntity: false,
                    requesterUser: requesterUser
                )
                .ToListAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { commentEntities });
            return commentEntities.Select(e => _commentConverter.ConvertToModel(e)).ToList();
        }

        public async Task<Comment> GetCommentByIdAsync(long id,
            bool includeAuthor = false, bool includePost = false,
            User requesterUser = null)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new
            {
                id, includeAuthor, includePost, requesterUser = requesterUser != null
            });
            var sw = Stopwatch.StartNew();
            var commentEntity = await _commentEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Include(
                    fireplaceApiContext: _fireplaceApiContext,
                    authorEntity: includeAuthor,
                    postEntity: includePost,
                    requesterUser: requesterUser
                )
                .SingleOrDefaultAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { commentEntity });
            return _commentConverter.ConvertToModel(commentEntity);
        }

        public async Task<Comment> CreateCommentAsync(long authorUserId,
            string authorUsername, long postId, string content,
            List<long> parentCommentIds = null)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new
            {
                authorUserId, authorUsername, postId, content
            });
            var sw = Stopwatch.StartNew();
            var commentEntity = new CommentEntity(authorUserId,
                authorUsername, postId, content, parentCommentIds);
            _commentEntities.Add(commentEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database | Output",
                new { commentEntity });
            return _commentConverter.ConvertToModel(commentEntity);
        }

        public async Task<Comment> UpdateCommentAsync(Comment comment)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { comment });
            var sw = Stopwatch.StartNew();
            var commentEntity = _commentConverter.ConvertToEntity(comment);
            _commentEntities.Update(commentEntity);
            try
            {
                await _fireplaceApiContext.SaveChangesAsync();
                _fireplaceApiContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var serverMessage = $"Can't update the commentEntity DbUpdateConcurrencyException. {commentEntity.ToJson()}";
                throw new ApiException(ErrorName.INTERNAL_SERVER, serverMessage, systemException: ex);
            }

            _logger.LogIOInformation(sw, "Database | Output", new { commentEntity });
            return _commentConverter.ConvertToModel(commentEntity);
        }

        public async Task DeleteCommentByIdAsync(long id)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { id });
            var sw = Stopwatch.StartNew();
            var commentEntity = await _commentEntities
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync();

            _commentEntities.Remove(commentEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database | Output", new { commentEntity });
        }

        public async Task<bool> DoesCommentIdExistAsync(long id)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { id });
            var sw = Stopwatch.StartNew();
            var doesExist = await _commentEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .AnyAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { doesExist });
            return doesExist;
        }
    }

    public static class CommentRepositoryExtensions
    {
        public static IQueryable<CommentEntity> Include(
            [NotNull] this IQueryable<CommentEntity> q,
            FireplaceApiContext fireplaceApiContext,
            bool authorEntity, bool postEntity, User requesterUser)
        {
            if (authorEntity)
                q = q.Include(e => e.AuthorEntity);

            if (postEntity)
                q = q.Include(e => e.PostEntity);

            if (requesterUser != null)
            {
                q = q.Include(
                    c => c.CommentVoteEntities
                        .Where(cv => cv.VoterEntityId == requesterUser.Id)
                );
            }

            return q;
        }

        public static IQueryable<CommentEntity> Search(
            [NotNull] this IQueryable<CommentEntity> q, bool? self,
            long? authorId, long? postId, string search, SortType? sort,
            List<long> parentCommentIds, long? parentCommentId, bool? isRoot)
        {
            if (self.HasValue && self.Value)
                q = q.Where(e => e.AuthorEntityId == authorId.Value);

            if (postId.HasValue)
                q = q.Where(e => e.PostEntityId == postId.Value);

            if (!string.IsNullOrWhiteSpace(search))
                q = q.Where(e => EF.Functions
                    .ILike(EF.Functions.Collate(e.Content, "default"), $"%{search}%"));

            if (parentCommentIds != null && parentCommentIds.Count != 0)
            {
                q = q.Where(e =>
                    e.ParentCommentEntityIds.Any(eParentId => parentCommentIds.Contains(eParentId)));
            }

            if (parentCommentId.HasValue)
                q = q.Where(e => e.ParentCommentEntityIds.Contains(parentCommentId.Value));

            if (isRoot.HasValue && isRoot.Value)
                q = q.Where(e => e.ParentCommentEntityIds == null || e.ParentCommentEntityIds.Count == 0);

            if (sort.HasValue)
            {
                switch (sort)
                {
                    case SortType.TOP:
                        q = q.OrderByDescending(e => e.Vote);
                        break;
                    case SortType.NEW:
                        q = q.OrderByDescending(e => e.CreationDate);
                        break;
                    case SortType.OLD:
                        q = q.OrderBy(e => e.CreationDate);
                        break;
                    default:
                        break;
                }
            }

            return q;
        }
    }
}
