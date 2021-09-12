using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace FireplaceApi.Core.Interfaces
{
    public interface IEmailGateway
    {
        public Task SendEmailMessage(string toEmailAddress,
            string subject, string body);

        public Task SendEmailMessage(string smtpServerAddress, int smtpServerPort,
            string fromEmailAddress, string fromEmailPassword, string toEmailAddress,
            string subject, string body);
    }
}
