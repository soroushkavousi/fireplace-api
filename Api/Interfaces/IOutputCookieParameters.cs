using System.Net;

namespace FireplaceApi.Api.Interfaces
{
    public interface IOutputCookieParameters
    {
        public CookieCollection GetCookieCollection();
    }
}
