using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Tools;
using FireplaceApi.Core.ValueObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Operators
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
            GoogleUserToken googleUserToken, string state,
            string scope, string authUser, string prompt)
        {
            var emailOperator = _serviceProvider.GetService<EmailOperator>();
            var googleUserOperator = _serviceProvider.GetService<GoogleUserOperator>();
            var username = await GenerateNewUsername();
            var user = await CreateUserAsync(googleUserToken.FirstName,
                googleUserToken.LastName, username, state: UserState.VERIFIED);

            await emailOperator.CreateEmailAsync(user.Id, googleUserToken.GmailAddress,
                ActivationStatus.COMPLETED);

            var redirectToUserUrl = await googleUserOperator.GetRedirectToUserUrlFromState();
            var googleUser = await googleUserOperator.CreateGoogleUserAsync(user.Id,
                googleUserToken, state,
                authUser, prompt, redirectToUserUrl);

            user = await GetUserByIdAsync(user.Id, true, true, true, false);
            return user;
        }

        private async Task<User> AddGoogleInformationToUserAsync(User user, IPAddress ipAddress,
            GoogleUserToken googleUserToken, string state,
            string scope, string authUser, string prompt)
        {
            var googleUserOperator = _serviceProvider.GetService<GoogleUserOperator>();
            var redirectToUserUrl = await googleUserOperator.GetRedirectToUserUrlFromState();
            var googleUser = await googleUserOperator.CreateGoogleUserAsync(user.Id,
                googleUserToken, state,
                authUser, prompt, redirectToUserUrl);

            if (user.State != UserState.VERIFIED)
            {
                var userOperator = _serviceProvider.GetService<UserOperator>();
                await userOperator.ApplyUserChanges(user, state: UserState.VERIFIED);
            }

            if (user.Email.Activation.Status != ActivationStatus.COMPLETED)
            {
                var emailOperator = _serviceProvider.GetService<EmailOperator>();
                await emailOperator.ApplyEmailChangesAsync(user.Email, activationStatus: ActivationStatus.COMPLETED);
            }

            user = await GetUserByIdAsync(user.Id, true, true, true, false);
            return user;
        }

        public async Task<User> LogInWithGoogleAsync(IPAddress ipAddress, string state,
            string code, string scope, string authUser, string prompt)
        {
            var googleUserOperator = _serviceProvider.GetService<GoogleUserOperator>(); // TODO too many GetService call
            var emailOperator = _serviceProvider.GetService<EmailOperator>();
            User user;
            ulong userId;
            var googleUserToken = await _googleGateway.GetgoogleUserToken(code);
            var gmailAddress = googleUserToken.GmailAddress;

            if (await googleUserOperator.DoesGoogleUserGmailAddressExistAsync(gmailAddress))
            {
                var redirectToUserUrl = await googleUserOperator.GetRedirectToUserUrlFromState();
                var googleUser = await googleUserOperator
                    .PatchGoogleUserByGmailAddressAsync(gmailAddress, googleUserToken: googleUserToken,
                        state: state, scope: scope, authUser: authUser, prompt: prompt,
                        redirectToUserUrl: redirectToUserUrl);
                userId = googleUser.UserId;
            }
            else
            {
                if (await emailOperator.DoesEmailAddressExistAsync(googleUserToken.GmailAddress))
                {
                    var email = await emailOperator.GetEmailByAddressAsync(gmailAddress, true);
                    email.User.Email = email;
                    user = await AddGoogleInformationToUserAsync(email.User, ipAddress,
                        googleUserToken, state, scope, authUser, prompt);
                    userId = user.Id;
                }
                else
                {
                    user = await SignUpWithGoogleAsync(ipAddress, googleUserToken,
                        state, scope, authUser, prompt);
                    userId = user.Id;
                }
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

            var emailOperator = _serviceProvider.GetService<EmailOperator>();
            var email = await emailOperator.CreateEmailAsync(user.Id, emailAddress);
            email = await emailOperator.SendActivationCodeAsync(email);

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

        public async Task<User> GetUserByIdAsync(ulong id,
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

        public async Task<string> GetUsernameByIdAsync(ulong id)
        {
            var username = await _userRepository.GetUsernameByIdAsync(id);
            return username;
        }

        public async Task<ulong> GetIdByUsernameAsync(string username)
        {
            var userId = await _userRepository.GetIdByUsernameAsync(username);
            return userId;
        }

        public async Task<User> CreateUserAsync(string firstName, string lastName,
            string username, Password password = null, UserState state = UserState.NOT_VERIFIED)
        {
            var id = await IdGenerator.GenerateNewIdAsync(DoesUserIdExistAsync);
            var user = await _userRepository.CreateUserAsync(id, firstName, lastName,
                username, state, password);
            return user;
        }

        public async Task<User> PatchUserByIdAsync(ulong id, string firstName = null,
            string lastName = null, string username = null, Password password = null,
            UserState? state = null, string emailAddress = null)
        {
            var user = await _userRepository.GetUserByIdAsync(id, true);
            user = await ApplyUserChanges(user, firstName, lastName, username, password,
                state, emailAddress);
            user = await GetUserByIdAsync(user.Id, true, false, false, false);
            return user;
        }

        public async Task<User> PatchUserByUsernameAsync(string currentUsername, string firstName = null,
            string lastName = null, string username = null, Password password = null,
            UserState? state = null, string emailAddress = null)
        {
            var user = await _userRepository.GetUserByUsernameAsync(currentUsername, true);
            user = await ApplyUserChanges(user, firstName, lastName, username, password,
                state, emailAddress);
            user = await GetUserByIdAsync(user.Id, true, false, false, false);
            return user;
        }

        public async Task DeleteUserByIdAsync(ulong id)
        {
            await _userRepository.DeleteUserByIdAsync(id);
        }

        public async Task DeleteUserByUsernameAsync(string username)
        {
            await _userRepository.DeleteUserByUsernameAsync(username);
        }

        public async Task<bool> DoesUserIdExistAsync(ulong id)
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
                randomNumber = Utils.GenerateRandomNumber(1000000, 9999999);
                newUsername = $"gamer{randomNumber}";
            }
            while (await DoesUsernameExistAsync(newUsername));
            return newUsername;
        }
    }
}
