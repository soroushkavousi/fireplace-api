using FireplaceApi.Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FireplaceApi.Api.Controllers
{
    [BindNever]
    public class ControllerListErrorsInputQueryParameters
    {

    }

    public class ControllerListErrorsOutputHeaderParameters : IControllerOutputHeaderParameters
    {
        public HeaderDictionary GetHeaderDictionary()
        {
            return default;
        }
    }
}
