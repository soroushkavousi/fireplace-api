using Microsoft.AspNetCore.Http;

namespace FireplaceApi.Api.Interfaces
{
    public interface IControllerOutputHeaderParameters
    {
        public HeaderDictionary GetHeaderDictionary();
    }
}
