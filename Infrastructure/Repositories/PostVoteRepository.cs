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
    public class PostVoteRepository : IPostVoteRepository
    {
        private readonly ILogger<PostVoteRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly FireplaceApiContext _fireplaceApiContext;
        private readonly DbSet<PostVoteEntity> _postVoteEntities;
        private readonly PostVoteConverter _postVoteConverter;

        public PostVoteRepository(ILogger<PostVoteRepository> logger, IConfiguration configuration,
            FireplaceApiContext fireplaceApiContext, PostVoteConverter postVoteConverter)
        {
            _logger = logger;
            _configuration = configuration;
            _fireplaceApiContext = fireplaceApiContext;
            _postVoteEntities = fireplaceApiContext.PostVoteEntities;
            _postVoteConverter = postVoteConverter;
        }

        public async Task<List<PostVote>> ListPostVotesAsync(List<long> Ids)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { Ids });
            var sw = Stopwatch.StartNew();
            var postEntities = await _postVoteEntities
                .AsNoTracking()
                .Where(e => Ids.Contains(e.Id.Value))
                .ToListAsync();

            Dictionary<long, PostVoteEntity> postEntityDictionary = new Dictionary<long, PostVoteEntity>();
            postEntities.ForEach(e => postEntityDictionary[e.Id.Value] = e);
            postEntities = new List<PostVoteEntity>();
            Ids.ForEach(id => postEntities.Add(postEntityDictionary[id]));

            _logger.LogIOInformation(sw, "Database | Output", new { postEntities });
            return postEntities.Select(e => _postVoteConverter.ConvertToModel(e)).ToList();
        }

        public async Task<PostVote> GetPostVoteByIdAsync(long id,
            bool includeVoter = false, bool includePost = false)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new
            {
                id, includeVoter, includePost
            });
            var sw = Stopwatch.StartNew();
            var postEntity = await _postVoteEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Include(
                    voterEntity: includeVoter,
                    postEntity: includePost
                )
                .SingleOrDefaultAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { postEntity });
            return _postVoteConverter.ConvertToModel(postEntity);
        }

        public async Task<PostVote> GetPostVoteAsync(long voterId,
            long postId, bool includeVoter = false, bool includePost = false)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new
            {
                voterId, postId, includeVoter, includePost
            });
            var sw = Stopwatch.StartNew();
            var postEntity = await _postVoteEntities
                .AsNoTracking()
                .Where(e => e.VoterEntityId == voterId
                    && e.PostEntityId == postId)
                .Include(
                    voterEntity: includeVoter,
                    postEntity: includePost
                )
                .SingleOrDefaultAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { postEntity });
            return _postVoteConverter.ConvertToModel(postEntity);
        }

        public async Task<PostVote> CreatePostVoteAsync(long voterUserId,
            string voterUsername, long postId, bool isUp)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new
            {
                voterUserId, voterUsername, postId, isUp
            });
            var sw = Stopwatch.StartNew();
            var postEntity = new PostVoteEntity(voterUserId,
                voterUsername, postId, isUp);
            _postVoteEntities.Add(postEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database | Output",
                new { postEntity });
            return _postVoteConverter.ConvertToModel(postEntity);
        }

        public async Task<PostVote> UpdatePostVoteAsync(PostVote postvote)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { postvote });
            var sw = Stopwatch.StartNew();
            var postEntity = _postVoteConverter.ConvertToEntity(postvote);
            _postVoteEntities.Update(postEntity);
            try
            {
                await _fireplaceApiContext.SaveChangesAsync();
                _fireplaceApiContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var serverMessage = $"Can't update the postEntity DbUpdateConcurrencyException. {postEntity.ToJson()}";
                throw new ApiException(ErrorName.INTERNAL_SERVER, serverMessage, systemException: ex);
            }

            _logger.LogIOInformation(sw, "Database | Output", new { postEntity });
            return _postVoteConverter.ConvertToModel(postEntity);
        }

        public async Task DeletePostVoteByIdAsync(long id)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { id });
            var sw = Stopwatch.StartNew();
            var postEntity = await _postVoteEntities
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync();

            _postVoteEntities.Remove(postEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database | Output", new { postEntity });
        }

        public async Task<bool> DoesPostVoteIdExistAsync(long id)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { id });
            var sw = Stopwatch.StartNew();
            var doesExist = await _postVoteEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .AnyAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { doesExist });
            return doesExist;
        }

        public async Task<bool> DoesPostVoteIdExistAsync(long voterId, long postId)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { voterId, postId });
            var sw = Stopwatch.StartNew();
            var doesExist = await _postVoteEntities
                .AsNoTracking()
                .Where(e => e.VoterEntityId == voterId
                    && e.PostEntityId == postId)
                .AnyAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { doesExist });
            return doesExist;
        }
    }

    public static class PostVoteRepositoryExtensions
    {
        public static IQueryable<PostVoteEntity> Include(
            [NotNull] this IQueryable<PostVoteEntity> q,
            bool voterEntity, bool postEntity)
        {
            if (voterEntity)
                q = q.Include(e => e.VoterEntity);

            if (postEntity)
                q = q.Include(e => e.PostEntity);

            return q;
        }

        public static IQueryable<PostVoteEntity> Search(
            [NotNull] this IQueryable<PostVoteEntity> q,
            long? voterId, long? postId, SortType? sort)
        {
            if (voterId.HasValue)
                q = q.Where(e => e.VoterEntityId == voterId.Value);

            if (postId.HasValue)
                q = q.Where(e => e.PostEntityId == postId.Value);

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
