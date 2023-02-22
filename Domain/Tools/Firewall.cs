using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Exceptions;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Operators;
using FireplaceApi.Domain.Validators;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Tools
{
    public class Firewall
    {
        private readonly ILogger<Firewall> _logger;
        private readonly AccessTokenOperator _accessTokenOperator;
        private readonly SessionOperator _sessionOperator;
        private readonly AccessTokenValidator _accessTokenValidator;
        private readonly RequestTraceOperator _requestTraceOperator;

        public Firewall(ILogger<Firewall> logger, AccessTokenOperator accessTokenOperator,
            SessionOperator sessionOperator, AccessTokenValidator accessTokenValidator,
            RequestTraceOperator requestTraceOperator)
        {
            _logger = logger;
            _accessTokenOperator = accessTokenOperator;
            _sessionOperator = sessionOperator;
            _accessTokenValidator = accessTokenValidator;
            _requestTraceOperator = requestTraceOperator;
        }

        public async Task<User> CheckUser(User requestingUser, IPAddress ipAddress)
        {
            var sw = Stopwatch.StartNew();
            await ValidateSessionAsync(requestingUser.Id, ipAddress);
            await ValidateLimitationOfIPRequestCounts(ipAddress);
            _logger.LogAppTrace($"User {requestingUser.Id} doesn't have any problem to continue. ",
                sw: sw, parameters: new { requestingUser });
            return requestingUser;
        }

        public async Task CheckGuest(IPAddress ipAddress)
        {
            var sw = Stopwatch.StartNew();
            await ValidateLimitationOfIPRequestCounts(ipAddress);
            _logger.LogAppTrace($"Guest doesn't have any problem to continue. {ipAddress}", sw);
        }

        public void ValidateRequestingUserExists(User requestingUser, string accessTokenValue)
        {
            if (requestingUser == null)
                throw new ApiException(ErrorName.AUTHENTICATION_FAILED,
                    $"There isn't any authorization in input header parameters! " +
                    $"accessTokenValue: {accessTokenValue}");
        }

        public async Task<AccessToken> ValidateAccessTokenAsync(string accessTokenValue, bool isUserEndpoint)
        {
            _accessTokenValidator.ValidateAccessTokenValueFormat(accessTokenValue);
            //if (Regexes.AccessTokenValue.IsMatch(accessTokenValue) == false)
            //    throw new ApiException(ErrorName.AUTHENTICATION_FAILED,
            //        $"Input access token doesn't have valid format! accessTokenValue: {accessTokenValue}");

            var accessToken = await _accessTokenOperator
                .GetAccessTokenByValueAsync(accessTokenValue, true);
            if (isUserEndpoint && accessToken == null)
                throw new ApiException(ErrorName.AUTHENTICATION_FAILED,
                    $"Input access token does not exist! accessTokenValue: {accessTokenValue}");

            return accessToken;
        }

        public async Task<Session> ValidateSessionAsync(ulong userId, IPAddress ipAddress)
        {
            var session = await _sessionOperator.FindSessionAsync(userId, ipAddress);
            if (session != null && session.State == SessionState.CLOSED)
                throw new ApiException(ErrorName.AUTHENTICATION_FAILED,
                    $"User session was closed! userId: {userId}, ipAddress: {ipAddress}");

            session = await _sessionOperator.CreateOrUpdateSessionAsync(userId, ipAddress);
            return session;
        }

        public async Task ValidateLimitationOfIPRequestCounts(IPAddress ip)
        {
            if (ip.IsLocalIpAddress())
                return;

            var fromDate = DateTime.UtcNow.AddMinutes(-Configs.Current.Api.RequestLimitionPeriodInMinutes);
            var requestCountPerIP = await _requestTraceOperator.CountRequestTracesAsync(ip: ip, fromDate: fromDate,
                withAction: true);
            if (requestCountPerIP > Configs.Current.Api.MaxRequestPerIP)
            {
                var message = $"Max request limit reached for ip {ip}! request count: {requestCountPerIP}";
                _logger.LogAppWarning(title: "REQUEST_LIMIT", parameters: new { fromDate, DateTime.UtcNow, ip, requestCountPerIP, Configs.Current.Api.MaxRequestPerIP });
                throw new ApiException(ErrorName.MAX_REQUEST_LIMIT, message);
            }
        }
    }
}
