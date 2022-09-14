using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Validators
{
    public class QueryResultValidator : ApiValidator
    {
        private readonly ILogger<QueryResultValidator> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly QueryResultOperator _queryResultOperator;

        public QueryResultValidator(ILogger<QueryResultValidator> logger,
            IServiceProvider serviceProvider, QueryResultOperator queryResultOperator)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _queryResultOperator = queryResultOperator;
        }

        public async Task ValidatePaginationInputParameters(PaginationInputParameters pagination,
            ModelName modelName)
        {
            if (pagination.Limit.HasValue)
                ValidatePaginationLimitFormat(pagination.Limit.Value);

            if (string.IsNullOrWhiteSpace(pagination.Pointer))
            {
                CheckIfPointerIsNeeded(pagination);
                return;
            }

            ValidatePaginationPointerFormat(pagination.Pointer);
            var queryResult = await ValidatePaginationPointerExist(pagination.Pointer, modelName);
            var totalItemsCount = queryResult.ReferenceIds.Count;
            var limit = pagination.Limit ?? queryResult.LastLimit;
            var totalPagesCount = (int)MathF.Ceiling((float)totalItemsCount / limit);

            if (pagination.Page.HasValue)
                ValidatePaginationPageFormat(pagination.Page.Value, totalPagesCount);

            if (pagination.Offset.HasValue)
                ValidatePaginationOffsetFormat(pagination.Offset.Value, totalItemsCount);
        }

        private static void ValidatePaginationLimitFormat(int limit)
        {
            if (limit < 3)
            {
                var serverMessage = $"Pagination limit {limit} is below its minimum!";
                throw new ApiException(ErrorName.PAGINATION_LIMIT_MIN_LIMIT, serverMessage);
            }

            if (limit > Configs.Current.Pagination.MaximumOfPageItemsCount)
            {
                var serverMessage = $"Pagination limit {limit} is exceed its maximum!";
                throw new ApiException(ErrorName.PAGINATION_LIMIT_MAX_LIMIT, serverMessage);
            }
        }

        private static void CheckIfPointerIsNeeded(PaginationInputParameters pagination)
        {
            if (pagination.Next.HasValue || pagination.Previous.HasValue
                || pagination.Page.HasValue || pagination.Offset.HasValue)
            {
                var serverMessage = $"(PAGINATION) Paramters " +
                    $"[\"next\", \"previous\", \"page\", \"offset\"] " +
                    $"can't be used without pointer.";
                throw new ApiException(ErrorName.PAGINATION_POINTER_IS_MISSING, serverMessage);
            }
        }

        private static void ValidatePaginationPointerFormat(string pointer)
        {
            if (pointer.Length < 6)
            {
                var serverMessage = $"Pagination pointer ({pointer}) is not valid!";
                throw new ApiException(ErrorName.PAGINATION_PAGE_POINTER_NOT_VALID, serverMessage);
            }
        }

        private async Task<QueryResult> ValidatePaginationPointerExist(string pointer, ModelName modelName)
        {
            var queryResult = await _queryResultOperator.GetQueryResultByPointerAsync(modelName, pointer);
            if (queryResult == null)
            {
                var serverMessage = $"Pagination pointer {pointer} in model {modelName} is not exists!";
                throw new ApiException(ErrorName.PAGINATION_POINTER_DOES_NOT_EXIST, serverMessage);
            }

            return queryResult;
        }

        private static void ValidatePaginationPageFormat(int page, int totalPagesCount)
        {
            if (page < 0 || totalPagesCount <= page)
            {
                var serverMessage = $"Pagination page ({page}) is out of range!";
                throw new ApiException(ErrorName.PAGINATION_PAGE_OUT_OF_RANGE, serverMessage);
            }
        }

        private static void ValidatePaginationOffsetFormat(int offset, int totalItemsCount)
        {
            if (offset < 0 || totalItemsCount <= offset)
            {
                var serverMessage = $"Pagination offset {offset} is out of range!";
                throw new ApiException(ErrorName.PAGINATION_OFFSET_OUT_OF_RANGE, serverMessage);
            }
        }
    }
}
