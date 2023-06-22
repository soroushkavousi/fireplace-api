using FireplaceApi.Presentation.Dtos;
using System;
using System.Linq;

namespace FireplaceApi.Presentation.Converters;

public static class ConverterUtils
{
    public static QueryResultDto<DTO> ToDto<M, DTO>(this QueryResult<M> queryResult, Func<M, DTO> ToDtoFunction)
        where M : BaseModel
    {
        if (queryResult == null)
            return null;

        var itemDtos = queryResult.Items?.Select(model => ToDtoFunction(model))?.ToList();
        var moreItemIds = queryResult.MoreItemIds?.Select(id => id.IdEncode())?.ToList();

        var queryResultDto = new QueryResultDto<DTO>(itemDtos, moreItemIds);
        return queryResultDto;
    }
}
