﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GamingCommunityApi.Core.Models.UserInformations;
using GamingCommunityApi.Core.ValueObjects;
using GamingCommunityApi.Core.Enums;

namespace GamingCommunityApi.Core.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        public Task<List<User>> ListUsersAsync(
                    bool includeEmail = false, bool includeAccessTokens = false,
                    bool includeSessions = false);
        public Task<User> GetUserByIdAsync(long id, bool includeEmail = false,
            bool includeAccessTokens = false, bool includeSessions = false);
        public Task<User> GetUserByUsernameAsync(string username,
            bool includeEmail = false, bool includeAccessTokens = false,
            bool includeSessions = false);
        public Task<User> CreateUserAsync(string firstName, string lastName,
            string username, Password password, UserState state);
        public Task<User> UpdateUserAsync(User user);
        public Task DeleteUserAsync(long id);
        public Task<bool> DoesUserIdExistAsync(long id);
        public Task<bool> DoesUsernameExistAsync(string username);
    }
}