using FireplaceApi.Domain.Interfaces;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Tools;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Operators
{
    public class AccessTokenOperator
    {
        private readonly ILogger<AccessTokenOperator> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IAccessTokenRepository _accessTokenRepository;

        public AccessTokenOperator(ILogger<AccessTokenOperator> logger,
            IServiceProvider serviceProvider, IAccessTokenRepository accessTokenRepository)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _accessTokenRepository = accessTokenRepository;
        }

        public async Task<List<AccessToken>> ListAccessTokensAsync(bool includeUser = false)
        {
            var accessToken = await _accessTokenRepository.ListAccessTokensAsync(includeUser);
            return accessToken;
        }

        public async Task<AccessToken> GetAccessTokenByIdAsync(ulong id, bool includeUser = false)
        {
            var accessToken = await _accessTokenRepository.GetAccessTokenByIdAsync(id, includeUser);
            if (accessToken == null)
                return accessToken;

            return accessToken;
        }

        public async Task<AccessToken> GetAccessTokenByValueAsync(string value, bool includeUser = false)
        {
            var accessToken = await _accessTokenRepository.GetAccessTokenByValueAsync(value, includeUser);
            if (accessToken == null)
                return accessToken;

            return accessToken;
        }

        public async Task<AccessToken> CreateAccessTokenAsync(ulong userId)
        {
            var accessTokenValue = GenerateNewAccessTokenValue();
            var id = await IdGenerator.GenerateNewIdAsync(DoesAccessTokenIdExistAsync);
            var accessToken = await _accessTokenRepository.CreateAccessTokenAsync(
                id, userId, accessTokenValue);
            return accessToken;
        }

        public async Task<AccessToken> PatchAccessTokenByIdAsync(ulong id, ulong? userId = null,
            string value = null)
        {
            var accessToken = await _accessTokenRepository.GetAccessTokenByIdAsync(id, true);
            accessToken = await ApplyAccessTokenChangesAsync(accessToken, userId, value);
            accessToken = await GetAccessTokenByIdAsync(id, true);
            return accessToken;
        }

        public async Task<AccessToken> PatchAccessTokenByValueAsync(string existingValue, ulong? userId = null,
            string value = null)
        {
            var accessToken = await _accessTokenRepository.GetAccessTokenByValueAsync(existingValue, true);
            accessToken = await ApplyAccessTokenChangesAsync(accessToken, userId, value);
            accessToken = await GetAccessTokenByIdAsync(accessToken.Id, true);
            return accessToken;
        }

        public async Task DeleteAccessTokenAsync(ulong id)
        {
            await _accessTokenRepository.DeleteAccessTokenAsync(id);
        }

        public async Task<bool> DoesAccessTokenIdExistAsync(ulong id)
        {
            var accessTokenIdExists = await _accessTokenRepository.DoesAccessTokenIdExistAsync(id);
            return accessTokenIdExists;
        }

        public async Task<bool> DoesAccessTokenValueExistAsync(string value)
        {
            var accessTokenValueExists = await _accessTokenRepository.DoesAccessTokenValueExistAsync(value);
            return accessTokenValueExists;
        }

        public async Task<AccessToken> ApplyAccessTokenChangesAsync(AccessToken accessToken, ulong? userId = null,
            string value = null)
        {
            if (userId != null)
            {
                accessToken.UserId = userId.Value;
            }

            if (value != null)
            {
                accessToken.Value = value;
            }

            accessToken = await _accessTokenRepository.UpdateAccessTokenAsync(accessToken);
            return accessToken;
        }

        public string GenerateNewAccessTokenValue()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
