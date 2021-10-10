using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.Tools;
using FireplaceApi.Core.ValueObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Validators
{
    public class UserValidator : ApiValidator
    {
        private readonly ILogger<UserValidator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly UserOperator _userOperator;

        public UserValidator(ILogger<UserValidator> logger, IConfiguration configuration,
            IServiceProvider serviceProvider, UserOperator userOperator)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _userOperator = userOperator;
        }

        public async Task ValidateOpenGoogleLogInPagenputParametersAsync(IPAddress ipAddress, string accessToken,
            string refreshToken, string tokenType, int? expiresIn, string idToken)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateLogInWithGoogleInputParametersAsync(IPAddress ipAddress, string state,
            string code, string scope, string authUser, string prompt, string error)
        {
            if (string.IsNullOrWhiteSpace(error) == false)
            {
                var serverMessage = $"Google error message: {error}";
                throw new ApiException(ErrorName.BAD_REQUEST, serverMessage);
            }
            await Task.CompletedTask;
        }

        public async Task ValidateSignUpWithEmailInputParametersAsync(IPAddress ipAddress,
            string firstName, string lastName, string username, Password password,
            string emailAddress)
        {
            ValidateParameterIsNotMissing(firstName, nameof(firstName), ErrorName.FIRST_NAME_IS_MISSING);
            ValidateParameterIsNotMissing(lastName, nameof(lastName), ErrorName.LAST_NAME_IS_MISSING);
            ValidateParameterIsNotMissing(username, nameof(username), ErrorName.USERNAME_IS_MISSING);
            ValidateParameterIsNotMissing(password, nameof(password), ErrorName.PASSWORD_IS_MISSING);
            ValidateParameterIsNotMissing(emailAddress, nameof(emailAddress), ErrorName.EMAIL_ADDRESS_IS_MISSING);
            ValidateFirstNameFormat(firstName);
            ValidateLastNameFormat(lastName);
            ValidatePasswordFormat(password);
            ValidateUsernameFormat(username);
            var emailValidator = _serviceProvider.GetService<EmailValidator>();
            emailValidator.ValidateEmailAddressFormat(emailAddress);
            await ValidateUsernameDoesNotExistAsync(username);
            await emailValidator.ValidateEmailAddressDoesNotExistAsync(emailAddress);
        }

        public async Task ValidateLogInWithEmailInputParametersAsync(IPAddress ipAddress,
            string emailAddress, Password password)
        {
            ValidateParameterIsNotMissing(emailAddress, nameof(emailAddress), ErrorName.EMAIL_ADDRESS_IS_MISSING);
            ValidateParameterIsNotMissing(password, nameof(password), ErrorName.PASSWORD_IS_MISSING);
            var emailValidator = _serviceProvider.GetService<EmailValidator>();
            emailValidator.ValidateEmailAddressFormat(emailAddress);
            await emailValidator.ValidateEmailAddressMatchWithPasswordAsync(emailAddress, password);
        }

        public async Task ValidateLogInWithUsernameInputParametersAsync(IPAddress ipAddress,
            string username, Password password)
        {
            ValidateParameterIsNotMissing(username, nameof(username), ErrorName.USERNAME_IS_MISSING);
            ValidateParameterIsNotMissing(password, nameof(password), ErrorName.PASSWORD_IS_MISSING);
            await ValidateUsernameMatchWithPasswordAsync(username, password);
        }

        public async Task ValidateListUsersInputParametersAsync(User requesterUser, bool? includeEmail,
            bool? includeSessions)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateGetUserByIdInputParametersAsync(User requesterUser, long? id,
            bool? includeEmail, bool? includeSessions)
        {
            ValidateParameterIsNotMissing(id, nameof(id), ErrorName.USER_ID_IS_MISSING);
            await ValidateUserIdExists(id.Value);
        }

        public async Task ValidateGetUserByUsernameInputParametersAsync(User requesterUser, string username,
            bool? includeEmail, bool? includeSessions)
        {
            ValidateParameterIsNotMissing(username, nameof(username), ErrorName.USERNAME_IS_MISSING);
            ValidateUsernameFormat(username);
            await ValidateUsernameExists(username);
        }

        public async Task ValidateDeleteUserByIdInputParametersAsync(User requesterUser, long? id)
        {
            ValidateParameterIsNotMissing(id, nameof(id), ErrorName.USER_ID_IS_MISSING);
            await ValidateUserIdExists(id.Value);
            ValidateRequesterUserCanAlterUser(requesterUser, id.Value);
        }

        public async Task ValidateDeleteUserByUsernameInputParametersAsync(User requesterUser, string username)
        {
            ValidateParameterIsNotMissing(username, nameof(username), ErrorName.USERNAME_IS_MISSING);
            await ValidateUsernameExists(username);
            ValidateRequesterUserCanAlterUser(requesterUser, username);
        }

        public async Task ValidatePatchUserByIdInputParametersAsync(User requesterUser, long? id, string firstName,
            string lastName, string username, Password currentPassword, Password password, string emailAddress)
        {
            ValidateParameterIsNotMissing(id, nameof(id), ErrorName.USER_ID_IS_MISSING);
            await ValidateUserIdExists(id.Value);
            ValidateRequesterUserCanAlterUser(requesterUser, id.Value);
        }

        public async Task ValidatePatchUserByUsernameInputParametersAsync(User requesterUser, string currentUsername, string firstName,
            string lastName, string username, Password currentPassword, Password password, string emailAddress)
        {
            ValidateParameterIsNotMissing(currentUsername, nameof(username), ErrorName.USERNAME_IS_MISSING);
            ValidateUsernameFormat(currentUsername);
            await ValidateUsernameExists(currentUsername);
            ValidateRequesterUserCanAlterUser(requesterUser, currentUsername);
        }

        public async Task ValidatePatchUserInputParametersAsync(User user, string firstName,
            string lastName, string username, Password oldPassword, Password password, string emailAddress)
        {
            if (firstName != null)
            {
                ValidateFirstNameFormat(firstName);
            }

            if (lastName != null)
            {
                ValidateLastNameFormat(lastName);
            }

            if (username != null)
            {
                ValidateUsernameFormat(username);
                await ValidateUsernameDoesNotExistAsync(username);
            }

            if (emailAddress != null)
            {
                var emailValidator = _serviceProvider.GetService<EmailValidator>();
                emailValidator.ValidateEmailAddressFormat(emailAddress);
                await emailValidator.ValidateEmailAddressDoesNotExistAsync(emailAddress);
            }

            ValidateBothOldPasswordAndPasswordAreNotNullIfRequested(oldPassword, password);
            if (oldPassword != null && password != null)
            {
                ValidatePasswordFormat(password);
                ValidateOldPasswordIsCorrect(user, oldPassword);
            }
        }

        public void ValidateFirstNameFormat(string firstName)
        {
            if (Regexes.FirstName.IsMatch(firstName) == false)
            {
                var serverMessage = $"First name ({firstName}) doesn't have correct format!";
                throw new ApiException(ErrorName.FIRST_NAME_NOT_VALID, serverMessage);
            }
        }

        public void ValidateLastNameFormat(string lastName)
        {
            if (Regexes.LastName.IsMatch(lastName) == false)
            {
                var serverMessage = $"Last name ({lastName}) doesn't have correct format!";
                throw new ApiException(ErrorName.LAST_NAME_NOT_VALID, serverMessage);
            }
        }

        public void ValidateBothOldPasswordAndPasswordAreNotNullIfRequested(Password oldPassword, Password password)
        {
            if ((oldPassword != null && password == null)
                || (oldPassword == null && password != null))
            {
                var serverMessage = $"Required both of old_password and password to change!";
                throw new ApiException(ErrorName.REQUIRED_BOTH_OF_OLD_PASSWORD_AND_PASSWORD, serverMessage);
            }
        }

        public void ValidateOldPasswordIsCorrect(User requesterUser, Password oldPassword)
        {
            if (Equals(requesterUser.Password.Hash, oldPassword.Hash))
            {
                var serverMessage = $"Old paassword ({oldPassword}) is not correct!";
                throw new ApiException(ErrorName.OLD_PASSWORD_NOT_CORRECT, serverMessage);
            }
        }

        public async Task ValidateUserIdExists(long id)
        {
            if (await _userOperator.DoesUserIdExistAsync(id) == false)
            {
                var serverMessage = $"User {id} doesn't exists!";
                throw new ApiException(ErrorName.USER_ID_DOES_NOT_EXIST_OR_ACCESS_DENIED, serverMessage);
            }
        }

        public void ValidateUsernameFormat(string username)
        {
            if (Regexes.UsernameMinLength.IsMatch(username) == false)
            {
                var serverMessage = $"Username ({username}) doesn't have the minimum length!";
                throw new ApiException(ErrorName.USERNAME_MIN_LENGTH, serverMessage);
            }
            if (Regexes.UsernameMaxLength.IsMatch(username) == false)
            {
                var serverMessage = $"Username ({username}) exceeds the maximum length!";
                throw new ApiException(ErrorName.USERNAME_MAX_LENGTH, serverMessage);
            }
            if (Regexes.UsernameStart.IsMatch(username) == false)
            {
                var serverMessage = $"Username ({username}) has wrong starts!";
                throw new ApiException(ErrorName.USERNAME_WRONG_START, serverMessage);
            }
            if (Regexes.UsernameEnd.IsMatch(username) == false)
            {
                var serverMessage = $"Username ({username}) has wrong end!";
                throw new ApiException(ErrorName.USERNAME_WRONG_END, serverMessage);
            }
            if (Regexes.UsernameSafeConsecutives.IsMatch(username) == false)
            {
                var serverMessage = $"Username ({username}) has invalid consecutive!";
                throw new ApiException(ErrorName.USERNAME_INVALID_CONSECUTIVE, serverMessage);
            }
            if (Regexes.UsernameValidCharacters.IsMatch(username) == false)
            {
                var serverMessage = $"Username ({username}) has invalid characters!";
                throw new ApiException(ErrorName.USERNAME_VALID_CHARACTERS, serverMessage);
            }
        }

        public async Task ValidateUsernameDoesNotExistAsync(string username)
        {
            if (await _userOperator.DoesUsernameExistAsync(username))
            {
                var serverMessage = $"Username {username} already exists!";
                throw new ApiException(ErrorName.USERNAME_EXISTS, serverMessage);
            }
        }

        public async Task ValidateUsernameExists(string username)
        {
            if (await _userOperator.DoesUsernameExistAsync(username) == false)
            {
                var serverMessage = $"Username {username} doesn't exist!";
                throw new ApiException(ErrorName.USERNAME_DOES_NOT_EXIST_OR_ACCESS_DENIED, serverMessage);
            }
        }

        public void ValidatePasswordFormat(Password password)
        {
            if (Regexes.PasswordMinLength.IsMatch(password.Value) == false)
            {
                var serverMessage = $"Password ({password.Value}) doesn't have the minimum length!";
                throw new ApiException(ErrorName.PASSWORD_MIN_LENGTH, serverMessage);
            }
            if (Regexes.PasswordMaxLength.IsMatch(password.Value) == false)
            {
                var serverMessage = $"Password ({password.Value}) exceeds the maximum length!";
                throw new ApiException(ErrorName.PASSWORD_MAX_LENGTH, serverMessage);
            }
            if (Regexes.PasswordAnUppercaseLetter.IsMatch(password.Value) == false)
            {
                var serverMessage = $"Password ({password.Value}) doesn't have an uppercase letter!";
                throw new ApiException(ErrorName.PASSWORD_AN_UPPERCASE_LETTER, serverMessage);
            }
            if (Regexes.PasswordANumber.IsMatch(password.Value) == false)
            {
                var serverMessage = $"Password ({password.Value}) doesn't have a number!";
                throw new ApiException(ErrorName.PASSWORD_A_NUMBER, serverMessage);
            }
            if (Regexes.PasswordALowercaseLetter.IsMatch(password.Value) == false)
            {
                var serverMessage = $"Password ({password.Value}) doesn't have a lowercase letter!";
                throw new ApiException(ErrorName.PASSWORD_A_LOWERCASE_LETTER, serverMessage);
            }
            if (Regexes.PasswordValidCharacters.IsMatch(password.Value) == false)
            {
                var serverMessage = $"Password ({password.Value}) doesn't have valid characters!";
                throw new ApiException(ErrorName.PASSWORD_VALID_CHARACTERS, serverMessage);
            }
        }

        public async Task ValidateUsernameMatchWithPasswordAsync(string username, Password password)
        {
            var user = await _userOperator.GetUserByUsernameAsync(username);
            if (user == null)
            {
                var serverMessage = $"Username {username} doesn't exist! password: {password.Value}";
                throw new ApiException(ErrorName.AUTHENTICATION_FAILED, serverMessage);
            }

            if (string.Equals(user.Password.Hash, password.Hash) == false)
            {
                var serverMessage = $"Username {username} isn't match with password {password.Value}!";
                throw new ApiException(ErrorName.AUTHENTICATION_FAILED, serverMessage);
            }
        }

        public void ValidateRequesterUserCanAlterUser(User requesterUser, long id)
        {
            if (requesterUser.Id != id)
            {
                var serverMessage = $"requesterUser {requesterUser.Id} can't alter user {id}";
                throw new ApiException(ErrorName.USER_ID_DOES_NOT_EXIST_OR_ACCESS_DENIED, serverMessage);
            }
        }

        public void ValidateRequesterUserCanAlterUser(User requesterUser, string username)
        {
            if (string.Equals(requesterUser.Username, username, StringComparison.OrdinalIgnoreCase))
            {
                var serverMessage = $"requesterUser {requesterUser.Id} can't alter to user {username}";
                throw new ApiException(ErrorName.USERNAME_DOES_NOT_EXIST_OR_ACCESS_DENIED, serverMessage);
            }
        }

        //public async Task ValidatePassword(string username, string password)
        //{
        //    var user = await _userOperator.GetIdentityByUsernameAsync(username);
        //    if (string.Equals(password, user.Password) == false)
        //    {
        //        var serverMessage = $"User {username} authentication failed with password {password}.";
        //        throw new ApiException(ErrorId.WRONG_BASIC_AUTHENTICATION, serverMessage);
        //    }
        //}

        //public void ValidatePasswordHasMinimumLength(string password, string field)
        //{
        //    if (password.Length < 8)
        //    {
        //        var serverMessage = $"Field {field} => {password} doesn't have minimum length.";
        //        throw new ApiException(ErrorId.PASSWORD_MINIMUM_LENGTH, serverMessage, field);
        //    }
        //}

        //public async Task<bool?> DoesUsernameBelong?ToUser(string username, long? userId)
        //{
        //    var userEntity = await _userOperator.GetIdentityByUsernameAsync(username);
        //    if (string.Equals(userEntity.Username, username, StringComparison.OrdinalIgnoreCase))
        //        return true;
        //    return false;
        //}
    }
}
