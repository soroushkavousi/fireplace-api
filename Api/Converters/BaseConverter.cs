using FireplaceApi.Api.Controllers;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Tools;
using FireplaceApi.Core.ValueObjects;
using System.Linq;

namespace FireplaceApi.Api.Converters
{
    public abstract class BaseConverter<M, DTO>
        where M : BaseModel
        where DTO : class
    {

        public abstract DTO ConvertToDto(M model);

        public QueryResultDto<DTO> ConvertToDto(QueryResult<M> queryResult)
        {
            if (queryResult == null)
                return null;

            var itemDtos = queryResult.Items?.Select(model => ConvertToDto(model))?.ToList();
            var moreItemIds = queryResult.MoreItemIds?.Select(id => id.IdEncode())?.ToList();

            var queryResultDto = new QueryResultDto<DTO>(itemDtos, moreItemIds);
            return queryResultDto;
        }
    }
}
