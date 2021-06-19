using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using GamingCommunityApi.Core.Enums;
using GamingCommunityApi.Core.Exceptions;
using GamingCommunityApi.Core.Extensions;
using GamingCommunityApi.Core.Models;
using GamingCommunityApi.Core.Models.UserInformations;
using GamingCommunityApi.Core.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Net;
using GamingCommunityApi.Core.ValueObjects;
using GamingCommunityApi.Core.Interfaces.IRepositories;
using GamingCommunityApi.Core.Interfaces.IGateways;
using Microsoft.AspNetCore.WebUtilities;
using GamingCommunityApi.Core.Tools;

namespace GamingCommunityApi.Core.Operators
{
    public class UserOperator
    {
        private readonly ILogger<UserOperator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserRepository _userRepository;
        private readonly IGoogleGateway _googleGateway;

        public UserOperator(ILogger<UserOperator> logger, IConfiguration configuration,
            IServiceProvider serviceProvider, IUserRepository userRepository,
            IGoogleGateway googleGateway)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _userRepository = userRepository;
            _googleGateway = googleGateway;
        }

        private async Task<User> SignUpWithGoogleAsync(IPAddress ipAddress, 
            GoogleUserInformations googleUserInformations, string state,
            string scope, string authUser, string prompt)
        {
            var emailOperator = _serviceProvider.GetService<EmailOperator>();
            var googleUserOperator = _serviceProvider.GetService<GoogleUserOperator>();
            Email email;
            User user;
            if (await emailOperator.DoesEmailAddressExistAsync(googleUserInformations.GmailAddress))
            {
                email = await emailOperator
                    .GetEmailByAddressAsync(googleUserInformations.GmailAddress);
            }
            else
            {
                var username = await GenerateNewUsername();
                user = await CreateUserAsync(googleUserInformations.FirstName,
                    googleUserInformations.LastName, username);

                email = await emailOperator.CreateEmailAsync(user.Id, googleUserInformations.GmailAddress);
            }

            var redirectToUserUrl = await googleUserOperator.GetRedirectToUserUrlFromState();
            var googleUser = await googleUserOperator.CreateGoogleUserAsync(email.UserId,
                googleUserInformations.Code, googleUserInformations.AccessToken,
                googleUserInformations.TokenType, googleUserInformations.AccessTokenExpiresInSeconds,
                googleUserInformations.RefreshToken, googleUserInformations.Scope,
                googleUserInformations.IdToken, googleUserInformations.AccessTokenIssuedTime,
                googleUserInformations.GmailAddress, googleUserInformations.GmailVerified,
                googleUserInformations.GmailIssuedTimeInSeconds, googleUserInformations.FullName,
                googleUserInformations.FirstName, googleUserInformations.LastName,
                googleUserInformations.Locale, googleUserInformations.PictureUrl, state,
                authUser, prompt, redirectToUserUrl);

            user = await GetUserByIdAsync(email.UserId, true, true, true, false);
            return user;
        }

        public async Task<User> LogInWithGoogleAsync(IPAddress ipAddress, string state,
            string code, string scope, string authUser, string prompt)
        {
            var googleUserOperator = _serviceProvider.GetService<GoogleUserOperator>(); //todo
            User user;
            long userId;
            var googleUserInformations = await _googleGateway.GetGoogleUserInformations(code);

            if (await googleUserOperator
                .DoesGoogleUserGmailAddressExistAsync(googleUserInformations.GmailAddress))
            {
                var googleUser = await googleUserOperator
                    .GetGoogleUserByGmailAddressAsync(googleUserInformations.GmailAddress);
                userId = googleUser.UserId;
            }
            else
            {
                user = await SignUpWithGoogleAsync(ipAddress, googleUserInformations,
                    state, scope, authUser, prompt);
                userId = user.Id;
            }
            
            var sessionOperator = _serviceProvider.GetService<SessionOperator>();
            var session = await sessionOperator.CreateOrUpdateSessionAsync(userId, ipAddress);

            var accessTokenOperator = _serviceProvider.GetService<AccessTokenOperator>();
            var accessToken = await accessTokenOperator.CreateAccessTokenAsync(userId);

            user = await GetUserByIdAsync(userId, true, true, true, false);
            return user;
        }

