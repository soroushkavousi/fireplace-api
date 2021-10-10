using FireplaceApi.Api.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FireplaceApi.Api.Controllers
{
    public class ControllerListSessionsOutputHeaderParameters : IControllerOutputHeaderParameters
    {
        public HeaderDictionary GetHeaderDictionary()
        {
            return default;
        }
    }
}
