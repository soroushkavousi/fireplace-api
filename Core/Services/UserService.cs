using FireplaceApi.Core.Identifiers;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.Validators;
using FireplaceApi.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Services
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

        public async Task<User> SignUpWithEmailAsync(IPAddress ipAddress, string firstName,
            string lastName, string username, Password password, string emailAddress)
        {
            await _userValidator.ValidateSignUpWithEmailInputParametersAsync(ipAddress,
                firstName, lastName, username, password, emailAddress);
            return await _userOperator.SignUpWithEmailAsync(ipAddress, firstName, lastName,
                username, password, emailAddress);
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

        public async Task<User> GetUserByEncodedIdOrUsernameAsync(User requestingUser, string encodedIdOrUsername)
        {
            var userIdentifier = await _userValidator.ValidateGetUserByEncodedIdOrUsernameInputParametersAsync(
                requestingUser, encodedIdOrUsername);

            var user = await _userOperator.GetUserByIdentifierAsync(userIdentifier, false,
                    false, false, false);

            return user;
        }

        public async Task<User> PatchRequestingUserAsync(User requestingUser, string firstName,
            string lastName, string username, Password oldPassword, Password password, string emailAddress)
        {
            await _userValidator.ValidatePatchUserInputParametersAsync(requestingUser, firstName,
                lastName, username, oldPassword, password, emailAddress);
            var user = await _userOperator.PatchUserByIdentifierAsync(UserIdentifier.OfId(requestingUser.Id),
                firstName: firstName, lastName: lastName, username: username,
                password: password, emailAddress: emailAddress);
            return user;
        }

        public async Task DeleteRequestingUserAsync(User requestingUser)
        {
            await _userValidator.ValidateDeleteUserInputParametersAsync(requestingUser);
            await _userOperator.DeleteUserByIdentifierAsync(UserIdentifier.OfId(requestingUser.Id));
        }
    }
}
