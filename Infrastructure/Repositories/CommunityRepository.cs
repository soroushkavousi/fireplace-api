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
    public class CommunityRepository : ICommunityRepository
    {
        private readonly ILogger<CommunityRepository> _logger;
        private readonly ProjectDbContext _dbContext;
        private readonly DbSet<CommunityEntity> _communityEntities;
        private readonly CommunityConverter _communityConverter;

        public CommunityRepository(ILogger<CommunityRepository> logger,
            ProjectDbContext dbContext, CommunityConverter communityConverter)
        {
            _logger = logger;
            _dbContext = dbContext;
            _communityEntities = dbContext.CommunityEntities;
            _communityConverter = communityConverter;
        }

        public async Task<List<Community>> ListCommunitiesAsync(string search, CommunitySortType sort)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { search });
            var sw = Stopwatch.StartNew();
            var communityEntities = await _communityEntities
                .AsNoTracking()
                .Search(
                    identifier: null,
                    search: search
                )
                .Sort(sort)
                .Take(Configs.Current.QueryResult.TotalLimit)
                .ToListAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT",
                parameters: new { communityEntities = communityEntities.Select(e => e.Id) });
            return communityEntities.Select(_communityConverter.ConvertToModel).ToList();
        }

        public async Task<List<Community>> ListCommunitiesByIdsAsync(List<ulong> Ids)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { Ids });
            var sw = Stopwatch.StartNew();
            var communityEntities = await _communityEntities
                .AsNoTracking()
                .Where(e => Ids.Contains(e.Id))
                .ToListAsync();

            var communityEntityDictionary = new Dictionary<ulong, CommunityEntity>();
            communityEntities.ForEach(e => communityEntityDictionary[e.Id] = e);
            communityEntities = new List<CommunityEntity>();
            Ids.ForEach(id => communityEntities.Add(communityEntityDictionary[id]));

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT",
                parameters: new { communityEntities = communityEntities.Select(e => e.Id) });
            return communityEntities.Select(_communityConverter.ConvertToModel).ToList();
        }

        public async Task<Community> GetCommunityByIdentifierAsync(CommunityIdentifier identifier,
            bool includeCreator = false)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { identifier, includeCreator });
            var sw = Stopwatch.StartNew();
            var communityEntity = await _communityEntities
                .AsNoTracking()
                .Search(
                    identifier: identifier,
                    search: null
                )
                .Include(
                    creatorEntity: includeCreator
                )
                .SingleOrDefaultAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { communityEntity });
            return _communityConverter.ConvertToModel(communityEntity);
        }

        public async Task<string> GetNameByIdAsync(ulong id)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { id });
            var sw = Stopwatch.StartNew();
            var communityName = (await _communityEntities
                .AsNoTracking()
                .Select(e => new { e.Id, e.Name })
                .SingleAsync(e => e.Id == id))
                .Name;

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { communityName });
            return communityName;
        }

        public async Task<ulong> GetIdByNameAsync(string communityName)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { communityName });
            var sw = Stopwatch.StartNew();
            var communityId = (await _communityEntities
                .AsNoTracking()
                .Select(e => new { e.Id, e.Name })
                .SingleAsync(e => string.Equals(e.Name, communityName)))
                .Id;

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { communityId });
            return communityId;
        }

        public async Task<Community> CreateCommunityAsync(ulong id, string name,
            ulong creatorId, string creatorUsername)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT",
                parameters: new { id, name, creatorId, creatorUsername });
            var sw = Stopwatch.StartNew();
            var communityEntity = new CommunityEntity(id, name, creatorId, creatorUsername);
            _communityEntities.Add(communityEntity);
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachAllEntries();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { communityEntity });
            return _communityConverter.ConvertToModel(communityEntity);
        }

        public async Task<Community> UpdateCommunityAsync(Community community)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { community });
            var sw = Stopwatch.StartNew();
            var communityEntity = _communityConverter.ConvertToEntity(community);
            _communityEntities.Update(communityEntity);
            try
            {
                await _dbContext.SaveChangesAsync();
                _dbContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new InternalServerException("Can't update the communityEntity DbUpdateConcurrencyException!",
                    parameters: communityEntity, systemException: ex);
            }

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { communityEntity });
            return _communityConverter.ConvertToModel(communityEntity);
        }

        public async Task UpdateCommunityNameAsync(ulong id, string newCommunityName)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { id, newCommunityName });
            var sw = Stopwatch.StartNew();
            int rowAffectedCount = 0;
            try
            {
                rowAffectedCount = await _dbContext.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL public.\"UpdateCommunityName\"({id}, {newCommunityName});");
                _dbContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new InternalServerException("Can't update the community search DbUpdateConcurrencyException!",
                    parameters: new { id, newCommunityName, rowAffectedCount }, systemException: ex);
            }

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { rowAffectedCount });
        }

        public async Task DeleteCommunityByIdentifierAsync(CommunityIdentifier identifier)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { identifier });
            var sw = Stopwatch.StartNew();
            var communityEntity = await _communityEntities
                .Search(
                    identifier: identifier,
                    search: null
                )
                .SingleOrDefaultAsync();

            _communityEntities.Remove(communityEntity);
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachAllEntries();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { communityEntity });
        }

        public async Task<bool> DoesCommunityIdentifierExistAsync(CommunityIdentifier identifier)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { identifier });
            var sw = Stopwatch.StartNew();
            var doesExist = await _communityEntities
                .AsNoTracking()
                .Search(
                    identifier: identifier,
                    search: null
                )
                .AnyAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { doesExist });
            return doesExist;
        }

        public async Task<bool> DoesCommunityNameExistAsync(string name)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { name });
            var sw = Stopwatch.StartNew();
            var doesExist = await _communityEntities
                .AsNoTracking()
                .Where(e => e.Name == name)
                .AnyAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { doesExist });
            return doesExist;
        }
    }

    public static class CommunityRepositoryExtensions
    {
        public static IQueryable<CommunityEntity> Include(
            [NotNull] this IQueryable<CommunityEntity> communityEntitiesQuery,
            bool creatorEntity)
        {
            if (creatorEntity)
                communityEntitiesQuery = communityEntitiesQuery.Include(e => e.CreatorEntity);

            return communityEntitiesQuery;
        }

        public static IQueryable<CommunityEntity> Search(
            [NotNull] this IQueryable<CommunityEntity> q, CommunityIdentifier identifier,
            string search)
        {
            if (identifier != null)
            {
                switch (identifier)
                {
                    case CommunityIdIdentifier idIdentifier:
                        q = q.Where(e => e.Id == idIdentifier.Id);
                        break;
                    case CommunityNameIdentifier nameIdentifier:
                        q = q.Where(e => e.Name == nameIdentifier.Name);
                        break;
                }
            }

            if (!string.IsNullOrWhiteSpace(search))
                q = q.Where(e => EF.Functions
                    .ILike(EF.Functions.Collate(e.Name, "default"), $"%{search}%"));

            return q;
        }

        public static IQueryable<CommunityEntity> Sort(
            [NotNull] this IQueryable<CommunityEntity> q, CommunitySortType sort)
        {
            q = sort switch
            {
                CommunitySortType.NEW => q.OrderByDescending(e => e.CreationDate),
                CommunitySortType.OLD => q.OrderBy(e => e.CreationDate),
                _ => q.OrderByDescending(e => e.CreationDate),
            };

            return q;
        }
    }
}
