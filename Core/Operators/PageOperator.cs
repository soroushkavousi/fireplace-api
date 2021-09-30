using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FireplaceApi.Core.ValueObjects;
using FireplaceApi.Core.Tools;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Interfaces;

namespace FireplaceApi.Core.Operators
{
    public class PageOperator
    {
        private readonly ILogger<PageOperator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly QueryResultOperator _queryResultOperator;

        public PageOperator(ILogger<PageOperator> logger, IConfiguration configuration,
            IServiceProvider serviceProvider, QueryResultOperator queryResultOperator)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _queryResultOperator = queryResultOperator;
        }


        public async Task<Page<T>> CreatePageWithoutPointerAsync<T>(ModelName modelName,
            PaginationInputParameters paginationInputParameters, List<long> ItemIds,
            Func<List<long>, Task<List<T>>> getItemsAsync)
        {
            var limit = paginationInputParameters.Limit ?? GlobalOperator.GlobalValues.Pagination.MaximumOfPageItemsCount;
            var totalItemsCount = ItemIds.Count;
            var totalPagesCount = totalItemsCount / limit + 1;

            var (start, end, page) = CalculateStartAndEndAndPage(paginationInputParameters, limit,
                totalItemsCount, totalPagesCount, 0, -1, -1);

            var queryResult = await _queryResultOperator.CreateQueryResultAsync(
                    modelName, start, end, limit, page.Value, ItemIds);
            var pointer = queryResult.Pointer;

            var pageItemIds = ItemIds.GetRange(start, end - start + 1);
            var resultItems = await getItemsAsync(pageItemIds);

            var resultPage = new Page<T>(pointer, page,
                start, end, limit, totalItemsCount, totalPagesCount, resultItems);
            return resultPage;
        }

        public async Task<Page<T>> CreatePageWithPointerAsync<T>(ModelName modelName,
            PaginationInputParameters paginationInputParameters,
            Func<List<long>, Task<List<T>>> getItemsAsync)
        {
            var queryResult = await _queryResultOperator
                .GetQueryResultByPointerAsync(modelName, paginationInputParameters.Pointer);
            var limit = paginationInputParameters.Limit ?? queryResult.LastLimit;
            var totalItemsCount = queryResult.ReferenceIds.Count;
            var totalPagesCount = totalItemsCount / limit + 1;

            var (start, end, page) = CalculateStartAndEndAndPage(paginationInputParameters, limit,
                totalItemsCount, totalPagesCount, queryResult.LastStart, queryResult.LastEnd, 
                queryResult.LastPage);

            queryResult = await _queryResultOperator.ApplyQueryResultChanges(
                    modelName, queryResult, start, end, limit, page);

            var pageItemIds = queryResult.ReferenceIds.GetRange(start, end - start + 1);
            var resultItems = await getItemsAsync(pageItemIds);

            var resultPage = new Page<T>(queryResult.Pointer, page,
                start, end, limit, totalItemsCount, totalPagesCount, resultItems);
            return resultPage;
        }

        public Tuple<int, int, int?> CalculateStartAndEndAndPage(PaginationInputParameters paginationInputParameters,
            int limit, int totalItemsCount, int totalPagesCount, int lastStart, int lastEnd, int lastPage)
        {
            int start, end;
            int? page = null;
            if (paginationInputParameters.Page.HasValue)
            {
                page = paginationInputParameters.Page;
                start = page.Value * limit;
                end = Math.Min(totalItemsCount - 1, start + limit - 1);
            }
            else
            {
                int offset;
                if (paginationInputParameters.Offset.HasValue)
                {
                    offset = paginationInputParameters.Offset.Value;
                    page = null;
                }
                else if (paginationInputParameters.Previous.HasValue)
                {
                    offset = lastStart;
                    page = Math.Max(0, lastPage - 1);
                }
                else
                {
                    offset = lastEnd;
                    page = Math.Min(totalPagesCount - 1, lastPage + 1);
                }

                if (paginationInputParameters.Previous.HasValue)
                {
                    start = Math.Max(0, offset - limit);
                    end = Math.Max(0, offset - 1);
                }
                else
                {
                    start = Math.Min(totalItemsCount - 1, offset + 1);
                    end = Math.Min(totalItemsCount - 1, offset + limit);
                }
            }

            return new Tuple<int, int, int?>(start, end, page);
        }
    }
}
