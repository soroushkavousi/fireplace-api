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
        private readonly QueryResultConverter _queryResultConverter;

        public QueryResultRepository(ILogger<QueryResultRepository> logger, IConfiguration configuration, 
            FireplaceApiContext fireplaceApiContext, QueryResultConverter queryResultConverter)
        {
            _logger = logger;
            _configuration = configuration;
            _fireplaceApiContext = fireplaceApiContext;
            _queryResultConverter = queryResultConverter;
        }

        public async Task<QueryResult> GetQueryResultByPointerAsync(ModelName modelName, string pointer)
        {
            switch (modelName)
            {
                case ModelName.COMMUNITY:
                    return await GetQueryResultByPointerAsync(
                        _fireplaceApiContext.CommunityQueryResultEntities, pointer);
                case ModelName.COMMUNITY_MEMBERSHIP:
                    return await GetQueryResultByPointerAsync(
                        _fireplaceApiContext.CommunityMembershipQueryResultEntities, pointer);
                default:
                    throw new NotImplementedException($"Model name {modelName} is not supported!");
            }
        }

        private async Task<QueryResult> GetQueryResultByPointerAsync<T>(DbSet<T> entities, string pointer) where T: QueryResultEntity
        {
            var sw = Stopwatch.StartNew();

            var queryResultEntity = await entities
                .AsNoTracking()
                .Where(e => e.Pointer == pointer)
                .Include<T>(
                )
                .SingleOrDefaultAsync();

            _logger.LogIOInformation(sw, "Database", new { pointer }, new { queryResultEntity });
            return _queryResultConverter.ConvertToModel(queryResultEntity);
        }

        public async Task<QueryResult> CreateQueryResultAsync(ModelName modelName, string pointer,
            int lastStart, int lastEnd, int lastLimit, int lastPage, List<long> referenceEntityIds)
        {
            switch (modelName)
            {
                case ModelName.COMMUNITY:
                    return await CreateQueryResultAsync(
                        _fireplaceApiContext.CommunityQueryResultEntities, pointer, lastStart,
                            lastEnd, lastLimit, lastPage, referenceEntityIds);
                case ModelName.COMMUNITY_MEMBERSHIP:
                    return await CreateQueryResultAsync(
                        _fireplaceApiContext.CommunityMembershipQueryResultEntities, pointer, lastStart,
                            lastEnd, lastLimit, lastPage, referenceEntityIds);
                default:
                    throw new NotImplementedException($"Model name {modelName} is not supported!");
            }
        }

        public async Task<QueryResult> CreateQueryResultAsync<T>(DbSet<T> entities, string pointer,
            int lastStart, int lastEnd, int lastLimit, int lastPage, 
            List<long> referenceEntityIds) where T: QueryResultEntity, new()
        {
            var sw = Stopwatch.StartNew();
            var queryResultEntity = new T();
            queryResultEntity.FillParameters(pointer, lastStart,
                lastEnd, lastLimit, lastPage, referenceEntityIds);
            entities.Add(queryResultEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database", new { pointer, referenceEntityIds }, new { queryResultEntity });
            return _queryResultConverter.ConvertToModel(queryResultEntity);
        }

        public async Task<QueryResult> UpdateQueryResultAsync(ModelName modelName, QueryResult queryResult)
        {
            switch (modelName)
            {
                case ModelName.COMMUNITY:
                    return await UpdateQueryResultAsync(
                        _fireplaceApiContext.CommunityQueryResultEntities, queryResult);
                case ModelName.COMMUNITY_MEMBERSHIP:
                    return await UpdateQueryResultAsync(
                        _fireplaceApiContext.CommunityMembershipQueryResultEntities, queryResult);
                default:
                    throw new NotImplementedException($"Model name {modelName} is not supported!");
            }
        }

        public async Task<QueryResult> UpdateQueryResultAsync<T>(DbSet<T> entities, 
            QueryResult queryResult) where T : QueryResultEntity, new()
        {
            var sw = Stopwatch.StartNew();
            var queryResultEntity = _queryResultConverter.ConvertToEntity<T>(queryResult);
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

            _logger.LogIOInformation(sw, "Database", new { queryResult }, new { queryResultEntity });
            return _queryResultConverter.ConvertToModel(queryResultEntity);
        }

        public async Task DeleteQueryResultByPointerAsync(ModelName modelName, string pointer)
        {
            switch (modelName)
            {
                case ModelName.COMMUNITY:
                    await DeleteQueryResultByPointerAsync(
                        _fireplaceApiContext.CommunityQueryResultEntities, pointer);
                    break;
                case ModelName.COMMUNITY_MEMBERSHIP:
                    await DeleteQueryResultByPointerAsync(
                        _fireplaceApiContext.CommunityMembershipQueryResultEntities, pointer);
                    break;
                default:
                    throw new NotImplementedException($"Model name {modelName} is not supported!");
            }
        }

        public async Task DeleteQueryResultByPointerAsync<T>(DbSet<T> entities, string pointer) 
            where T : QueryResultEntity
        {
            var sw = Stopwatch.StartNew();
            var queryResultEntity = await entities
                .Where(e => e.Pointer == pointer)
                .SingleOrDefaultAsync();

            entities.Remove(queryResultEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database", new { pointer }, new { queryResultEntity });
        }

        public async Task<bool> DoesQueryResultPointerExistAsync(ModelName modelName, string pointer)
        {
            switch (modelName)
            {
                case ModelName.COMMUNITY:
                    return await DoesQueryResultPointerExistAsync(
                        _fireplaceApiContext.CommunityQueryResultEntities, pointer);
                case ModelName.COMMUNITY_MEMBERSHIP:
                    return await DoesQueryResultPointerExistAsync(
                        _fireplaceApiContext.CommunityMembershipQueryResultEntities, pointer);
                default:
                    throw new NotImplementedException($"Model name {modelName} is not supported!");
            }
        }

        public async Task<bool> DoesQueryResultPointerExistAsync<T>(DbSet<T> entities, string pointer)
            where T : QueryResultEntity
        {
            var sw = Stopwatch.StartNew();
            var doesExist = await entities
                .AsNoTracking()
                .Where(e => e.Pointer == pointer)
                .AnyAsync();

            _logger.LogIOInformation(sw, "Database", new { pointer }, new { doesExist });
            return doesExist;
        }
    }

    public static class QueryResultRepositoryExtensions
    {
        public static IQueryable<T> Include<T>(
            [NotNull] this IQueryable<T> queryResultEntitiesQuery)
            where T : QueryResultEntity
        {
            return queryResultEntitiesQuery;
        }
    }
}