        public async Task<string> GetGoogleAuthUrlAsync(IPAddress ipAddress)
        {
            await Task.CompletedTask;
            var googleAuthUrl = _googleGateway.GetAuthUrl();
            _logger.LogInformation($"googleLogInPageUrl: {googleAuthUrl}");
            return googleAuthUrl;
        }

        public async Task<User> SignUpWithEmailAsync(IPAddress ipAddress, string firstName,
            string lastName, string username, Password password, string emailAddress)
        {
            var user = await CreateUserAsync(firstName, lastName, username, password);
            //var user = new User(10, "Joseph", "Armstrong", "josepharmstrong", Password.OfValue("P@ssw0rd"), UserState.NOT_VERIFIED);

            var emailOperator = _serviceProvider.GetService<EmailOperator>();
            var email = await emailOperator.CreateEmailAsync(user.Id, emailAddress);

            var sessionOperator = _serviceProvider.GetService<SessionOperator>();
            var session = await sessionOperator.CreateSessionAsync(user.Id, ipAddress);

            var accessTokenOperator = _serviceProvider.GetService<AccessTokenOperator>();
            var accessToken = await accessTokenOperator.CreateAccessTokenAsync(user.Id);

            user = await GetUserByIdAsync(user.Id, true, false, true, false);
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

            var user = await GetUserByIdAsync(email.UserId, true, false, true, false);
            return user;
        }

        public async Task<User> LogInWithUsernameAsync(IPAddress ipAddress, string username, Password password)
        {
            var user = await GetUserByUsernameAsync(username, false, false, false);

            var sessionOperator = _serviceProvider.GetService<SessionOperator>();
            var session = await sessionOperator.CreateOrUpdateSessionAsync(user.Id, ipAddress);

            var accessTokenOperator = _serviceProvider.GetService<AccessTokenOperator>();
            var accessToken = await accessTokenOperator.CreateAccessTokenAsync(user.Id);

            user = await GetUserByIdAsync(user.Id, true, false, true, false);
            return user;
        }

        public async Task<List<User>> ListUsersAsync(bool includeEmail = false, 
            bool includeGoogleUser = false, bool includeAccessTokens = false, 
            bool includeSessions = false)
        {
            var users = await _userRepository.ListUsersAsync(includeEmail,
                includeGoogleUser, includeAccessTokens, includeSessions);

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

        public async Task<User> GetUserByIdAsync(long id, 
            bool includeEmail = false, bool includeGoogleUser = false, 
            bool includeAccessTokens = false, bool includeSessions = false)
        {
            var user = await _userRepository.GetUserByIdAsync(id, includeEmail,
                includeGoogleUser, includeAccessTokens, includeSessions);

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

        public async Task<User> GetUserByUsernameAsync(string username, 
            bool includeEmail = false, bool includeGoogleUser = false, 
            bool includeAccessTokens = false, bool includeSessions = false)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username, includeEmail,
                 includeGoogleUser, includeAccessTokens, includeSessions);

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
            string username, Password password = null)
        {
            var user = await _userRepository.CreateUserAsync(firstName, lastName,
                username, UserState.NOT_VERIFIED, password);
            return user;
        }

        public async Task<User> PatchUserByIdAsync(long id, string firstName = null, 
            string lastName = null, string username = null, Password password = null, 
            UserState? state = null, string emailAddress = null)
        {
            var user = await _userRepository.GetUserByIdAsync(id, true);
            user = await ApplyUserChanges(user, firstName, lastName, username, password, 
                state, emailAddress);
            user = await GetUserByIdAsync(user.Id, true, false, false, false);
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

        public async Task<string> GenerateNewUsername()
        {
            int randomNumber;
            string newUsername;
            do
            {
                randomNumber = Utils.RandomNumber(1000000, 9999999);
                newUsername = $"gamer{randomNumber}";
            }
            while (await DoesUsernameExistAsync(newUsername));
            return newUsername;
        }
    }
}
