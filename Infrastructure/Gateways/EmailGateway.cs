using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Operators;

namespace FireplaceApi.Infrastructure.Gateways
{
    public class EmailGateway : IEmailGateway
    {
        private readonly ILogger<EmailGateway> _logger;
        private readonly IConfiguration _configuration;

        public EmailGateway(ILogger<EmailGateway> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task SendEmailMessage(string toEmailAddress,
            string subject, string body)
        {
            var emailGlobalValues = GlobalOperator.GlobalValues.Email;
            await SendEmailMessage(emailGlobalValues.SmtpServerAddress,
                emailGlobalValues.SmtpServerPort, emailGlobalValues.Address,
                emailGlobalValues.Password, toEmailAddress,
                subject, body);
        }

        public async Task SendEmailMessage(string smtpServerAddress, int smtpServerPort,
            string fromEmailAddress, string fromEmailPassword, string toEmailAddress, 
            string subject, string body)
        {
            await Task.Run(() =>
            {
                try
                {
                    var from = new MailAddress(fromEmailAddress, "Fireplace");
                    var to = new MailAddress(toEmailAddress, "User");

                    var smtp = new SmtpClient
                    {
                        Host = smtpServerAddress,
                        Port = smtpServerPort,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(from.Address, fromEmailPassword),
                        Timeout = 30000,
                    };
                    using var message = new MailMessage(from, to)
                    {
                        Subject = subject,
                        Body = body
                    };
                    smtp.Send(message);
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, $"Can't send email from {fromEmailAddress} to {fromEmailPassword}! body: {body.Substring(0, 20)}...");
                }
            });
            
            _logger.LogInformation($"Sending mail from {fromEmailAddress} to {fromEmailPassword} successfully completed. body: {body.Substring(0, 20)}...");
        }
    }
}
