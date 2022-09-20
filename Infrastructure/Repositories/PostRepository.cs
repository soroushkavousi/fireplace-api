using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Identifiers;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Models;
using FireplaceApi.Infrastructure.Converters;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
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
        private readonly FireplaceApiDbContext _dbContext;
        private readonly DbSet<PostEntity> _postEntities;
        private readonly PostConverter _postConverter;

        public PostRepository(ILogger<PostRepository> logger,
            FireplaceApiDbContext dbContext, PostConverter postConverter)
        {
            _logger = logger;
            _dbContext = dbContext;
            _postEntities = dbContext.PostEntities;
            _postConverter = postConverter;
        }

        public async Task<List<Post>> ListPostsAsync(List<ulong> Ids,
            User requestingUser = null)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new
            {
                Ids,
                requestingUser = requestingUser != null
            });
            var sw = Stopwatch.StartNew();
            var postEntities = await _postEntities
                .AsNoTracking()
                .Where(e => Ids.Contains(e.Id))
                .Include(
                    authorEntity: false,
                    communityEntity: false,
                    requestingUser: requestingUser
                )
                .ToListAsync();

            var postEntityDictionary = new Dictionary<ulong, PostEntity>();
            postEntities.ForEach(e => postEntityDictionary[e.Id] = e);
            postEntities = new List<PostEntity>();
            Ids.ForEach(id => postEntities.Add(postEntityDictionary[id]));

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { postEntities });
            return postEntities.Select(e => _postConverter.ConvertToModel(e)).ToList();
        }

        public async Task<List<Post>> ListPostsAsync(ulong? authorId,
            bool? self, bool? joined, CommunityIdentifier communityIdentifier,
            string search, SortType? sort,
            User requestingUser)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new
            {
                authorId,
                self,
                joined,
                communityIdentifier,
                search,
                sort
            });
            var sw = Stopwatch.StartNew();
            var postEntities = await _postEntities
                .AsNoTracking()
                .Search(
                    authorId: authorId,
                    self: self,
                    joined: joined,
                    communityIdentifier: communityIdentifier,
                    search: search,
                    sort: sort
                )
                .Include(
                    authorEntity: false,
                    communityEntity: true,
                    requestingUser: requestingUser
                )
                .Take(Configs.Current.Pagination.TotalItemsCount)
                .ToListAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { postEntities });
            return postEntities.Select(e => _postConverter.ConvertToModel(e)).ToList();
        }

        public async Task<List<ulong>> ListPostIdsAsync(ulong? authorId,
            bool? self, bool? joined, CommunityIdentifier communityIdentifier,
            string search, SortType? sort)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new
            {
                authorId,
                self,
                joined,
                communityIdentifier,
                search,
                sort
            });
            var sw = Stopwatch.StartNew();
            var postEntityIds = await _postEntities
                .AsNoTracking()
                .Search(
                    authorId: authorId,
                    self: self,
                    joined: joined,
                    communityIdentifier: communityIdentifier,
                    search: search,
                    sort: sort
                )
                .Take(Configs.Current.Pagination.TotalItemsCount)
                .Select(e => e.Id)
                .ToListAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { postEntityIds });
            return postEntityIds;
        }

        public async Task<Post> GetPostByIdAsync(ulong id,
            bool includeAuthor = false, bool includeCommunity = false,
            User requestingUser = null)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new
            {
                id,
                includeAuthor,
                includeCommunity,
                requestingUser = requestingUser != null
            });
            var sw = Stopwatch.StartNew();
            var postEntity = await _postEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Include(
                    authorEntity: includeAuthor,
                    communityEntity: includeCommunity,
                    requestingUser: requestingUser
                )
                .SingleOrDefaultAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { postEntity });
            return _postConverter.ConvertToModel(postEntity);
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
            return _postConverter.ConvertToModel(postEntity);
        }

        public async Task<Post> UpdatePostAsync(Post post)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { post });
            var sw = Stopwatch.StartNew();
            var postEntity = _postConverter.ConvertToEntity(post);
            _postEntities.Update(postEntity);
            try
            {
                await _dbContext.SaveChangesAsync();
                _dbContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var serverMessage = $"Can't update the postEntity DbUpdateConcurrencyException. {postEntity.ToJson()}";
                throw new ApiException(ErrorName.INTERNAL_SERVER, serverMessage, systemException: ex);
            }

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { postEntity });
            return _postConverter.ConvertToModel(postEntity);
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
            User requestingUser)
        {
            if (authorEntity)
                q = q.Include(e => e.AuthorEntity);

            if (communityEntity)
                q = q.Include(e => e.CommunityEntity);

            if (requestingUser != null)
            {
                q = q.Include(
                    c => c.PostVoteEntities
                        .Where(cv => cv.VoterEntityId == requestingUser.Id)
                );
            }

            return q;
        }

        public static IQueryable<PostEntity> Search(
            [NotNull] this IQueryable<PostEntity> q, bool? self,
            bool? joined, ulong? authorId, CommunityIdentifier communityIdentifier,
            string search, SortType? sort)
        {
            if (self.HasValue)
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

            if (sort.HasValue)
            {
                switch (sort)
                {
                    case SortType.TOP:
                        q = q.OrderBy(e => e.Vote);
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
