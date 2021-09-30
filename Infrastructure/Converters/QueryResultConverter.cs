using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FireplaceApi.Infrastructure.Entities;
using System;
using FireplaceApi.Core.Models;

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

        public QueryResultEntity ConvertToEntity(QueryResult queryResult)
        {
            if (queryResult == null)
                return null;

            var queryResultEntity = new QueryResultEntity(queryResult.Pointer, 
                queryResult.LastStart, queryResult.LastEnd, queryResult.LastLimit,
                queryResult.LastPage, queryResult.ReferenceIds, queryResult.CreationDate, 
                queryResult.ModifiedDate, queryResult.Id);

            return queryResultEntity;
        }

        public QueryResult ConvertToModel(QueryResultEntity queryResultEntity)
        {
            if (queryResultEntity == null)
                return null;

            var queryResult = new QueryResult(queryResultEntity.Id.Value, queryResultEntity.Pointer, 
                queryResultEntity.LastStart, queryResultEntity.LastEnd, queryResultEntity.LastLimit,
                queryResultEntity.LastPage, queryResultEntity.ReferenceEntityIds, 
                queryResultEntity.CreationDate, queryResultEntity.ModifiedDate);

            return queryResult;
        }
    }
}
