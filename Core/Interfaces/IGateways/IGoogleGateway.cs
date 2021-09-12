using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using FireplaceApi.Core.ValueObjects;

namespace FireplaceApi.Core.Interfaces
{
    public interface IGoogleGateway
    {
        public Task<GoogleUserToken> GetgoogleUserToken(string userCode);
        public Task<GoogleUserToken> GetgoogleUserToken(string clientId,
            string clientSecret, string redirectUrl, string userCode);
        public string GetTokenUrl(string code);
        public string GetTokenUrl(string baseTokenUrl,
            string clientId, string clientSecret, string code,
            string grantType, string redirectUrl);
        public string GetAuthUrl();
        public string GetAuthUrl(string baseAuthUrl,
            string clientId, string redirectUrl, string responseType,
            string scope, string accessType, string state,
            string includeGrantedScope, string display);
    }
}
