using FireplaceApi.Api.Interfaces;
using FireplaceApi.Api.Tools;
using Microsoft.AspNetCore.Mvc;

namespace FireplaceApi.Api.Controllers
{
    public class ApiController : ControllerBase
    {
        protected void SetOutputHeaderParameters(IControllerOutputHeaderParameters outputHeaderParameters)
        {
            HttpContext.Items[Constants.ControllerOutputHeaderParametersKey] = outputHeaderParameters;
        }

        protected void SetOutputCookieParameters(IControllerOutputCookieParameters outputCookieParameters)
        {
            HttpContext.Items[Constants.ControllerOutputCookieParametersKey] = outputCookieParameters;
        }
    }
}
