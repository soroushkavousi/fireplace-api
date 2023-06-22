using System.Net;

namespace FireplaceApi.Presentation.Interfaces;

public interface IOutputCookieDto
{
    public CookieCollection GetCookieCollection();
}
