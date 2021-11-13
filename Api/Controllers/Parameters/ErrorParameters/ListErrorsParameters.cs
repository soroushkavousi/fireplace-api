using FireplaceApi.Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FireplaceApi.Api.Controllers
{
    [BindNever]
    public class ListErrorsInputQueryParameters
    {

    }

    public class ListErrorsOutputHeaderParameters : IOutputHeaderParameters
    {
        public HeaderDictionary GetHeaderDictionary()
        {
            return default;
        }
    }
}
