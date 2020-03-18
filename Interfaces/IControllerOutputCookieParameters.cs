using Microsoft.AspNetCore.Http;
using GamingCommunityApi.Extensions;
using GamingCommunityApi.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GamingCommunityApi.Interfaces
{
    public interface IControllerOutputCookieParameters
    {
        public CookieCollection GetCookieCollection();
    }
}
