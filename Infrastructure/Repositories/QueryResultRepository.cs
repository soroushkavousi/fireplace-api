using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Models;
using FireplaceApi.Infrastructure.Converters;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

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
            return modelName switch
            {
                ModelName.COMMUNITY => await GetQueryResultByPointerAsync(
                    _fireplaceApiContext.CommunityQueryResultEntities, pointer),

                ModelName.COMMUNITY_MEMBERSHIP => await GetQueryResultByPointerAsync(
                    _fireplaceApiContext.CommunityMembershipQueryResultEntities, pointer),

                ModelName.POST => await GetQueryResultByPointerAsync(
                    _fireplaceApiContext.PostQueryResultEntities, pointer),

                ModelName.COMMENT => await GetQueryResultByPointerAsync(
                    _fireplaceApiContext.CommentQueryResultEntities, pointer),

                _ => throw new NotImplementedException($"Model name {modelName} is not supported!"),
            };
        }

        private async Task<QueryResult> GetQueryResultByPointerAsync<T>(DbSet<T> entities, string pointer)
            where T : QueryResultEntity
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { pointer });
            var sw = Stopwatch.StartNew();

            var queryResultEntity = await entities
                .AsNoTracking()
                .Where(e => e.Pointer == pointer)
                .Include<T>(
                )
                .SingleOrDefaultAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { queryResultEntity });
            return _queryResultConverter.ConvertToModel(queryResultEntity);
        }

        public async Task<QueryResult> CreateQueryResultAsync(ModelName modelName, ulong id, string pointer,
            int lastStart, int lastEnd, int lastLimit, int lastPage, List<ulong> referenceEntityIds)
        {
            return modelName switch
            {
                ModelName.COMMUNITY => await CreateQueryResultAsync(
                    _fireplaceApiContext.CommunityQueryResultEntities, id, pointer, lastStart,
                    lastEnd, lastLimit, lastPage, referenceEntityIds),

                ModelName.COMMUNITY_MEMBERSHIP => await CreateQueryResultAsync(
                    _fireplaceApiContext.CommunityMembershipQueryResultEntities, id, pointer, lastStart,
                    lastEnd, lastLimit, lastPage, referenceEntityIds),

                ModelName.POST => await CreateQueryResultAsync(
                    _fireplaceApiContext.PostQueryResultEntities, id, pointer, lastStart,
                    lastEnd, lastLimit, lastPage, referenceEntityIds),

                ModelName.COMMENT => await CreateQueryResultAsync(
                    _fireplaceApiContext.CommentQueryResultEntities, id, pointer, lastStart,
                    lastEnd, lastLimit, lastPage, referenceEntityIds),

                _ => throw new NotImplementedException($"Model name {modelName} is not supported!"),
            };
        }

        public async Task<QueryResult> CreateQueryResultAsync<T>(DbSet<T> entities, ulong id,
            string pointer, int lastStart, int lastEnd, int lastLimit, int lastPage,
            List<ulong> referenceEntityIds) where T : QueryResultEntity, new()
        {
            _logger.LogIOInformation(null, "Database | Iutput", new
            {
                id, pointer, lastStart, lastEnd, lastLimit, lastPage, referenceEntityIds
            });
            var sw = Stopwatch.StartNew();
            var queryResultEntity = new T();
            queryResultEntity.FillParameters(id, pointer, lastStart,
                lastEnd, lastLimit, lastPage, referenceEntityIds);
            entities.Add(queryResultEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database | Output", new { queryResultEntity });
            return _queryResultConverter.ConvertToModel(queryResultEntity);
        }

        public async Task<QueryResult> UpdateQueryResultAsync(ModelName modelName, QueryResult queryResult)
        {
            return modelName switch
            {
                ModelName.COMMUNITY => await UpdateQueryResultAsync(
                    _fireplaceApiContext.CommunityQueryResultEntities, queryResult),

                ModelName.COMMUNITY_MEMBERSHIP => await UpdateQueryResultAsync(
                    _fireplaceApiContext.CommunityMembershipQueryResultEntities, queryResult),

                ModelName.POST => await UpdateQueryResultAsync(
                    _fireplaceApiContext.PostQueryResultEntities, queryResult),

                ModelName.COMMENT => await UpdateQueryResultAsync(
                    _fireplaceApiContext.CommentQueryResultEntities, queryResult),

                _ => throw new NotImplementedException($"Model name {modelName} is not supported!"),
            };
        }

        public async Task<QueryResult> UpdateQueryResultAsync<T>(DbSet<T> entities,
            QueryResult queryResult) where T : QueryResultEntity, new()
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { queryResult });
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

            _logger.LogIOInformation(sw, "Database | Output", new { queryResultEntity });
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
                case ModelName.POST:
                    await DeleteQueryResultByPointerAsync(
                        _fireplaceApiContext.PostQueryResultEntities, pointer);
                    break;
                case ModelName.COMMENT:
                    await DeleteQueryResultByPointerAsync(
                        _fireplaceApiContext.CommentQueryResultEntities, pointer);
                    break;
                default:
                    throw new NotImplementedException($"Model name {modelName} is not supported!");
            }
        }

        public async Task DeleteQueryResultByPointerAsync<T>(DbSet<T> entities, string pointer)
            where T : QueryResultEntity
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { pointer });
            var sw = Stopwatch.StartNew();
            var queryResultEntity = await entities
                .Where(e => e.Pointer == pointer)
                .SingleOrDefaultAsync();

            entities.Remove(queryResultEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database | Output", new { queryResultEntity });
        }

        public async Task<bool> DoesQueryResultIdExistAsync(ModelName modelName, ulong id)
        {
            return modelName switch
            {
                ModelName.COMMUNITY => await DoesQueryResultIdExistAsync(
                    _fireplaceApiContext.CommunityQueryResultEntities, id),

                ModelName.COMMUNITY_MEMBERSHIP => await DoesQueryResultIdExistAsync(
                    _fireplaceApiContext.CommunityMembershipQueryResultEntities, id),

                ModelName.POST => await DoesQueryResultIdExistAsync(
                    _fireplaceApiContext.PostQueryResultEntities, id),

                ModelName.COMMENT => await DoesQueryResultIdExistAsync(
                    _fireplaceApiContext.CommentQueryResultEntities, id),

                _ => throw new NotImplementedException($"Model name {modelName} is not supported!"),
            };
        }

        public async Task<bool> DoesQueryResultIdExistAsync<T>(DbSet<T> entities, ulong id)
            where T : QueryResultEntity
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { id });
            var sw = Stopwatch.StartNew();
            var doesExist = await entities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .AnyAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { doesExist });
            return doesExist;
        }

        public async Task<bool> DoesQueryResultPointerExistAsync(ModelName modelName, string pointer)
        {
            return modelName switch
            {
                ModelName.COMMUNITY => await DoesQueryResultPointerExistAsync(
                    _fireplaceApiContext.CommunityQueryResultEntities, pointer),

                ModelName.COMMUNITY_MEMBERSHIP => await DoesQueryResultPointerExistAsync(
                    _fireplaceApiContext.CommunityMembershipQueryResultEntities, pointer),

                ModelName.POST => await DoesQueryResultPointerExistAsync(
                    _fireplaceApiContext.PostQueryResultEntities, pointer),

                ModelName.COMMENT => await DoesQueryResultPointerExistAsync(
                    _fireplaceApiContext.CommentQueryResultEntities, pointer),

                _ => throw new NotImplementedException($"Model name {modelName} is not supported!"),
            };
        }

        public async Task<bool> DoesQueryResultPointerExistAsync<T>(DbSet<T> entities, string pointer)
            where T : QueryResultEntity
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { pointer });
            var sw = Stopwatch.StartNew();
            var doesExist = await entities
                .AsNoTracking()
                .Where(e => e.Pointer == pointer)
                .AnyAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { doesExist });
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
