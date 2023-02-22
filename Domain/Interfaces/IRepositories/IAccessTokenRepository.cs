using FireplaceApi.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Interfaces
{
    public interface IAccessTokenRepository
    {
        public Task<List<AccessToken>> ListAccessTokensAsync(bool includeUser = false);
        public Task<AccessToken> GetAccessTokenByIdAsync(ulong id, bool includeUser = false);
        public Task<AccessToken> GetAccessTokenByValueAsync(string value, bool includeUser = false);
        public Task<AccessToken> CreateAccessTokenAsync(ulong id, ulong userId, string value);
        public Task<AccessToken> UpdateAccessTokenAsync(AccessToken accessToken);
        public Task DeleteAccessTokenAsync(ulong id);
        public Task<bool> DoesAccessTokenIdExistAsync(ulong id);
        public Task<bool> DoesAccessTokenValueExistAsync(string value);
    }
}
