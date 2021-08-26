using Microsoft.AspNetCore.Mvc;
using FireplaceApi.Api.Interfaces;
using FireplaceApi.Api.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

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
