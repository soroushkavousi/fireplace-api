using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Models;
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
    public class CommentVoteRepository : ICommentVoteRepository
    {
        private readonly ILogger<CommentVoteRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly FireplaceApiContext _fireplaceApiContext;
        private readonly DbSet<CommentVoteEntity> _commentVoteEntities;
        private readonly CommentVoteConverter _commentVoteConverter;

        public CommentVoteRepository(ILogger<CommentVoteRepository> logger, IConfiguration configuration,
            FireplaceApiContext fireplaceApiContext, CommentVoteConverter commentVoteConverter)
        {
            _logger = logger;
            _configuration = configuration;
            _fireplaceApiContext = fireplaceApiContext;
            _commentVoteEntities = fireplaceApiContext.CommentVoteEntities;
            _commentVoteConverter = commentVoteConverter;
        }

        public async Task<List<CommentVote>> ListCommentVotesAsync(List<ulong> Ids)
        {
            _logger.LogIOInformation(null, "Database | Input", new { Ids });
            var sw = Stopwatch.StartNew();
            var commentEntities = await _commentVoteEntities
                .AsNoTracking()
                .Where(e => Ids.Contains(e.Id))
                .ToListAsync();

            Dictionary<ulong, CommentVoteEntity> commentEntityDictionary = new Dictionary<ulong, CommentVoteEntity>();
            commentEntities.ForEach(e => commentEntityDictionary[e.Id] = e);
            commentEntities = new List<CommentVoteEntity>();
            Ids.ForEach(id => commentEntities.Add(commentEntityDictionary[id]));

            _logger.LogIOInformation(sw, "Database | Output", new { commentEntities });
            return commentEntities.Select(e => _commentVoteConverter.ConvertToModel(e)).ToList();
        }

        public async Task<CommentVote> GetCommentVoteByIdAsync(ulong id,
            bool includeVoter = false, bool includeComment = false)
        {
            _logger.LogIOInformation(null, "Database | Input", new
            {
                id, includeVoter, includeComment
            });
            var sw = Stopwatch.StartNew();
            var commentEntity = await _commentVoteEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Include(
                    voterEntity: includeVoter,
                    commentEntity: includeComment
                )
                .SingleOrDefaultAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { commentEntity });
            return _commentVoteConverter.ConvertToModel(commentEntity);
        }

        public async Task<CommentVote> GetCommentVoteAsync(ulong voterId,
            ulong commentId, bool includeVoter = false, bool includeComment = false)
        {
            _logger.LogIOInformation(null, "Database | Input", new
            {
                voterId, commentId, includeVoter, includeComment
            });
            var sw = Stopwatch.StartNew();
            var commentEntity = await _commentVoteEntities
                .AsNoTracking()
                .Where(e => e.VoterEntityId == voterId
                    && e.CommentEntityId == commentId)
                .Include(
                    voterEntity: includeVoter,
                    commentEntity: includeComment
                )
                .SingleOrDefaultAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { commentEntity });
            return _commentVoteConverter.ConvertToModel(commentEntity);
        }

        public async Task<CommentVote> CreateCommentVoteAsync(ulong id, ulong voterUserId,
            string voterUsername, ulong commentId, bool isUp)
        {
            _logger.LogIOInformation(null, "Database | Input", new
            {
                id, voterUserId, voterUsername, commentId, isUp
            });
            var sw = Stopwatch.StartNew();
            var commentEntity = new CommentVoteEntity(id, voterUserId,
                voterUsername, commentId, isUp);
            _commentVoteEntities.Add(commentEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database | Output",
                new { commentEntity });
            return _commentVoteConverter.ConvertToModel(commentEntity);
        }

        public async Task<CommentVote> UpdateCommentVoteAsync(CommentVote commentvote)
        {
            _logger.LogIOInformation(null, "Database | Input", new { commentvote });
            var sw = Stopwatch.StartNew();
            var commentEntity = _commentVoteConverter.ConvertToEntity(commentvote);
            _commentVoteEntities.Update(commentEntity);
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
            return _commentVoteConverter.ConvertToModel(commentEntity);
        }

        public async Task DeleteCommentVoteByIdAsync(ulong id)
        {
            _logger.LogIOInformation(null, "Database | Input", new { id });
            var sw = Stopwatch.StartNew();
            var commentEntity = await _commentVoteEntities
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync();

            _commentVoteEntities.Remove(commentEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database | Output", new { commentEntity });
        }

        public async Task<bool> DoesCommentVoteIdExistAsync(ulong id)
        {
            _logger.LogIOInformation(null, "Database | Input", new { id });
            var sw = Stopwatch.StartNew();
            var doesExist = await _commentVoteEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .AnyAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { doesExist });
            return doesExist;
        }

        public async Task<bool> DoesCommentVoteIdExistAsync(ulong voterId, ulong commentId)
        {
            _logger.LogIOInformation(null, "Database | Input", new { voterId, commentId });
            var sw = Stopwatch.StartNew();
            var doesExist = await _commentVoteEntities
                .AsNoTracking()
                .Where(e => e.VoterEntityId == voterId
                    && e.CommentEntityId == commentId)
                .AnyAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { doesExist });
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
            ulong? voterId, ulong? commentId, SortType? sort)
        {
            if (voterId.HasValue)
                q = q.Where(e => e.VoterEntityId == voterId.Value);

            if (commentId.HasValue)
                q = q.Where(e => e.CommentEntityId == commentId.Value);

            if (sort.HasValue)
            {
                switch (sort)
                {
                    case SortType.TOP:
                        q = q.OrderByDescending(e => e.IsUp);
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
