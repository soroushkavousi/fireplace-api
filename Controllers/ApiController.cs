using Microsoft.AspNetCore.Mvc;
using GamingCommunityApi.Controllers.Parameters;
using GamingCommunityApi.Converters;
using GamingCommunityApi.Exceptions;
using GamingCommunityApi.Extensions;
using GamingCommunityApi.Interfaces;
using GamingCommunityApi.Models.UserInformations;
using GamingCommunityApi.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GamingCommunityApi.Controllers
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
