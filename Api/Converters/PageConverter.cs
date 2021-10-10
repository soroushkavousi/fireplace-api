using FireplaceApi.Api.Controllers;
using FireplaceApi.Core.ValueObjects;

namespace FireplaceApi.Api.Converters
{
    public static class PageConverter
    {
        public static PaginationInputParameters ConvertToModel(PaginationInputQueryParameters inputQueryParameters)
        {
            var model = new PaginationInputParameters(inputQueryParameters.Limit,
                inputQueryParameters.Pointer, inputQueryParameters.Next,
                inputQueryParameters.Previous, inputQueryParameters.Page,
                inputQueryParameters.Offset);

            return model;
        }
    }
}
