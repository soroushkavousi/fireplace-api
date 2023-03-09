using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Identifiers;
using FireplaceApi.Domain.Interfaces;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Tools;
using FireplaceApi.Domain.ValueObjects;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Operators
{
    public class UserOperator
    {
        private readonly ILogger<UserOperator> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserRepository _userRepository;
        private readonly IGoogleGateway _googleGateway;

        public UserOperator(ILogger<UserOperator> logger,
            IServiceProvider serviceProvider, IUserRepository userRepository,
            IGoogleGateway googleGateway)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _userRepository = userRepository;
            _googleGateway = googleGateway;
        }

        public async Task<User> LogInWithGoogleAsync(IPAddress ipAddress, string state,
            string code, string scope, string authUser, string prompt)
        {
            var googleUserOperator = _serviceProvider.GetService<GoogleUserOperator>();
            var emailOperator = _serviceProvider.GetService<EmailOperator>();
            User user;
            ulong userId;
            var googleUserToken = await _googleGateway.GetGoogleUserToken(code);
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
                if (await emailOperator.DoesEmailIdentifierExistAsync(
                    EmailIdentifier.OfAddress(googleUserToken.GmailAddress)))
                {
                    var email = await emailOperator.GetEmailByIdentifierAsync(
                        EmailIdentifier.OfAddress(gmailAddress), true);
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

            user = await GetUserByIdentifierAsync(UserIdentifier.OfId(userId), true, true, true, false);
            return user;
        }

        private async Task<User> AddGoogleInformationToUserAsync(User user,
            IPAddress ipAddress, GoogleUserToken googleUserToken, string state,
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
                await emailOperator.ApplyEmailChangesAsync(user.Email,
                    activationStatus: ActivationStatus.COMPLETED);
            }

            user = await GetUserByIdentifierAsync(UserIdentifier.OfId(user.Id),
                true, true, true, false);
            return user;
        }

        private async Task<User> SignUpWithGoogleAsync(IPAddress ipAddress,
            GoogleUserToken googleUserToken, string state,
            string scope, string authUser, string prompt)
        {
            var emailOperator = _serviceProvider.GetService<EmailOperator>();
            var googleUserOperator = _serviceProvider.GetService<GoogleUserOperator>();
            var username = await GenerateNewUsername();
            var displayName = googleUserToken.FullName;
            if (string.IsNullOrWhiteSpace(displayName))
                displayName = $"{googleUserToken.FirstName} {googleUserToken.LastName}";
            var user = await CreateUserAsync(username, state: UserState.VERIFIED,
                displayName: displayName, avatarUrl: googleUserToken.PictureUrl);

            await emailOperator.CreateEmailAsync(user.Id, googleUserToken.GmailAddress,
                ActivationStatus.COMPLETED);

            var redirectToUserUrl = await googleUserOperator.GetRedirectToUserUrlFromState();
            var googleUser = await googleUserOperator.CreateGoogleUserAsync(user.Id,
                googleUserToken, state, authUser, prompt, redirectToUserUrl);

            user = await GetUserByIdentifierAsync(UserIdentifier.OfId(user.Id),
                true, true, true, false);
            return user;
        }

        public async Task<string> GetGoogleAuthUrlAsync(IPAddress ipAddress)
        {
            await Task.CompletedTask;
            var googleAuthUrl = _googleGateway.GetAuthUrl();
            _logger.LogAppInformation($"googleLogInPageUrl: {googleAuthUrl}");
            return googleAuthUrl;
        }

        public async Task<User> SignUpWithEmailAsync(IPAddress ipAddress, string emailAddress,
            string username, Password password)
        {
            var user = await CreateUserAsync(username, password);

            var emailOperator = _serviceProvider.GetService<EmailOperator>();
            var email = await emailOperator.CreateEmailAsync(user.Id, emailAddress);
            email = await emailOperator.SendActivationCodeAsync(email);

            var sessionOperator = _serviceProvider.GetService<SessionOperator>();
            var session = await sessionOperator.CreateSessionAsync(user.Id, ipAddress);

            var accessTokenOperator = _serviceProvider.GetService<AccessTokenOperator>();
            var accessToken = await accessTokenOperator.CreateAccessTokenAsync(user.Id);

            user = await GetUserByIdentifierAsync(UserIdentifier.OfId(user.Id), true, false,
                true, false);
            return user;
        }

        public async Task<User> LogInWithEmailAsync(IPAddress ipAddress, string emailAddress, Password password)
        {
            var emailOperator = _serviceProvider.GetService<EmailOperator>();
            var email = await emailOperator.GetEmailByIdentifierAsync(EmailIdentifier.OfAddress(emailAddress));

            var sessionOperator = _serviceProvider.GetService<SessionOperator>();
            var session = await sessionOperator.CreateOrUpdateSessionAsync(email.UserId, ipAddress);

            var accessTokenOperator = _serviceProvider.GetService<AccessTokenOperator>();
            var accessToken = await accessTokenOperator.CreateAccessTokenAsync(email.UserId);

            var user = await GetUserByIdentifierAsync(UserIdentifier.OfId(email.UserId), true, false, true, false);
            return user;
        }

        public async Task<User> LogInWithUsernameAsync(IPAddress ipAddress, string username, Password password)
        {
            var user = await GetUserByIdentifierAsync(UserIdentifier.OfUsername(username), false, false, false);

            var sessionOperator = _serviceProvider.GetService<SessionOperator>();
            var session = await sessionOperator.CreateOrUpdateSessionAsync(user.Id, ipAddress);

            var accessTokenOperator = _serviceProvider.GetService<AccessTokenOperator>();
            var accessToken = await accessTokenOperator.CreateAccessTokenAsync(user.Id);

            user = await GetUserByIdentifierAsync(UserIdentifier.OfId(user.Id), true, false, true, false);
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

        public async Task<User> GetUserByIdentifierAsync(UserIdentifier identifier,
            bool includeEmail = false, bool includeGoogleUser = false,
            bool includeAccessTokens = false, bool includeSessions = false)
        {
            var user = await _userRepository.GetUserByIdentifierAsync(identifier, includeEmail,
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

        public async Task<User> CreateUserAsync(string username, Password password = null,
            UserState state = UserState.NOT_VERIFIED, string displayName = null,
            string about = null, string avatarUrl = null, string bannerUrl = null)
        {
            var id = await IdGenerator.GenerateNewIdAsync(
                (id) => DoesUserIdentifierExistAsync(UserIdentifier.OfId(id)));
            var user = await _userRepository.CreateUserAsync(id, username, state,
                password, displayName, about, avatarUrl, bannerUrl);
            return user;
        }

        public async Task SendResetPasswordCode(string emailAddress, string resetPasswordUrl)
        {
            var emailOperator = _serviceProvider.GetService<EmailOperator>();
            var email = await emailOperator.GetEmailByIdentifierAsync(EmailIdentifier.OfAddress(emailAddress), true);
            var user = email.User;
            var resetPasswordCode = GenerateNewResetPasswordCode();
            user = await ApplyUserChanges(user, resetPasswordCode: resetPasswordCode);
            var queryParameters = new Dictionary<string, string>()
            {
                { "code", resetPasswordCode },
                { "email", email.Address },
            };
            var resetPasswordUrlWithQueryParameters = QueryHelpers.AddQueryString(resetPasswordUrl, queryParameters);
            var emailSubject = "Fireplace Reset Password";
            var emailMessage = $"<h4>Hello {user.Username},</h4>" +
                $"A request has been received to reset the password for your account.<br/>" +
                $"Use the following code and link to change your password.<br/>" +
                $"<br/><b>Code: {resetPasswordCode}</b><br/>" +
                $"<b>Go to <a href=\"{resetPasswordUrlWithQueryParameters}\" target=\"_blank\">Reset Password</a></b><br/>" +
                $"<br/>Best Regards,<br/>" +
                $"The Fireplace Team";
            _ = emailOperator.SendEmailMessage(emailAddress, emailSubject, emailMessage);
        }

        public async Task<User> PatchUserByIdentifierAsync(UserIdentifier userIdentifier,
            string displayName = null, string about = null, string avatarUrl = null,
            string bannerUrl = null, string username = null, UserState? state = null)
        {
            var user = await _userRepository.GetUserByIdentifierAsync(userIdentifier, true);
            user = await ApplyUserChanges(user, displayName, about, avatarUrl, bannerUrl,
                username, state);
            user = await GetUserByIdentifierAsync(UserIdentifier.OfId(user.Id), true, false, false, false);
            return user;
        }

        public async Task PatchRequestingUserPasswordAsync(User user,
            Password password = null)
        {
            user.Password = password;
            user = await _userRepository.UpdateUserAsync(user);
        }

        public async Task DeleteUserByIdentifierAsync(UserIdentifier userIdentifier)
        {
            await _userRepository.DeleteUserByIdentifierAsync(userIdentifier);
        }

        public async Task<bool> DoesUserIdentifierExistAsync(UserIdentifier userIdentifier)
        {
            var userIdExists = await _userRepository.DoesUserIdentifierExistAsync(userIdentifier);
            return userIdExists;
        }

        public async Task<string> GenerateNewUsername()
        {
            int randomNumber;
            string newUsername;
            do
            {
                randomNumber = Utils.GenerateRandomNumber(1000000, 9999999);
                newUsername = $"user{randomNumber}";
            }
            while (await DoesUserIdentifierExistAsync(UserIdentifier.OfUsername(newUsername)));
            return newUsername;
        }

        public string GenerateNewResetPasswordCode()
        {
            return Utils.GenerateRandomString(10, uppercase: true, special: false);
        }

        public async Task<User> ApplyUserChanges(User user, string displayName = null,
            string about = null, string avatarUrl = null, string bannerUrl = null,
            string username = null, UserState? state = null, string resetPasswordCode = null)
        {
            var foundAnyChange = false;
            if (displayName != null)
            {
                user.DisplayName = displayName;
                foundAnyChange = true;
            }

            if (about != null)
            {
                user.About = about;
                foundAnyChange = true;
            }

            if (avatarUrl != null)
            {
                user.AvatarUrl = avatarUrl;
                foundAnyChange = true;
            }

            if (bannerUrl != null)
            {
                user.BannerUrl = bannerUrl;
                foundAnyChange = true;
            }

            if (username != null)
            {
                user.Username = username;
                await _userRepository.UpdateUsernameAsync(user.Id, username);
            }

            if (state != null)
            {
                user.State = state.Value;
                foundAnyChange = true;
            }

            if (resetPasswordCode != null)
            {
                user.ResetPasswordCode = resetPasswordCode;
                foundAnyChange = true;
            }

            if (foundAnyChange)
                user = await _userRepository.UpdateUserAsync(user);

            return user;
        }
    }
}
