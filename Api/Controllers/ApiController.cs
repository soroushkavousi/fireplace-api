using Microsoft.AspNetCore.Mvc;
using GamingCommunityApi.Api.Interfaces;
using GamingCommunityApi.Api.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GamingCommunityApi.Api.Controllers
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
