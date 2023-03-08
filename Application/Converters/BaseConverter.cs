using FireplaceApi.Application.Controllers;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Tools;
using FireplaceApi.Domain.ValueObjects;
using System.Linq;

namespace FireplaceApi.Application.Converters
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
