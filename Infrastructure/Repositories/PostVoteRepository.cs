﻿using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
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
    public class PostVoteRepository : IPostVoteRepository
    {
        private readonly ILogger<PostVoteRepository> _logger;
        private readonly FireplaceApiDbContext _dbContext;
        private readonly DbSet<PostVoteEntity> _postVoteEntities;
        private readonly PostVoteConverter _postVoteConverter;

        public PostVoteRepository(ILogger<PostVoteRepository> logger,
            FireplaceApiDbContext dbContext, PostVoteConverter postVoteConverter)
        {
            _logger = logger;
            _dbContext = dbContext;
            _postVoteEntities = dbContext.PostVoteEntities;
            _postVoteConverter = postVoteConverter;
        }

        public async Task<List<PostVote>> ListPostVotesAsync(List<ulong> Ids)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { Ids });
            var sw = Stopwatch.StartNew();
            var postEntities = await _postVoteEntities
                .AsNoTracking()
                .Where(e => Ids.Contains(e.Id))
                .ToListAsync();

            var postEntityDictionary = new Dictionary<ulong, PostVoteEntity>();
            postEntities.ForEach(e => postEntityDictionary[e.Id] = e);
            postEntities = new List<PostVoteEntity>();
            Ids.ForEach(id => postEntities.Add(postEntityDictionary[id]));

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT",
                parameters: new { postEntities = postEntities.Select(e => e.Id) });
            return postEntities.Select(_postVoteConverter.ConvertToModel).ToList();
        }

        public async Task<PostVote> GetPostVoteByIdAsync(ulong id,
            bool includeVoter = false, bool includePost = false)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new
            {
                id,
                includeVoter,
                includePost
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

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { postEntity });
            return _postVoteConverter.ConvertToModel(postEntity);
        }

        public async Task<PostVote> GetPostVoteAsync(ulong voterId,
            ulong postId, bool includeVoter = false, bool includePost = false)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new
            {
                voterId,
                postId,
                includeVoter,
                includePost
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

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { postEntity });
            return _postVoteConverter.ConvertToModel(postEntity);
        }

        public async Task<PostVote> CreatePostVoteAsync(ulong id, ulong voterUserId,
            string voterUsername, ulong postId, bool isUp)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new
            {
                id,
                voterUserId,
                voterUsername,
                postId,
                isUp
            });
            var sw = Stopwatch.StartNew();
            var postEntity = new PostVoteEntity(id, voterUserId,
                voterUsername, postId, isUp);
            _postVoteEntities.Add(postEntity);
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachAllEntries();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT",
                parameters: new { postEntity });
            return _postVoteConverter.ConvertToModel(postEntity);
        }

        public async Task<PostVote> UpdatePostVoteAsync(PostVote postvote)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { postvote });
            var sw = Stopwatch.StartNew();
            var postEntity = _postVoteConverter.ConvertToEntity(postvote);
            _postVoteEntities.Update(postEntity);
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
            return _postVoteConverter.ConvertToModel(postEntity);
        }

        public async Task DeletePostVoteByIdAsync(ulong id)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { id });
            var sw = Stopwatch.StartNew();
            var postEntity = await _postVoteEntities
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync();

            _postVoteEntities.Remove(postEntity);
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachAllEntries();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { postEntity });
        }

        public async Task<bool> DoesPostVoteIdExistAsync(ulong id)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { id });
            var sw = Stopwatch.StartNew();
            var doesExist = await _postVoteEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .AnyAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { doesExist });
            return doesExist;
        }

        public async Task<bool> DoesPostVoteIdExistAsync(ulong voterId, ulong postId)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { voterId, postId });
            var sw = Stopwatch.StartNew();
            var doesExist = await _postVoteEntities
                .AsNoTracking()
                .Where(e => e.VoterEntityId == voterId
                    && e.PostEntityId == postId)
                .AnyAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { doesExist });
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
            ulong? voterId, ulong? postId, SortType? sort)
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
