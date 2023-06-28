﻿using FireplaceApi.Domain.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Users;

public interface IUserRepository
{
    public Task<List<User>> ListUsersAsync(
        bool includeEmail = false, bool includeGoogleUser = false,
        bool includeSessions = false);
    public Task<User> GetUserByIdentifierAsync(UserIdentifier identifier,
        bool includeEmail = false, bool includeGoogleUser = false,
        bool includeSessions = false);
    public Task<Username> GetUsernameByIdAsync(ulong id);
    public Task<ulong> GetIdByUsernameAsync(Username username);
    public Task<User> CreateUserAsync(Username username, UserState state,
        List<UserRole> roles, Password password = null, string displayName = null,
        string about = null, string avatarUrl = null, string bannerUrl = null);
    public Task<User> UpdateUserAsync(User user);
    public Task UpdateUsernameAsync(ulong id, Username newUsername);
    public Task DeleteUserByIdentifierAsync(UserIdentifier identifier);
    public Task<bool> DoesUserIdentifierExistAsync(UserIdentifier identifier);
}
