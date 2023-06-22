using FireplaceApi.Domain.Configurations;
using FireplaceApi.Presentation.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FireplaceApi.Presentation.Controllers;

public class ApiController : ControllerBase
{
    protected void SetOutputHeaderDto(IOutputHeaderDto outputHeaderDto)
    {
        var headerDictionary = outputHeaderDto.GetHeaderDictionary();
        if (headerDictionary == null || headerDictionary.Count == 0)
            return;

        foreach (var header in headerDictionary)
        {
            HttpContext.Response.Headers.Add(header);
        }
    }

    protected void SetOutputCookieDto(IOutputCookieDto outputCookieDto)
    {
        var cookieCollection = outputCookieDto.GetCookieCollection();
        if (cookieCollection == null || cookieCollection.Count == 0)
            return;

        var cookieOptions = new CookieOptions
        {
            MaxAge = new System.TimeSpan(
                Configs.Current.Api.CookieMaxAgeInDays, 0, 0, 0)
        };
        foreach (Cookie cookie in cookieCollection)
        {
            HttpContext.Response.Cookies.Append(cookie.Name, cookie.Value,
                cookieOptions);
        }
    }
}
