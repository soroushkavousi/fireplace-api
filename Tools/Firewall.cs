using GamingCommunityApi.Enums;
using GamingCommunityApi.Exceptions;
using GamingCommunityApi.Extensions;
using GamingCommunityApi.Models.UserInformations;
using GamingCommunityApi.Operators;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GamingCommunityApi.Tools
{
    public class Firewall
    {
        private readonly ILogger<Firewall> _logger;
        private readonly AccessTokenOperator _accessTokenOperator;
        private readonly SessionOperator _sessionOperator;

        public Firewall(ILogger<Firewall> logger, 
            AccessTokenOperator accessTokenOperator, SessionOperator sessionOperator)
        {
            _logger = logger;
            _accessTokenOperator = accessTokenOperator;
            _sessionOperator = sessionOperator;
        }

        public async Task CheckRequestBody(HttpRequest request)
        {
            if (request.ContentType == "application/json")
            {
                var requestBodyText = await request.ReadRequestBodyAsync();
                if(requestBodyText.IsJson() == false)
                {
                    var serverMessage = $"Input request body is not json! requestBodyText: {requestBodyText}";
                    throw new ApiException(ErrorName.REQUEST_BODY_IS_NOT_JSON, serverMessage);
                }
            }
        }

        public async Task<User> CheckUser(string accessTokenValue, IPAddress ipAddress)
        {
            var accessToken = await ValidateAccessTokenAsync(accessTokenValue);
            var requesterUser = accessToken.User;
            requesterUser.AccessTokens = new List<AccessToken> { accessToken };
            await ValidateSessionAsync(requesterUser.Id, ipAddress);
            await ValidateLimitationOfUserRequestCounts(requesterUser.Id);
            await ValidateLimitationOfIpRequestCounts(ipAddress);
            _logger.LogInformation($"User {requesterUser.Id} doesn't have any problem to continue. {requesterUser.ToJson()}");
            return requesterUser;
        }

        public async Task CheckGuest(IPAddress ipAddress)
        {
            await ValidateLimitationOfIpRequestCounts(ipAddress);
            _logger.LogInformation($"Guest doesn't have any problem to continue. {ipAddress}");
        }

        public async Task<AccessToken> ValidateAccessTokenAsync(string accessTokenValue)
        {
            if (string.IsNullOrWhiteSpace(accessTokenValue))
                throw new ApiException(Enums.ErrorName.AUTHENTICATION_FAILED,
                    $"There isn't any authorization in input header parameters! accessTokenValue: {accessTokenValue}");

            if (Regexes.AccessToken.IsMatch(accessTokenValue) == false)
                throw new ApiException(Enums.ErrorName.AUTHENTICATION_FAILED,
                    $"Input access token doesn't have valid format! accessTokenValue: {accessTokenValue}");

            var accessToken = await _accessTokenOperator
                .GetAccessTokenByValueAsync(accessTokenValue, true);
            if (accessToken == null)
                throw new ApiException(Enums.ErrorName.AUTHENTICATION_FAILED,
                    $"Input access token does not exist! accessTokenValue: {accessTokenValue}");

            return accessToken;
        }

        public async Task<Session> ValidateSessionAsync(long userId, IPAddress ipAddress)
        {
            var session = await _sessionOperator.FindSessionAsync(userId, ipAddress);
            if (session != null && session.State == Enums.SessionState.CLOSED)
                throw new ApiException(Enums.ErrorName.AUTHENTICATION_FAILED,
                    $"User session was closed! userId: {userId}, ipAddress: {ipAddress}");

            session = await _sessionOperator.CreateOrUpdateSessionAsync(userId, ipAddress);
            return session;
        }

        public async Task ValidateLimitationOfUserRequestCounts(long userId)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateLimitationOfIpRequestCounts(IPAddress ipAddress)
        {
            await Task.CompletedTask;
        }
    }
}
