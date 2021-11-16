using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Identifiers;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Interfaces
{
    public interface IUserRepository
    {
        public Task<List<User>> ListUsersAsync(
            bool includeEmail = false, bool includeGoogleUser = false,
            bool includeAccessTokens = false, bool includeSessions = false);
        public Task<User> GetUserByIdentifierAsync(UserIdentifier identifier,
            bool includeEmail = false, bool includeGoogleUser = false,
            bool includeAccessTokens = false, bool includeSessions = false);
        public Task<string> GetUsernameByIdAsync(ulong id);
        public Task<ulong> GetIdByUsernameAsync(string username);
        public Task<User> CreateUserAsync(ulong id, string firstName,
            string lastName, string username, UserState state,
            Password password = null);
        public Task<User> UpdateUserAsync(User user);
        public Task UpdateUsernameAsync(ulong id, string newUsername);
        public Task DeleteUserByIdentifierAsync(UserIdentifier identifier);
        public Task<bool> DoesUserIdentifierExistAsync(UserIdentifier identifier);
    }
}
