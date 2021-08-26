using Microsoft.AspNetCore.Http;
using FireplaceApi.Api.Extensions;
using FireplaceApi.Api.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FireplaceApi.Api.Interfaces
{
    public interface IControllerOutputCookieParameters
    {
        public CookieCollection GetCookieCollection();
    }
}
