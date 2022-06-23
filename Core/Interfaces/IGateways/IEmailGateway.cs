using System.Threading.Tasks;

namespace FireplaceApi.Core.Interfaces
{
    public interface IEmailGateway
    {
        public Task SendEmailMessageAsync(string toEmailAddress,
            string subject, string body);
    }
}
