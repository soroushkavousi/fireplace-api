using Microsoft.AspNetCore.Http;

namespace FireplaceApi.Api.Interfaces
{
    public interface IOutputHeaderParameters
    {
        public HeaderDictionary GetHeaderDictionary();
    }
}
