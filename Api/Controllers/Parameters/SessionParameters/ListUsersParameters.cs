using FireplaceApi.Api.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FireplaceApi.Api.Controllers
{
    public class ListSessionsOutputHeaderParameters : IOutputHeaderParameters
    {
        public HeaderDictionary GetHeaderDictionary()
        {
            return default;
        }
    }
}
