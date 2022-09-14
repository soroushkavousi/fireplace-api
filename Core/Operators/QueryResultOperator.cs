using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Tools;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Operators
{
    public class QueryResultOperator
    {
        private readonly ILogger<QueryResultOperator> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IQueryResultRepository _queryResultRepository;

        public QueryResultOperator(ILogger<QueryResultOperator> logger,
            IServiceProvider serviceProvider, IQueryResultRepository queryResultRepository)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _queryResultRepository = queryResultRepository;
        }

        public async Task<QueryResult> GetQueryResultByPointerAsync(ModelName modelName, string pointer)
        {
            var queryResult = await _queryResultRepository.GetQueryResultByPointerAsync(modelName, pointer);
            if (queryResult == null)
                return queryResult;

            return queryResult;
        }

        public async Task<QueryResult> CreateQueryResultAsync(ModelName modelName,
            int lastStart, int lastEnd, int lastLimit, int lastPage, List<ulong> referenceIds,
            string pointer = null)
        {
            if (pointer == null)
                pointer = await GenerateUniquePointerAsync(modelName);
            var id = await IdGenerator.GenerateNewIdAsync((id) => DoesQueryResultIdExistAsync(modelName, id));
            var queryResult = await _queryResultRepository.CreateQueryResultAsync(modelName,
                id, pointer, lastStart, lastEnd, lastLimit, lastPage, referenceIds);
            return queryResult;
        }

        public async Task DeleteQueryResultByPointerAsync(ModelName modelName, string pointer)
        {
            await _queryResultRepository.DeleteQueryResultByPointerAsync(modelName, pointer);
        }

        public async Task<bool> DoesQueryResultPointerExistAsync(ModelName modelName, string pointer)
        {
            return await _queryResultRepository.DoesQueryResultPointerExistAsync(modelName, pointer);
        }

        public async Task<bool> DoesQueryResultIdExistAsync(ModelName modelName, ulong id)
        {
            return await _queryResultRepository.DoesQueryResultIdExistAsync(modelName, id);
        }

        private async Task<string> GenerateUniquePointerAsync(ModelName modelName)
        {
            string pointer;
            do
            {
                var generatedPointerLength = Configs.Current.Pagination.GeneratedPointerLength;
                pointer = Utils.GenerateRandomString(generatedPointerLength);
            } while (await DoesQueryResultPointerExistAsync(modelName, pointer));

            return pointer;
        }

        public async Task<QueryResult> ApplyQueryResultChanges(ModelName modelName,
            QueryResult queryResult, int? lastStart = null, int? lastEnd = null,
            int? lastLimit = null, int? lastPage = null)
        {
            if (lastStart != null)
            {
                queryResult.LastStart = lastStart.Value;
            }

            if (lastEnd != null)
            {
                queryResult.LastEnd = lastEnd.Value;
            }

            if (lastLimit != null)
            {
                queryResult.LastLimit = lastLimit.Value;
            }

            if (lastPage != null)
            {
                queryResult.LastPage = lastPage.Value;
            }

            queryResult = await _queryResultRepository.UpdateQueryResultAsync(modelName, queryResult);
            return queryResult;
        }
    }
}
