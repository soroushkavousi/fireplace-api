using FireplaceApi.Core.Models;
using FireplaceApi.Infrastructure.Entities;
using FireplaceApi.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;
using System;

namespace FireplaceApi.Infrastructure.Converters
{
    public class QueryResultConverter
    {
        private readonly ILogger<QueryResultConverter> _logger;
        private readonly IServiceProvider _serviceProvider;

        public QueryResultConverter(ILogger<QueryResultConverter> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        // Entity

        public T ConvertToEntity<T>(QueryResult queryResult) where T : QueryResultEntity, new()
        {
            if (queryResult == null)
                return null;

            var queryResultEntity = new T();
            queryResultEntity.FillParameters(queryResult.Id, queryResult.Pointer,
                queryResult.LastStart, queryResult.LastEnd, queryResult.LastLimit,
                queryResult.LastPage, queryResult.ReferenceIds.ToDecimals(), queryResult.CreationDate,
                queryResult.ModifiedDate);

            return queryResultEntity;
        }

        public QueryResult ConvertToModel(QueryResultEntity queryResultEntity)
        {
            if (queryResultEntity == null)
                return null;

            var queryResult = new QueryResult(queryResultEntity.Id, queryResultEntity.Pointer,
                queryResultEntity.LastStart, queryResultEntity.LastEnd, queryResultEntity.LastLimit,
                queryResultEntity.LastPage, queryResultEntity.ReferenceEntityIds.ToUlongs(),
                queryResultEntity.CreationDate, queryResultEntity.ModifiedDate);

            return queryResult;
        }
    }
}
