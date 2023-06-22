using System.Threading.Tasks;

namespace FireplaceApi.Application.Emails;

public interface IEmailGateway
{
    public Task SendEmailMessageAsync(string toEmailAddress,
        string subject, string body);
}
