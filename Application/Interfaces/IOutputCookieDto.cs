using System.Net;

namespace FireplaceApi.Application.Interfaces;

public interface IOutputCookieDto
{
    public CookieCollection GetCookieCollection();
}
