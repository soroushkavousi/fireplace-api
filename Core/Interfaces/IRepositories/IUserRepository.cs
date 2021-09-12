using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.ValueObjects;
using FireplaceApi.Core.Enums;

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
        public Task<User> CreateUserAsync(string firstName, string lastName,
            string username, UserState state, Password password = null);
        public Task<User> UpdateUserAsync(User user);
        public Task DeleteUserAsync(long id);
        public Task<bool> DoesUserIdExistAsync(long id);
        public Task<bool> DoesUsernameExistAsync(string username);
    }
}
