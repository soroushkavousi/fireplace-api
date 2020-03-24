using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GamingCommunityApi.Core.Models.UserInformations;

namespace GamingCommunityApi.Core.Interfaces.IRepositories
{
    public interface IAccessTokenRepository
    {
        public Task<List<AccessToken>> ListAccessTokensAsync(bool includeUser = false);
        public Task<AccessToken> GetAccessTokenByIdAsync(long id, bool includeUser = false);
        public Task<AccessToken> GetAccessTokenByValueAsync(string value, bool includeUser = false);
        public Task<AccessToken> CreateAccessTokenAsync(long userId, string value);
        public Task<AccessToken> UpdateAccessTokenAsync(AccessToken accessToken);
        public Task DeleteAccessTokenAsync(long id);
        public Task<bool> DoesAccessTokenIdExistAsync(long id);
    }
}
