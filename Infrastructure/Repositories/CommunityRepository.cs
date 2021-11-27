using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Identifiers;
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
    public class CommunityRepository : ICommunityRepository
    {
        private readonly ILogger<CommunityRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly FireplaceApiContext _fireplaceApiContext;
        private readonly DbSet<CommunityEntity> _communityEntities;
        private readonly CommunityConverter _communityConverter;

        public CommunityRepository(ILogger<CommunityRepository> logger, IConfiguration configuration,
            FireplaceApiContext fireplaceApiContext, CommunityConverter communityConverter)
        {
            _logger = logger;
            _configuration = configuration;
            _fireplaceApiContext = fireplaceApiContext;
            _communityEntities = fireplaceApiContext.CommunityEntities;
            _communityConverter = communityConverter;
        }

        public async Task<List<Community>> ListCommunitiesAsync(List<ulong> Ids)
        {
            _logger.LogIOInformation(null, "Database | Input", new { Ids });
            var sw = Stopwatch.StartNew();
            var communityEntities = await _communityEntities
                .AsNoTracking()
                .Where(e => Ids.Contains(e.Id))
                .ToListAsync();

            var communityEntityDictionary = new Dictionary<ulong, CommunityEntity>();
            communityEntities.ForEach(e => communityEntityDictionary[e.Id] = e);
            communityEntities = new List<CommunityEntity>();
            Ids.ForEach(id => communityEntities.Add(communityEntityDictionary[id]));

            _logger.LogIOInformation(sw, "Database | Output", new { communityEntities });
            return communityEntities.Select(e => _communityConverter.ConvertToModel(e)).ToList();
        }

        public async Task<List<Community>> ListCommunitiesAsync(string name)
        {
            _logger.LogIOInformation(null, "Database | Input", new { name });
            var sw = Stopwatch.StartNew();
            var communityEntities = await _communityEntities
                .AsNoTracking()
                .Search(
                    identifier: null,
                    search: name,
                    sort: null
                )
                .Take(GlobalOperator.GlobalValues.Pagination.TotalItemsCount)
                .ToListAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { communityEntities });
            return communityEntities.Select(e => _communityConverter.ConvertToModel(e)).ToList();
        }

        public async Task<List<ulong>> ListCommunityIdsAsync(string name)
        {
            _logger.LogIOInformation(null, "Database | Input", new { name });
            var sw = Stopwatch.StartNew();
            var communityEntityIds = await _communityEntities
                .AsNoTracking()
                .Search(
                    identifier: null,
                    search: name,
                    sort: null
                )
                .Take(GlobalOperator.GlobalValues.Pagination.TotalItemsCount)
                .Select(e => e.Id)
                .ToListAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { communityEntityIds });
            return communityEntityIds;
        }

        public async Task<Community> GetCommunityByIdentifierAsync(CommunityIdentifier identifier,
            bool includeCreator = false)
        {
            _logger.LogIOInformation(null, "Database | Input", new { identifier, includeCreator });
            var sw = Stopwatch.StartNew();
            var communityEntity = await _communityEntities
                .AsNoTracking()
                .Search(
                    identifier: identifier,
                    search: null,
                    sort: null
                )
                .Include(
                    creatorEntity: includeCreator
                )
                .SingleOrDefaultAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { communityEntity });
            return _communityConverter.ConvertToModel(communityEntity);
        }

        public async Task<string> GetNameByIdAsync(ulong id)
        {
            _logger.LogIOInformation(null, "Database | Input", new { id });
            var sw = Stopwatch.StartNew();
            var communityName = (await _communityEntities
                .AsNoTracking()
                .Select(e => new { e.Id, e.Name })
                .SingleAsync(e => e.Id == id))
                .Name;

            _logger.LogIOInformation(sw, "Database | Output", new { communityName });
            return communityName;
        }

        public async Task<ulong> GetIdByNameAsync(string communityName)
        {
            _logger.LogIOInformation(null, "Database | Input", new { communityName });
            var sw = Stopwatch.StartNew();
            var communityId = (await _communityEntities
                .AsNoTracking()
                .Select(e => new { e.Id, e.Name })
                .SingleAsync(e => string.Equals(e.Name, communityName)))
                .Id;

            _logger.LogIOInformation(sw, "Database | Output", new { communityId });
            return communityId;
        }

        public async Task<Community> CreateCommunityAsync(ulong id, string name,
            ulong creatorId, string creatorUsername)
        {
            _logger.LogIOInformation(null, "Database | Input",
                new { id, name, creatorId, creatorUsername });
            var sw = Stopwatch.StartNew();
            var communityEntity = new CommunityEntity(id, name, creatorId, creatorUsername);
            _communityEntities.Add(communityEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database | Output", new { communityEntity });
            return _communityConverter.ConvertToModel(communityEntity);
        }

        public async Task<Community> UpdateCommunityAsync(Community community)
        {
            _logger.LogIOInformation(null, "Database | Input", new { community });
            var sw = Stopwatch.StartNew();
            var communityEntity = _communityConverter.ConvertToEntity(community);
            _communityEntities.Update(communityEntity);
            try
            {
                await _fireplaceApiContext.SaveChangesAsync();
                _fireplaceApiContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var serverMessage = $"Can't update the communityEntity DbUpdateConcurrencyException. {communityEntity.ToJson()}";
                throw new ApiException(ErrorName.INTERNAL_SERVER, serverMessage, systemException: ex);
            }

            _logger.LogIOInformation(sw, "Database | Output", new { communityEntity });
            return _communityConverter.ConvertToModel(communityEntity);
        }

        public async Task UpdateCommunityNameAsync(ulong id, string newCommunityName)
        {
            _logger.LogIOInformation(null, "Database | Input", new { id, newCommunityName });
            var sw = Stopwatch.StartNew();
            int rowAffectedCount = 0;
            try
            {
                rowAffectedCount = await _fireplaceApiContext.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL public.\"UpdateCommunityName\"({id}, {newCommunityName});");
                _fireplaceApiContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var serverMessage = $"Can't update the userEntity DbUpdateConcurrencyException. " +
                    $"{new { id, newCommunityName, rowAffectedCount }.ToJson()}";
                throw new ApiException(ErrorName.INTERNAL_SERVER, serverMessage, systemException: ex);
            }

            _logger.LogIOInformation(sw, "Database | Output", new { rowAffectedCount });
        }

        public async Task DeleteCommunityByIdentifierAsync(CommunityIdentifier identifier)
        {
            _logger.LogIOInformation(null, "Database | Input", new { identifier });
            var sw = Stopwatch.StartNew();
            var communityEntity = await _communityEntities
                .Search(
                    identifier: identifier,
                    search: null,
                    sort: null
                )
                .SingleOrDefaultAsync();

            _communityEntities.Remove(communityEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database | Output", new { communityEntity });
        }

        public async Task<bool> DoesCommunityIdentifierExistAsync(CommunityIdentifier identifier)
        {
            _logger.LogIOInformation(null, "Database | Input", new { identifier });
            var sw = Stopwatch.StartNew();
            var doesExist = await _communityEntities
                .AsNoTracking()
                .Search(
                    identifier: identifier,
                    search: null,
                    sort: null
                )
                .AnyAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { doesExist });
            return doesExist;
        }

        public async Task<bool> DoesCommunityNameExistAsync(string name)
        {
            _logger.LogIOInformation(null, "Database | Input", new { name });
            var sw = Stopwatch.StartNew();
            var doesExist = await _communityEntities
                .AsNoTracking()
                .Where(e => e.Name == name)
                .AnyAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { doesExist });
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
            string search, SortType? sort)
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

            if (sort.HasValue)
            {
                switch (sort)
                {
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
