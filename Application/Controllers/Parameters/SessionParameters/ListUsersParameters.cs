using FireplaceApi.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FireplaceApi.Application.Controllers
{
    public class ListSessionsOutputHeaderParameters : IOutputHeaderParameters
    {
        public HeaderDictionary GetHeaderDictionary()
        {
            return default;
        }
    }
}
