using FireplaceApi.Api.Interfaces;
using FireplaceApi.Core.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FireplaceApi.Api.Controllers
{
    public class ApiController : ControllerBase
    {
        protected void SetOutputHeaderParameters(IOutputHeaderParameters outputHeaderParameters)
        {
            var headerDictionary = outputHeaderParameters.GetHeaderDictionary();
            if (headerDictionary == null || headerDictionary.Count == 0)
                return;

            foreach (var header in headerDictionary)
            {
                HttpContext.Response.Headers.Add(header);
            }
        }

        protected void SetOutputCookieParameters(IOutputCookieParameters outputCookieParameters)
        {
            var cookieCollection = outputCookieParameters.GetCookieCollection();
            if (cookieCollection == null || cookieCollection.Count == 0)
                return;

            var cookieOptions = new CookieOptions
            {
                MaxAge = new System.TimeSpan(
                    Configs.Instance.Api.CookieMaxAgeInDays, 0, 0, 0)
            };
            foreach (Cookie cookie in cookieCollection)
            {
                HttpContext.Response.Cookies.Append(cookie.Name, cookie.Value,
                    cookieOptions);
            }
        }
    }
}
