using FireplaceApi.Api.Controllers;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.ValueObjects;
using System;
using System.Linq;

namespace FireplaceApi.Api.Converters
{
    public abstract class BaseConverter<M, DTO> 
        where M : class 
        where DTO : class
    {

        public abstract DTO ConvertToDto(M model);

        public PageDto<DTO> ConvertToDto(Page<M> page, string relativeRequestPath)
        {
            if (page == null)
                return null;

            var listPath = $"{GlobalOperator.GlobalValues.Api.BaseUrlPath}{relativeRequestPath}";

            var paginationDto = new PaginationDto(page.QueryResultPointer,
                listPath, page.Number, page.Start, page.End, page.Limit,
                page.TotalItemsCount, page.TotalPagesCount);

            var itemDtos = page.Items.Select(model => ConvertToDto(model)).ToList();

            var pageDto = new PageDto<DTO>(itemDtos, paginationDto);
            return pageDto;
        }
    }
}
