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

        public async Task<List<Community>> ListCommunitiesAsync(List<long> Ids)
        {
            var sw = Stopwatch.StartNew();
            var communityEntities = await _communityEntities
                .AsNoTracking()
                .Where(e => Ids.Contains(e.Id.Value))
                .ToListAsync();

            _logger.LogIOInformation(sw, "Database", new { Ids }, new { communityEntities });
            return communityEntities.Select(e => _communityConverter.ConvertToModel(e)).ToList();
        }

        public async Task<List<Community>> ListCommunitiesAsync(string name)
        {
            var sw = Stopwatch.StartNew();
            var communityEntities = await _communityEntities
                .AsNoTracking()
                .Where(e => e.Name.Contains(name))
                .Take(GlobalOperator.GlobalValues.Pagination.TotalItemsCount)
                .ToListAsync();

            _logger.LogIOInformation(sw, "Database", new { name }, new { communityEntities });
            return communityEntities.Select(e => _communityConverter.ConvertToModel(e)).ToList();
        }

        public async Task<List<long>> ListCommunityIdsAsync(string name)
        {
            var sw = Stopwatch.StartNew();
            var communityEntities = await _communityEntities
                .AsNoTracking()
                .Where(e => e.Name.Contains(name))
                .Take(GlobalOperator.GlobalValues.Pagination.TotalItemsCount)
                .Select(e => e.Id.Value)
                .ToListAsync();

            _logger.LogIOInformation(sw, "Database", new { name }, new { communityEntities });
            return communityEntities;
        }

        public async Task<Community> GetCommunityByIdAsync(long id, bool includeCreator = false)
        {
            var sw = Stopwatch.StartNew();
            var communityEntity = await _communityEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Include(
                    creatorEntity: includeCreator
                )
                .SingleOrDefaultAsync();

            _logger.LogIOInformation(sw, "Database", new { id, includeCreator }, new { communityEntity });
            return _communityConverter.ConvertToModel(communityEntity);
        }

        public async Task<Community> GetCommunityByNameAsync(string name, bool includeCreator = false)
        {
            var sw = Stopwatch.StartNew();
            var communityEntity = await _communityEntities
                .AsNoTracking()
                .Where(e => e.Name == name)
                .Include(
                    creatorEntity: includeCreator
                )
                .SingleOrDefaultAsync();

            _logger.LogIOInformation(sw, "Database", new { name, includeCreator }, new { communityEntity });
            return _communityConverter.ConvertToModel(communityEntity);
        }

        public async Task<string> GetNameByIdAsync(long id)
        {
            var sw = Stopwatch.StartNew();
            var communityName = (await _communityEntities
                .AsNoTracking()
                .Select(e => new { Id = e.Id.Value, e.Name })
                .SingleAsync(e => e.Id == id))
                .Name;

            _logger.LogIOInformation(sw, "Database",
                new { id },
                new { communityName });
            return communityName;
        }

        public async Task<long> GetIdByNameAsync(string communityName)
        {
            var sw = Stopwatch.StartNew();
            var communityId = (await _communityEntities
                .AsNoTracking()
                .Select(e => new { Id = e.Id.Value, e.Name })
                .SingleAsync(e => string.Equals(e.Name, communityName)))
                .Id;

            _logger.LogIOInformation(sw, "Database",
                new { communityName },
                new { communityId });
            return communityId;
        }

        public async Task<Community> CreateCommunityAsync(string name, long creatorId)
        {
            var sw = Stopwatch.StartNew();
            var communityEntity = new CommunityEntity(name, creatorId);
            _communityEntities.Add(communityEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database", new { name, creatorId }, new { communityEntity });
            return _communityConverter.ConvertToModel(communityEntity);
        }

        public async Task<Community> UpdateCommunityAsync(Community community)
        {
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

            _logger.LogIOInformation(sw, "Database", new { community }, new { communityEntity });
            return _communityConverter.ConvertToModel(communityEntity);
        }

        public async Task DeleteCommunityByIdAsync(long id)
        {
            var sw = Stopwatch.StartNew();
            var communityEntity = await _communityEntities
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync();

            _communityEntities.Remove(communityEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database", new { id }, new { communityEntity });
        }

        public async Task DeleteCommunityByNameAsync(string name)
        {
            var sw = Stopwatch.StartNew();
            var communityEntity = await _communityEntities
                .Where(e => e.Name == name)
                .SingleOrDefaultAsync();

            _communityEntities.Remove(communityEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database", new { name }, new { communityEntity });
        }

        public async Task<bool> DoesCommunityIdExistAsync(long id)
        {
            var sw = Stopwatch.StartNew();
            var doesExist = await _communityEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .AnyAsync();

            _logger.LogIOInformation(sw, "Database", new { id }, new { doesExist });
            return doesExist;
        }

        public async Task<bool> DoesCommunityNameExistAsync(string name)
        {
            var sw = Stopwatch.StartNew();
            var doesExist = await _communityEntities
                .AsNoTracking()
                .Where(e => e.Name == name)
                .AnyAsync();

            _logger.LogIOInformation(sw, "Database", new { name }, new { doesExist });
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
    }
}
