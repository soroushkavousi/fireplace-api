using System.Net;

namespace FireplaceApi.Application.Interfaces;

public interface IOutputCookieParameters
{
    public CookieCollection GetCookieCollection();
}
