using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Models;
using FireplaceApi.Infrastructure.Converters;
using FireplaceApi.Infrastructure.Entities;
using FireplaceApi.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
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
        private readonly FireplaceApiDbContext _dbContext;
        private readonly QueryResultConverter _queryResultConverter;

        public QueryResultRepository(ILogger<QueryResultRepository> logger,
            FireplaceApiDbContext dbContext, QueryResultConverter queryResultConverter)
        {
            _logger = logger;
            _dbContext = dbContext;
            _queryResultConverter = queryResultConverter;
        }

        public async Task<QueryResult> GetQueryResultByPointerAsync(ModelName modelName, string pointer)
        {
            return modelName switch
            {
                ModelName.COMMUNITY => await GetQueryResultByPointerAsync(
                    _dbContext.CommunityQueryResultEntities, pointer),

                ModelName.COMMUNITY_MEMBERSHIP => await GetQueryResultByPointerAsync(
                    _dbContext.CommunityMembershipQueryResultEntities, pointer),

                ModelName.POST => await GetQueryResultByPointerAsync(
                    _dbContext.PostQueryResultEntities, pointer),

                ModelName.COMMENT => await GetQueryResultByPointerAsync(
                    _dbContext.CommentQueryResultEntities, pointer),

                _ => throw new NotImplementedException($"Model name {modelName} is not supported!"),
            };
        }

        private async Task<QueryResult> GetQueryResultByPointerAsync<T>(DbSet<T> entities, string pointer)
            where T : QueryResultEntity
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { pointer });
            var sw = Stopwatch.StartNew();

            var queryResultEntity = await entities
                .AsNoTracking()
                .Where(e => e.Pointer == pointer)
                .Include<T>(
                )
                .SingleOrDefaultAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { queryResultEntity });
            return _queryResultConverter.ConvertToModel(queryResultEntity);
        }

        public async Task<QueryResult> CreateQueryResultAsync(ModelName modelName, ulong id, string pointer,
            int lastStart, int lastEnd, int lastLimit, int lastPage, List<ulong> referenceEntityIds)
        {
            return modelName switch
            {
                ModelName.COMMUNITY => await CreateQueryResultAsync(
                    _dbContext.CommunityQueryResultEntities, id, pointer, lastStart,
                    lastEnd, lastLimit, lastPage, referenceEntityIds),

                ModelName.COMMUNITY_MEMBERSHIP => await CreateQueryResultAsync(
                    _dbContext.CommunityMembershipQueryResultEntities, id, pointer, lastStart,
                    lastEnd, lastLimit, lastPage, referenceEntityIds),

                ModelName.POST => await CreateQueryResultAsync(
                    _dbContext.PostQueryResultEntities, id, pointer, lastStart,
                    lastEnd, lastLimit, lastPage, referenceEntityIds),

                ModelName.COMMENT => await CreateQueryResultAsync(
                    _dbContext.CommentQueryResultEntities, id, pointer, lastStart,
                    lastEnd, lastLimit, lastPage, referenceEntityIds),

                _ => throw new NotImplementedException($"Model name {modelName} is not supported!"),
            };
        }

        public async Task<QueryResult> CreateQueryResultAsync<T>(DbSet<T> entities, ulong id,
            string pointer, int lastStart, int lastEnd, int lastLimit, int lastPage,
            List<ulong> referenceEntityIds) where T : QueryResultEntity, new()
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new
            {
                id,
                pointer,
                lastStart,
                lastEnd,
                lastLimit,
                lastPage,
                referenceEntityIds
            });
            var sw = Stopwatch.StartNew();
            var queryResultEntity = new T();
            queryResultEntity.FillParameters(id, pointer, lastStart,
                lastEnd, lastLimit, lastPage, referenceEntityIds.ToDecimals());
            entities.Add(queryResultEntity);
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachAllEntries();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { queryResultEntity });
            return _queryResultConverter.ConvertToModel(queryResultEntity);
        }

        public async Task<QueryResult> UpdateQueryResultAsync(ModelName modelName, QueryResult queryResult)
        {
            return modelName switch
            {
                ModelName.COMMUNITY => await UpdateQueryResultAsync(
                    _dbContext.CommunityQueryResultEntities, queryResult),

                ModelName.COMMUNITY_MEMBERSHIP => await UpdateQueryResultAsync(
                    _dbContext.CommunityMembershipQueryResultEntities, queryResult),

                ModelName.POST => await UpdateQueryResultAsync(
                    _dbContext.PostQueryResultEntities, queryResult),

                ModelName.COMMENT => await UpdateQueryResultAsync(
                    _dbContext.CommentQueryResultEntities, queryResult),

                _ => throw new NotImplementedException($"Model name {modelName} is not supported!"),
            };
        }

        public async Task<QueryResult> UpdateQueryResultAsync<T>(DbSet<T> entities,
            QueryResult queryResult) where T : QueryResultEntity, new()
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { queryResult });
            var sw = Stopwatch.StartNew();
            var queryResultEntity = _queryResultConverter.ConvertToEntity<T>(queryResult);
            entities.Update(queryResultEntity);
            try
            {
                await _dbContext.SaveChangesAsync();
                _dbContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var serverMessage = $"Can't update the queryResultEntity DbUpdateConcurrencyException. {queryResultEntity.ToJson()}";
                throw new ApiException(ErrorName.INTERNAL_SERVER, serverMessage, systemException: ex);
            }

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { queryResultEntity });
            return _queryResultConverter.ConvertToModel(queryResultEntity);
        }

        public async Task DeleteQueryResultByPointerAsync(ModelName modelName, string pointer)
        {
            switch (modelName)
            {
                case ModelName.COMMUNITY:
                    await DeleteQueryResultByPointerAsync(
                        _dbContext.CommunityQueryResultEntities, pointer);
                    break;
                case ModelName.COMMUNITY_MEMBERSHIP:
                    await DeleteQueryResultByPointerAsync(
                        _dbContext.CommunityMembershipQueryResultEntities, pointer);
                    break;
                case ModelName.POST:
                    await DeleteQueryResultByPointerAsync(
                        _dbContext.PostQueryResultEntities, pointer);
                    break;
                case ModelName.COMMENT:
                    await DeleteQueryResultByPointerAsync(
                        _dbContext.CommentQueryResultEntities, pointer);
                    break;
                default:
                    throw new NotImplementedException($"Model name {modelName} is not supported!");
            }
        }

        public async Task DeleteQueryResultByPointerAsync<T>(DbSet<T> entities, string pointer)
            where T : QueryResultEntity
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { pointer });
            var sw = Stopwatch.StartNew();
            var queryResultEntity = await entities
                .Where(e => e.Pointer == pointer)
                .SingleOrDefaultAsync();

            entities.Remove(queryResultEntity);
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachAllEntries();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { queryResultEntity });
        }

        public async Task<bool> DoesQueryResultIdExistAsync(ModelName modelName, ulong id)
        {
            return modelName switch
            {
                ModelName.COMMUNITY => await DoesQueryResultIdExistAsync(
                    _dbContext.CommunityQueryResultEntities, id),

                ModelName.COMMUNITY_MEMBERSHIP => await DoesQueryResultIdExistAsync(
                    _dbContext.CommunityMembershipQueryResultEntities, id),

                ModelName.POST => await DoesQueryResultIdExistAsync(
                    _dbContext.PostQueryResultEntities, id),

                ModelName.COMMENT => await DoesQueryResultIdExistAsync(
                    _dbContext.CommentQueryResultEntities, id),

                _ => throw new NotImplementedException($"Model name {modelName} is not supported!"),
            };
        }

        public async Task<bool> DoesQueryResultIdExistAsync<T>(DbSet<T> entities, ulong id)
            where T : QueryResultEntity
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { id });
            var sw = Stopwatch.StartNew();
            var doesExist = await entities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .AnyAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { doesExist });
            return doesExist;
        }

        public async Task<bool> DoesQueryResultPointerExistAsync(ModelName modelName, string pointer)
        {
            return modelName switch
            {
                ModelName.COMMUNITY => await DoesQueryResultPointerExistAsync(
                    _dbContext.CommunityQueryResultEntities, pointer),

                ModelName.COMMUNITY_MEMBERSHIP => await DoesQueryResultPointerExistAsync(
                    _dbContext.CommunityMembershipQueryResultEntities, pointer),

                ModelName.POST => await DoesQueryResultPointerExistAsync(
                    _dbContext.PostQueryResultEntities, pointer),

                ModelName.COMMENT => await DoesQueryResultPointerExistAsync(
                    _dbContext.CommentQueryResultEntities, pointer),

                _ => throw new NotImplementedException($"Model name {modelName} is not supported!"),
            };
        }

        public async Task<bool> DoesQueryResultPointerExistAsync<T>(DbSet<T> entities, string pointer)
            where T : QueryResultEntity
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { pointer });
            var sw = Stopwatch.StartNew();
            var doesExist = await entities
                .AsNoTracking()
                .Where(e => e.Pointer == pointer)
                .AnyAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { doesExist });
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
