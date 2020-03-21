using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

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

        public async Task SendEmailMessage(string smtpServerAddress, int smtpServerPort,
            string fromEmailAddress, string fromEmailPassword, string toEmailAddress, 
            string subject, string body)
        {
            await Task.Run(() =>
            {
                var from = new MailAddress(fromEmailAddress, "Gaming Community");
                var to = new MailAddress(toEmailAddress, "User");

                var smtp = new SmtpClient
                {
                    Host = smtpServerAddress,
                    Port = smtpServerPort,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(from.Address, fromEmailPassword),
                    Timeout = 20000
                };
                using var message = new MailMessage(from, to)
                {
                    Subject = subject,
                    Body = body
                };
                smtp.Send(message);
            });
            
            _logger.LogInformation($"Sending mail from {fromEmailAddress} to {fromEmailPassword} successfully completed. body: {body.Substring(0, 20)}...");
        }
    }
}
