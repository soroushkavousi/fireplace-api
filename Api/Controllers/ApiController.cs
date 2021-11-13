using FireplaceApi.Api.Interfaces;
using FireplaceApi.Api.Tools;
using Microsoft.AspNetCore.Mvc;

namespace FireplaceApi.Api.Controllers
{
    public class ApiController : ControllerBase
    {
        protected void SetOutputHeaderParameters(IOutputHeaderParameters outputHeaderParameters)
        {
            HttpContext.Items[Constants.OutputHeaderParametersKey] = outputHeaderParameters;
        }

        protected void SetOutputCookieParameters(IOutputCookieParameters outputCookieParameters)
        {
            HttpContext.Items[Constants.OutputCookieParametersKey] = outputCookieParameters;
        }
    }
}
