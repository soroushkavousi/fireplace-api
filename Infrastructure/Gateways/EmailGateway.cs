using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Operators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

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
                var sw = Stopwatch.StartNew();
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
                        Timeout = 30000,
                    };
                    smtp.Credentials = new NetworkCredential(from.Address, fromEmailPassword);
                    using var message = new MailMessage(from, to)
                    {
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };
                    smtp.Send(message);
                    _logger.LogAppInformation($"Email has been sent from {fromEmailAddress} to {toEmailAddress}! body: {body[..10]}...", sw);
                }
                catch (Exception ex)
                {
                    string message = $"Can't send email from {fromEmailAddress} to {toEmailAddress}! body: {body[..10]}...";
                    _logger.LogAppError(message, ex, sw);
                }
            });
        }
    }
}
