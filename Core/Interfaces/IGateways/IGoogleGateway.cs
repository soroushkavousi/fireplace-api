using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace GamingCommunityApi.Core.Interfaces.IGateways
{
    public interface IGoogleGateway
    {
        public Task RequestToken(string clientId, string clientSecret, string userCode);
    }
}
