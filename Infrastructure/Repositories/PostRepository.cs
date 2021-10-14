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
    public class PostRepository : IPostRepository
    {
        private readonly ILogger<PostRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly FireplaceApiContext _fireplaceApiContext;
        private readonly DbSet<PostEntity> _postEntities;
        private readonly PostConverter _postConverter;

        public PostRepository(ILogger<PostRepository> logger, IConfiguration configuration,
            FireplaceApiContext fireplaceApiContext, PostConverter postConverter)
        {
            _logger = logger;
            _configuration = configuration;
            _fireplaceApiContext = fireplaceApiContext;
            _postEntities = fireplaceApiContext.PostEntities;
            _postConverter = postConverter;
        }

        public async Task<List<Post>> ListPostsAsync(List<long> Ids)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { Ids });
            var sw = Stopwatch.StartNew();
            var postEntities = await _postEntities
                .AsNoTracking()
                .Where(e => Ids.Contains(e.Id.Value))
                .ToListAsync();

            Dictionary<long, PostEntity> postEntityDictionary = new Dictionary<long, PostEntity>();
            postEntities.ForEach(e => postEntityDictionary[e.Id.Value] = e);
            postEntities = new List<PostEntity>();
            Ids.ForEach(id => postEntities.Add(postEntityDictionary[id]));

            _logger.LogIOInformation(sw, "Database | Output", new { postEntities });
            return postEntities.Select(e => _postConverter.ConvertToModel(e)).ToList();
        }

        public async Task<List<Post>> ListPostsAsync(long? authorId,
            bool? self, bool? joined, long? communityId,
            string communityName, string search, SortType? sort)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new
            {
                authorId, self, joined, communityId, 
                communityName, search, sort
            });
            var sw = Stopwatch.StartNew();
            var postEntities = await _postEntities
                .AsNoTracking()
                .Search(
                    authorId: authorId,
                    self: self,
                    joined: joined,
                    communityId: communityId,
                    communityName: communityName,
                    search: search,
                    sort: sort
                )
                .Include(
                    authorEntity: false,
                    communityEntity: true
                )
                .Take(GlobalOperator.GlobalValues.Pagination.TotalItemsCount)
                .ToListAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { postEntities });
            return postEntities.Select(e => _postConverter.ConvertToModel(e)).ToList();
        }

        public async Task<List<long>> ListPostIdsAsync(long? authorId,
            bool? self, bool? joined, long? communityId,
            string communityName, string search, SortType? sort)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new
            {
                authorId, self, joined, communityId, communityName,
                search, sort
            });
            var sw = Stopwatch.StartNew();
            var postEntities = await _postEntities
                .AsNoTracking()
                .Search(
                    authorId: authorId,
                    self: self,
                    joined: joined,
                    communityId: communityId,
                    communityName: communityName,
                    search: search,
                    sort: sort
                )
                .Include(
                    authorEntity: false,
                    communityEntity: true
                )
                .Take(GlobalOperator.GlobalValues.Pagination.TotalItemsCount)
                .Select(e => e.Id.Value)
                .ToListAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { postEntities });
            return postEntities;
        }

        public async Task<Post> GetPostByIdAsync(long id,
            bool includeAuthor = false, bool includeCommunity = false)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new
            {
                id, includeAuthor, includeCommunity
            });
            var sw = Stopwatch.StartNew();
            var postEntity = await _postEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Include(
                    authorEntity: includeAuthor,
                    communityEntity: includeCommunity
                )
                .SingleOrDefaultAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { postEntity });
            return _postConverter.ConvertToModel(postEntity);
        }

        public async Task<Post> CreatePostAsync(long authorUserId,
            string authorUsername, long communityId, string communityName,
            string content)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new
            {
                authorUserId, authorUsername, communityId,
                communityName, content
            });
            var sw = Stopwatch.StartNew();
            var postEntity = new PostEntity(authorUserId, authorUsername,
                communityId, communityName, content);
            _postEntities.Add(postEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database | Output",
                new { postEntity });
            return _postConverter.ConvertToModel(postEntity);
        }

        public async Task<Post> UpdatePostAsync(Post post)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { post });
            var sw = Stopwatch.StartNew();
            var postEntity = _postConverter.ConvertToEntity(post);
            _postEntities.Update(postEntity);
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
            return _postConverter.ConvertToModel(postEntity);
        }

        public async Task DeletePostByIdAsync(long id)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { id });
            var sw = Stopwatch.StartNew();
            var postEntity = await _postEntities
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync();

            _postEntities.Remove(postEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database | Output", new { postEntity });
        }

        public async Task<bool> DoesPostIdExistAsync(long id)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { id });
            var sw = Stopwatch.StartNew();
            var doesExist = await _postEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .AnyAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { doesExist });
            return doesExist;
        }
    }

    public static class PostRepositoryExtensions
    {
        public static IQueryable<PostEntity> Include(
            [NotNull] this IQueryable<PostEntity> q,
            bool authorEntity, bool communityEntity)
        {
            if (authorEntity)
                q = q.Include(e => e.AuthorEntity);

            if (communityEntity)
                q = q.Include(e => e.CommunityEntity);

            return q;
        }



        public static IQueryable<PostEntity> Search(
            [NotNull] this IQueryable<PostEntity> q, bool? self,
            bool? joined, long? authorId, long? communityId,
            string communityName, string search, SortType? sort)
        {
            if (self.HasValue)
                q = q.Where(e => e.AuthorEntityId == authorId.Value);

            if (communityId.HasValue)
                q = q.Where(e => e.CommunityEntityId == communityId.Value);
            else if (!string.IsNullOrWhiteSpace(communityName))
                q = q.Where(e => e.CommunityEntityName == communityName);

            if (!string.IsNullOrWhiteSpace(search))
                q = q.Where(e => EF.Functions
                    .ILike(EF.Functions.Collate(e.Content, "default"), $"%{search}%"));

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

            if (joined.HasValue)
                q = q.Where(
                    e => e.CommunityEntity.CommunityMemberEntities.Select(jc => jc.UserEntityId)
                        .Contains(e.CommunityEntityId));

            return q;
        }
    }
}
