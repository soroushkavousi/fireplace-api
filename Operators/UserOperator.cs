using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using GamingCommunityApi.Controllers.Parameters.UserParameters;
using GamingCommunityApi.Entities.UserInformationEntities;
using GamingCommunityApi.Enums;
using GamingCommunityApi.Exceptions;
using GamingCommunityApi.Extensions;
using GamingCommunityApi.Models;
using GamingCommunityApi.Models.UserInformations;
using GamingCommunityApi.Repositories;
using GamingCommunityApi.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Net;
using GamingCommunityApi.ValueObjects;

namespace GamingCommunityApi.Operators
{
    public class UserOperator
    {
        private readonly ILogger<UserOperator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly UserRepository _userRepository;
        
        public UserOperator(ILogger<UserOperator> logger, IConfiguration configuration,
            IServiceProvider serviceProvider, UserRepository userRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _userRepository = userRepository;
        }

        public async Task<User> SignUpWithEmailAsync(IPAddress ipAddress, string firstName,
            string lastName, string username, Password password, string emailAddress)
        {
            var user = await CreateUserAsync(firstName, lastName, username, password);
            //var user = new User(10, "Joseph", "Armstrong", "josepharmstrong", Password.OfValue("P@ssw0rd"), UserState.NOT_VERIFIED);

            var emailOperator = _serviceProvider.GetService<EmailOperator>();
            var emailActivation = await emailOperator.CreateEmailAsync(user.Id, emailAddress);

            var sessionOperator = _serviceProvider.GetService<SessionOperator>();
            var session = await sessionOperator.CreateSessionAsync(user.Id, ipAddress);

            var accessTokenOperator = _serviceProvider.GetService<AccessTokenOperator>();
            var accessToken = await accessTokenOperator.CreateAccessTokenAsync(user.Id);

            user = await GetUserByIdAsync(user.Id, true, true, false);
            return user;
        }

        public async Task<User> LogInWithEmailAsync(IPAddress ipAddress, string emailAddress, Password password)
        {
            var emailOperator = _serviceProvider.GetService<EmailOperator>();
            var email = await emailOperator.GetEmailByAddressAsync(emailAddress);

            var sessionOperator = _serviceProvider.GetService<SessionOperator>();
            var session = await sessionOperator.CreateOrUpdateSessionAsync(email.UserId, ipAddress);

            var accessTokenOperator = _serviceProvider.GetService<AccessTokenOperator>();
            var accessToken = await accessTokenOperator.CreateAccessTokenAsync(email.UserId);

            var user = await GetUserByIdAsync(email.UserId, true, true, false);
            return user;

        }

        public async Task<User> LogInWithUsernameAsync(IPAddress ipAddress, string username, Password password)
        {
            var user = await GetUserByUsernameAsync(username, false, false, false);

            var sessionOperator = _serviceProvider.GetService<SessionOperator>();
            var session = await sessionOperator.CreateOrUpdateSessionAsync(user.Id, ipAddress);

            var accessTokenOperator = _serviceProvider.GetService<AccessTokenOperator>();
            var accessToken = await accessTokenOperator.CreateAccessTokenAsync(user.Id);

            user = await GetUserByIdAsync(user.Id, true, true, false);
            return user;
        }

        public async Task<List<User>> ListUsersAsync(bool includeEmail = false, 
            bool includeAccessTokens = false, bool includeSessions = false)
        {
            var users = await _userRepository.ListUsersAsync(includeEmail,
                includeAccessTokens, includeSessions);

            foreach (var user in users)
            {
                if (user.Sessions.IsNullOrEmpty())
                    continue;

                for (int i = 0; i < user.Sessions.Count; i++)
                {
                    if (user.Sessions[i].State == SessionState.CLOSED)
                        user.Sessions.RemoveAt(i);
                }
            }
            return users;
        }

        public async Task<User> GetUserByIdAsync(long id, bool includeEmail = false,
            bool includeAccessTokens = false, bool includeSessions = false)
        {
            var user = await _userRepository.GetUserByIdAsync(id, includeEmail, 
                includeAccessTokens, includeSessions);

            if (user == null)
                return user;

            if (user.Sessions.IsNullOrEmpty())
                return user;
            
            for (int i = 0; i < user.Sessions.Count; i++)
            {
                if (user.Sessions[i].State == SessionState.CLOSED)
                    user.Sessions.RemoveAt(i);
            }
            return user;
        }

        public async Task<User> GetUserByUsernameAsync(string username, bool includeEmail = false, 
            bool includeAccessTokens = false, bool includeSessions = false)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username, includeEmail,
                 includeAccessTokens, includeSessions);

            if (user == null)
                return user;

            if (user.Sessions.IsNullOrEmpty())
                return user;

            for (int i = 0; i < user.Sessions.Count; i++)
            {
                if (user.Sessions[i].State == SessionState.CLOSED)
                    user.Sessions.RemoveAt(i);
            }
            return user;
        }

        public async Task<User> CreateUserAsync(string firstName, string lastName,
            string username, Password password)
        {
            var user = await _userRepository.CreateUserAsync(firstName, lastName, username,
                 password, UserState.NOT_VERIFIED);
            return user;
        }

        public async Task<User> PatchUserByIdAsync(long id, string firstName = null, 
            string lastName = null, string username = null, Password password = null, 
            UserState? state = null, string emailAddress = null)
        {
            var user = await _userRepository.GetUserByIdAsync(id, true);
            user = await ApplyUserChanges(user, firstName, lastName, username, password, 
                state, emailAddress);
            user = await GetUserByIdAsync(user.Id, true, false, false);
            return user;
        }

        public async Task DeleteUserAsync(long id)
        {
            await _userRepository.DeleteUserAsync(id);
        }

        public async Task<bool> DoesUserIdExistAsync(long id)
        {
            var userIdExists = await _userRepository.DoesUserIdExistAsync(id);
            return userIdExists;
        }

        public async Task<User> ApplyUserChanges(User user, string firstName = null,
            string lastName = null, string username = null, Password password = null,
            UserState? state = null, string emailAddress = null)
        {
            if (firstName != null)
            {
                user.FirstName = firstName;
            }

            if (lastName != null)
            {
                user.LastName = lastName;
            }

            if (username != null)
            {
                user.Username = username;
            }

            if (password != null)
            {
                user.Password = password;
            }

            if (state != null)
            {
                user.State = state.Value;
            }

            user = await _userRepository.UpdateUserAsync(user);

            if (emailAddress != null)
            {
                var emailOperator = _serviceProvider.GetService<EmailOperator>();
                user.Email = await emailOperator.PatchEmailByIdAsync(user.Email.Id, address: emailAddress);
            }

            return user;
        }

        public async Task<bool> DoesUsernameExistAsync(string username)
        {
            return await _userRepository.DoesUsernameExistAsync(username);
        }
    }
}
