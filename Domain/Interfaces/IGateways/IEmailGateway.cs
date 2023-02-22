using System.Threading.Tasks;

namespace FireplaceApi.Domain.Interfaces
{
    public interface IEmailGateway
    {
        public Task SendEmailMessageAsync(string toEmailAddress,
            string subject, string body);
    }
}
