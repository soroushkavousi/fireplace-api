using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Tools;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using static Google.Apis.Gmail.v1.GmailService;

namespace FireplaceApi.Infrastructure.Gateways
{
    public class GmailGateway : IEmailGateway
    {
        private readonly ILogger<GmailGateway> _logger;
        private GmailService _gmailService;
        private const string _projectName = "fireplace-api";
        private const string _tokenResponseFileName = $"Google.Apis.Auth.OAuth2.Responses.TokenResponse-{_projectName}";

        public GmailGateway(ILogger<GmailGateway> logger)
        {
            _logger = logger;
            MakeTokenResponseFileNameIfNotExists();
            InitializeGmailService();
        }

        private static void MakeTokenResponseFileNameIfNotExists()
        {
            var theFilePath = Path.Combine(Constants.SecretsDirectoryPath, _tokenResponseFileName);
            if (System.IO.File.Exists(theFilePath))
                return;

            var tokenResponse = Configs.Current.Email.GmailTokenResponse;
            Directory.CreateDirectory(Constants.SecretsDirectoryPath);
            System.IO.File.WriteAllText(theFilePath, tokenResponse.ToJson());
        }

        private void InitializeGmailService()
        {
            var clientSecrets = new ClientSecrets
            {
                ClientId = Configs.Current.Google.ClientId,
                ClientSecret = Configs.Current.Google.ClientSecret,
            };

            var scopes = new string[] { ScopeConstants.GmailSend };
            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        clientSecrets,
                        scopes,
                        _projectName,
                        CancellationToken.None,
                        new FileDataStore(Constants.SecretsDirectoryPath, true)).Result;

            _gmailService = new GmailService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = _projectName,
            });
        }

        public async Task SendEmailMessageAsync(string toEmailAddress,
            string subject, string body)
        {
            var sw = Stopwatch.StartNew();
            string message = $"To: {toEmailAddress}\r\nSubject: {subject}\r\nContent-Type: text/html;charset=utf-8\r\n\r\n{body}";
            var newMsg = new Message
            {
                Raw = message.ToBase64UrlEncode(),
            };

            Message response = await _gmailService.Users.Messages.Send(newMsg, "me").ExecuteAsync();

            if (response != null)
            {
                string serverLog = $"Email has been sent to {toEmailAddress}! body: {body[..10]}...";
                _logger.LogAppInformation(serverLog, sw);
            }
            else
            {
                string serverLog = $"Can't send email from to {toEmailAddress}! body: {body[..10]}...";
                _logger.LogAppError(serverLog, sw);
            }
        }
    }
}
