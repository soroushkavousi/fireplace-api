using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Interfaces
{
    public interface IQueryResultRepository
    {
        public Task<QueryResult> GetQueryResultByPointerAsync(ModelName modelName,
            string pointer);
        public Task<QueryResult> CreateQueryResultAsync(ModelName modelName, string pointer,
            int lastStart, int lastEnd, int lastLimit, int lastPage, List<long> referenceEntityIds);
        public Task<QueryResult> UpdateQueryResultAsync(ModelName modelName, QueryResult queryResult);
        public Task DeleteQueryResultByPointerAsync(ModelName modelName, string pointer);
        public Task<bool> DoesQueryResultPointerExistAsync(ModelName modelName,
            string pointer);
    }
}
