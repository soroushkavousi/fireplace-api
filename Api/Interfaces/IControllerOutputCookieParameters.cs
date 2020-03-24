using Microsoft.AspNetCore.Http;
using GamingCommunityApi.Api.Extensions;
using GamingCommunityApi.Api.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GamingCommunityApi.Api.Interfaces
{
    public interface IControllerOutputCookieParameters
    {
        public CookieCollection GetCookieCollection();
    }
}
