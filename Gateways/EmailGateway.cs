using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingCommunityApi.Gateways
{
    public class EmailGateway
    {
        private readonly ILogger<EmailGateway> _logger;
        private readonly IConfiguration _configuration;

        public EmailGateway(ILogger<EmailGateway> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public Task Send(string emailAddress, string message)
        {
            _logger.LogInformation($"Sending mail to {emailAddress} successfully completed. message: {message}");
            return Task.CompletedTask;
        }
    }
}
