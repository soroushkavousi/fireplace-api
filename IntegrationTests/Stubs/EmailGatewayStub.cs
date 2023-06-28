using FireplaceApi.Application.Emails;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FireplaceApi.IntegrationTests.Stubs;

public class EmailGatewayStub : IEmailGateway
{
    private readonly ILogger<EmailGatewayStub> _logger;

    public EmailGatewayStub(ILogger<EmailGatewayStub> logger)
    {
        _logger = logger;
    }

    public Task SendEmailMessageAsync(string toEmailAddress, string subject, string body)
    {
        var sw = Stopwatch.StartNew();
        string serverLog = $"Email has been sent to {toEmailAddress}! body: {body[..10]}...";
        _logger.LogServerInformation(serverLog, sw);
        return Task.CompletedTask;
    }
}
