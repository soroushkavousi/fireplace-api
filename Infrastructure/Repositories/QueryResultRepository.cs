using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using FireplaceApi.Infrastructure.Converters;
using FireplaceApi.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.ValueObjects;
using System.Diagnostics;

namespace FireplaceApi.Infrastructure.Repositories
{
    public class QueryResultRepository : IQueryResultRepository
    {
        private readonly ILogger<QueryResultRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly FireplaceApiContext _fireplaceApiContext;
        private readonly Dictionary<ModelName, DbSet<QueryResultEntity>> _queryResultEntitiesDictionary;
        private readonly QueryResultConverter _queryResultConverter;

        public QueryResultRepository(ILogger<QueryResultRepository> logger, IConfiguration configuration, 
            FireplaceApiContext fireplaceApiContext, QueryResultConverter queryResultConverter)
        {
            _logger = logger;
            _configuration = configuration;
            _fireplaceApiContext = fireplaceApiContext;
            _queryResultEntitiesDictionary = new Dictionary<ModelName, DbSet<QueryResultEntity>>()
            {
                [ModelName.COMMUNITY] = fireplaceApiContext.CommunityQueryResultEntities,
            };
            _queryResultConverter = queryResultConverter;
        }

        public async Task<QueryResult> GetQueryResultByPointerAsync(ModelName modelName, string pointer)
        {
            var sw = Stopwatch.StartNew();
            var entities = _queryResultEntitiesDictionary[modelName];

            var queryResultEntity = await entities
                .AsNoTracking()
                .Where(e => e.Pointer == pointer)
                .Include(
                )
                .SingleOrDefaultAsync();

            _logger.LogIOInformation(sw, "Database", new { modelName, pointer }, new { queryResultEntity });
            return _queryResultConverter.ConvertToModel(queryResultEntity);
        }

        public async Task<QueryResult> CreateQueryResultAsync(ModelName modelName, string pointer,
            int lastStart, int lastEnd, int lastLimit, int lastPage, List<long> referenceEntityIds)
        {
            var sw = Stopwatch.StartNew();
            var entities = _queryResultEntitiesDictionary[modelName];
            var queryResultEntity = new QueryResultEntity(pointer, lastStart,
                lastEnd, lastLimit, lastPage, referenceEntityIds);
            entities.Add(queryResultEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database", new { modelName, pointer, referenceEntityIds }, new { queryResultEntity });
            return _queryResultConverter.ConvertToModel(queryResultEntity);
        }

        public async Task<QueryResult> UpdateQueryResultAsync(ModelName modelName, QueryResult queryResult)
        {
            var sw = Stopwatch.StartNew();
            var entities = _queryResultEntitiesDictionary[modelName];
            var queryResultEntity = _queryResultConverter.ConvertToEntity(queryResult);
            entities.Update(queryResultEntity);
            try
            {
                await _fireplaceApiContext.SaveChangesAsync();
                _fireplaceApiContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var serverMessage = $"Can't update the queryResultEntity DbUpdateConcurrencyException. {queryResultEntity.ToJson()}";
                throw new ApiException(ErrorName.INTERNAL_SERVER, serverMessage, systemException: ex);
            }

            _logger.LogIOInformation(sw, "Database", new { modelName, queryResult }, new { queryResultEntity });
            return _queryResultConverter.ConvertToModel(queryResultEntity);
        }

        public async Task DeleteQueryResultByPointerAsync(ModelName modelName, string pointer)
        {
            var sw = Stopwatch.StartNew();
            var entities = _queryResultEntitiesDictionary[modelName];
            var queryResultEntity = await entities
                .Where(e => e.Pointer == pointer)
                .SingleOrDefaultAsync();

            entities.Remove(queryResultEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database", new { modelName, pointer }, new { queryResultEntity });
        }

        public async Task<bool> DoesQueryResultPointerExistAsync(ModelName modelName, string pointer)
        {
            var sw = Stopwatch.StartNew();
            var entities = _queryResultEntitiesDictionary[modelName];
            var doesExist = await entities
                .AsNoTracking()
                .Where(e => e.Pointer == pointer)
                .AnyAsync();

            _logger.LogIOInformation(sw, "Database", new { modelName, pointer }, new { doesExist });
            return doesExist;
        }
    }

    public static class QueryResultRepositoryExtensions
    {
        public static IQueryable<QueryResultEntity> Include(
            [NotNull] this IQueryable<QueryResultEntity> queryResultEntitiesQuery)
        {
            return queryResultEntitiesQuery;
        }
    }
}
