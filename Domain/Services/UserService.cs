using FireplaceApi.Domain.Identifiers;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Operators;
using FireplaceApi.Domain.Validators;
using FireplaceApi.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Services
{
    public class UserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly UserValidator _userValidator;
        private readonly UserOperator _userOperator;

        public UserService(ILogger<UserService> logger, UserValidator userValidator, UserOperator userOperator)
        {
            _logger = logger;
            _userValidator = userValidator;
            _userOperator = userOperator;
        }

        public async Task<User> LogInWithGoogleAsync(IPAddress ipAddress, string state,
            string code, string scope, string authUser, string prompt, string error)
        {
            await _userValidator.ValidateLogInWithGoogleInputParametersAsync(ipAddress,
                state, code, scope, authUser, prompt, error);
            return await _userOperator.LogInWithGoogleAsync(ipAddress, state, code,
                scope, authUser, prompt);
        }

        public async Task<User> SignUpWithEmailAsync(IPAddress ipAddress, string emailAddress,
            string username, Password password)
        {
            await _userValidator.ValidateSignUpWithEmailInputParametersAsync(ipAddress,
                emailAddress, username, password);
            return await _userOperator.SignUpWithEmailAsync(ipAddress,
                emailAddress, username, password);
        }

        public async Task<string> GetGoogleAuthUrlAsync(IPAddress ipAddress)
        {
            return await _userOperator.GetGoogleAuthUrlAsync(ipAddress);
        }

        public async Task<User> LogInWithEmailAsync(IPAddress ipAddress, string emailAddress, Password password)
        {
            await _userValidator.ValidateLogInWithEmailInputParametersAsync(ipAddress, emailAddress, password);
            var user = await _userOperator.LogInWithEmailAsync(ipAddress, emailAddress, password);
            return user;
        }

        public async Task<User> LogInWithUsernameAsync(IPAddress ipAddress, string username, Password password)
        {
            await _userValidator.ValidateLogInWithUsernameInputParametersAsync(ipAddress, username, password);
            var user = await _userOperator.LogInWithUsernameAsync(ipAddress, username, password);
            return user;
        }

        public async Task<User> GetRequestingUserAsync(User requestingUser,
            bool? includeEmail, bool? includeSessions)
        {
            await _userValidator.ValidateRequestingUserInputParametersAsync(requestingUser,
                includeEmail, includeSessions);
            var user = await _userOperator.GetUserByIdentifierAsync(UserIdentifier.OfId(requestingUser.Id),
                includeEmail.Value, false, false, includeSessions.Value);
            return user;
        }

        public async Task<Profile> GetUserProfileAsync(User requestingUser, string username)
        {
            await _userValidator.ValidateGetUserByUsernameInputParametersAsync(
                requestingUser, username);

            var user = await _userOperator.GetUserByIdentifierAsync(_userValidator.UserIdentifier,
                false, false, false, false);

            var profile = new Profile(user);
            return profile;
        }

        public async Task SendResetPasswordCodeAsync(string emailAddress, string resetPasswordWithCodeUrlFormat)
        {
            await _userValidator.ValidateSendResetPasswordCodeInputParametersAsync(emailAddress, resetPasswordWithCodeUrlFormat);
            await _userOperator.SendResetPasswordCode(emailAddress, resetPasswordWithCodeUrlFormat);
        }

        public async Task ResetPasswordWithCodeAsync(string emailAddress, string resetPasswordCode, Password newPassword)
        {
            await _userValidator.ValidateResetPasswordWithCodeInputParametersAsync(emailAddress, resetPasswordCode, newPassword);
            await _userOperator.ResetPasswordWithCode(_userValidator.User, newPassword);
        }

        public async Task<User> PatchRequestingUserAsync(User requestingUser, string displayName,
            string about, string avatarUrl, string bannerUrl, string username,
            Password oldPassword, Password password, string emailAddress)
        {
            await _userValidator.ValidatePatchUserInputParametersAsync(requestingUser, displayName,
                about, avatarUrl, bannerUrl, username, oldPassword, password, emailAddress);
            var user = await _userOperator.PatchUserByIdentifierAsync(
                UserIdentifier.OfId(requestingUser.Id), displayName, about, avatarUrl, bannerUrl,
                username, password, emailAddress);
            return user;
        }

        public async Task DeleteRequestingUserAsync(User requestingUser)
        {
            await _userValidator.ValidateDeleteUserInputParametersAsync(requestingUser);
            await _userOperator.DeleteUserByIdentifierAsync(_userValidator.UserIdentifier);
        }
    }
}
