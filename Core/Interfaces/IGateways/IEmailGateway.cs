using System.Threading.Tasks;

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
