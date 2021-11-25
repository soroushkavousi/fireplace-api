using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.ValueObjects;
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
    public class GlobalRepository : IGlobalRepository
    {
        private readonly ILogger<GlobalRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly FireplaceApiContext _fireplaceApiContext;
        private readonly DbSet<GlobalEntity> _globalEntities;
        private readonly GlobalConverter _globalConverter;

        public GlobalRepository(ILogger<GlobalRepository> logger,
            IConfiguration configuration, FireplaceApiContext fireplaceApiContext,
            GlobalConverter globalConverter)
        {
            _logger = logger;
            _configuration = configuration;
            _fireplaceApiContext = fireplaceApiContext;
            _globalEntities = fireplaceApiContext.GlobalEntities;
            _globalConverter = globalConverter;
        }

        public async Task<List<Global>> ListGlobalsAsync()
        {
            _logger.LogIOInformation(null, "Database | Input", null);
            var sw = Stopwatch.StartNew();
            var globalEntites = await _globalEntities
                .AsNoTracking()
                .Include(
                )
                .ToListAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { globalEntites });
            return globalEntites.Select(e => _globalConverter.ConvertToModel(e)).ToList();
        }

        public async Task<Global> GetGlobalByIdAsync(ulong globalId)
        {
            _logger.LogIOInformation(null, "Database | Input", new { globalId });
            var sw = Stopwatch.StartNew();
            var globalEntity = await _globalEntities
                .AsNoTracking()
                .Where(e => e.Id == globalId)
                .Include(
                )
                .SingleOrDefaultAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { globalEntity });
            return _globalConverter.ConvertToModel(globalEntity);
        }

        public async Task<Global> CreateGlobalAsync(ulong id, EnvironmentName environmentName,
            GlobalValues globalValues)
        {
            _logger.LogIOInformation(null, "Database | Input", new { id, globalValues });
            var sw = Stopwatch.StartNew();
            var globalEntity = new GlobalEntity(id, environmentName.ToString(), globalValues);
            _globalEntities.Add(globalEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database | Output", new { globalEntity });
            return _globalConverter.ConvertToModel(globalEntity);
        }

        public async Task<Global> UpdateGlobalAsync(Global global)
        {
            _logger.LogIOInformation(null, "Database | Input", new { global });
            var sw = Stopwatch.StartNew();
            var globalEntity = _globalConverter.ConvertToEntity(global);
            _globalEntities.Update(globalEntity);
            try
            {
                await _fireplaceApiContext.SaveChangesAsync();
                _fireplaceApiContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var serverMessage = $"Can't update the globalEntity DbUpdateConcurrencyException. {globalEntity.ToJson()}";
                throw new ApiException(ErrorName.INTERNAL_SERVER, serverMessage, systemException: ex);
            }

            _logger.LogIOInformation(sw, "Database | Output", new { globalEntity });
            return _globalConverter.ConvertToModel(globalEntity);
        }

        public async Task DeleteGlobalAsync(ulong globalId)
        {
            _logger.LogIOInformation(null, "Database | Input", new { globalId });
            var sw = Stopwatch.StartNew();
            var globalEntity = await _globalEntities
                .Where(e => e.Id == globalId)
                .SingleOrDefaultAsync();

            _globalEntities.Remove(globalEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database | Output", new { globalEntity });
        }

        public async Task<bool> DoesGlobalIdExistAsync(ulong id)
        {
            _logger.LogIOInformation(null, "Database | Input", new { id });
            var sw = Stopwatch.StartNew();
            var doesExist = await _globalEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .AnyAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { doesExist });
            return doesExist;
        }
    }

    public static class GlobalRepositoryExtensions
    {
        public static IQueryable<GlobalEntity> Include(
                    [NotNull] this IQueryable<GlobalEntity> globalEntitiesQuery)
        {
            return globalEntitiesQuery;
        }
    }
}
