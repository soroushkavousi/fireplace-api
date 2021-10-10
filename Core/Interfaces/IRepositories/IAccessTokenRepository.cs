using FireplaceApi.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Interfaces
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
        public Task<bool> DoesAccessTokenValueExistAsync(string value);
    }
}
