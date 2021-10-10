using FireplaceApi.Core.Enums;
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
        public Task<User> GetUserByIdAsync(long id,
            bool includeEmail = false, bool includeGoogleUser = false,
            bool includeAccessTokens = false, bool includeSessions = false);
        public Task<User> GetUserByUsernameAsync(string username,
            bool includeEmail = false, bool includeGoogleUser = false,
            bool includeAccessTokens = false, bool includeSessions = false);
        public Task<string> GetUsernameByIdAsync(long id);
        public Task<long> GetIdByUsernameAsync(string username);
        public Task<User> CreateUserAsync(string firstName, string lastName,
            string username, UserState state, Password password = null);
        public Task<User> UpdateUserAsync(User user);
        public Task DeleteUserByIdAsync(long id);
        public Task DeleteUserByUsernameAsync(string username);
        public Task<bool> DoesUserIdExistAsync(long id);
        public Task<bool> DoesUsernameExistAsync(string username);
    }
}
