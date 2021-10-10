using System.Net;

namespace FireplaceApi.Api.Interfaces
{
    public interface IControllerOutputCookieParameters
    {
        public CookieCollection GetCookieCollection();
    }
}
