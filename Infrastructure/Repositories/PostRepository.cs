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

        public async Task<List<Post>> ListCommunityPostsAsync(CommunityIdentifier communityIdentifier,
            SortType sort, User requestingUser = null)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new
            {
                communityIdentifier,
                sort,
                requestingUserId = requestingUser?.Id
            });
            var sw = Stopwatch.StartNew();
            var postEntities = await _postEntities
                .AsNoTracking()
                .Search(
                    authorId: null,
                    self: null,
                    joined: null,
                    communityIdentifier: communityIdentifier,
                    search: null
                )
                .Include(
                    authorEntity: false,
                    communityEntity: false,
                    requestingUser: requestingUser
                )
                .Sort(sort)
                .Take(Configs.Current.QueryResult.TotalLimit)
                .ToListAsync();

            if (requestingUser != null)
                postEntities.ForEach(e => e.CheckRequestingUserVote(requestingUser));

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT",
                parameters: new { postEntities = postEntities.Select(e => e.Id) });
            return postEntities.Select(_postConverter.ConvertToModel).ToList();
        }

        public async Task<List<Post>> ListPostsAsync(string search, SortType sort,
            User requestingUser = null)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new
            {
                search,
                sort,
                requestingUser = requestingUser?.Id
            });
            var sw = Stopwatch.StartNew();
            var postEntities = await _postEntities
                .AsNoTracking()
                .Search(
                    authorId: null,
                    self: null,
                    joined: null,
                    communityIdentifier: null,
                    search: search
                )
                .Include(
                    authorEntity: false,
                    communityEntity: false,
                    requestingUser: requestingUser
                )
                .Sort(sort)
                .Take(Configs.Current.QueryResult.TotalLimit)
                .ToListAsync();

            if (requestingUser != null)
                postEntities.ForEach(e => e.CheckRequestingUserVote(requestingUser));

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT",
                parameters: new { postEntities = postEntities.Select(e => e.Id) });
            return postEntities.Select(_postConverter.ConvertToModel).ToList();
        }

        public async Task<List<Post>> ListPostsByIdsAsync(List<ulong> Ids,
            User requestingUser = null)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new
            {
                Ids,
                requestingUser = requestingUser?.Id
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

            if (requestingUser != null)
                postEntities.ForEach(e => e.CheckRequestingUserVote(requestingUser));

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT",
                parameters: new { postEntities = postEntities.Select(e => e.Id) });
            return postEntities.Select(_postConverter.ConvertToModel).ToList();
        }

        public async Task<List<Post>> ListSelfPostsAsync(User author,
            SortType sort)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new
            {
                authorId = author.Id,
                sort
            });
            var sw = Stopwatch.StartNew();
            var postEntities = await _postEntities
                .AsNoTracking()
                .Search(
                    authorId: author.Id,
                    self: null,
                    joined: null,
                    communityIdentifier: null,
                    search: null
                )
                .Include(
                    authorEntity: false,
                    communityEntity: false,
                    requestingUser: author
                )
                .Sort(sort)
                .Take(Configs.Current.QueryResult.TotalLimit)
                .ToListAsync();

            postEntities.ForEach(e => e.CheckRequestingUserVote(author));

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT",
                parameters: new { postEntities = postEntities.Select(e => e.Id) });
            return postEntities.Select(_postConverter.ConvertToModel).ToList();
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

            if (postEntity != null && requestingUser != null)
                postEntity.CheckRequestingUserVote(requestingUser);

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
                throw new InternalServerException("Can't update the postEntity DbUpdateConcurrencyException!",
                    parameters: postEntity, systemException: ex);
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
            string search)
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
}
