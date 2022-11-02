using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Identifiers;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.Tools;
using FireplaceApi.Core.ValueObjects;
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
        private readonly IServiceProvider _serviceProvider;
        private readonly UserOperator _userOperator;

        public UserIdentifier UserIdentifier { get; private set; }

        public UserValidator(ILogger<UserValidator> logger,
            IServiceProvider serviceProvider, UserOperator userOperator)
        {
            _logger = logger;
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
            string emailAddress, string username, Password password)
        {
            ValidateParameterIsNotMissing(emailAddress, nameof(emailAddress), ErrorName.EMAIL_ADDRESS_IS_MISSING);
            ValidateParameterIsNotMissing(username, nameof(username), ErrorName.USERNAME_IS_MISSING);
            ValidateParameterIsNotMissing(password, nameof(password), ErrorName.PASSWORD_IS_MISSING);
            ValidateUsernameFormat(username);
            ValidatePasswordFormat(password);
            var emailValidator = _serviceProvider.GetService<EmailValidator>();
            emailValidator.ValidateEmailAddressFormat(emailAddress);
            await ValidateUserIdentifierDoesNotExistAsync(UserIdentifier.OfUsername(username));
            await emailValidator.ValidateEmailIdentifierDoesNotExistAsync(EmailIdentifier.OfAddress(emailAddress));
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

        public async Task ValidateRequestingUserInputParametersAsync(User requestingUser,
            bool? includeEmail, bool? includeSessions)
        {
            await Task.CompletedTask;
        }

        public async Task ValidateGetUserByUsernameInputParametersAsync(
            User requestingUser, string username)
        {
            ValidateUsernameFormat(username);
            UserIdentifier = UserIdentifier.OfUsername(username);
            await ValidateUserIdentifierExists(UserIdentifier);
        }

        public async Task ValidateDeleteUserInputParametersAsync(User requestingUser)
        {
            UserIdentifier = UserIdentifier.OfId(requestingUser.Id);
            await Task.CompletedTask;
        }

        public async Task ValidatePatchUserInputParametersAsync(User user, string displayName,
            string about, string avatarUrl, string bannerUrl, string username,
            Password oldPassword, Password password, string emailAddress)
        {
            if (displayName != null)
                ValidateDisplayNameFormat(displayName);

            if (about != null)
                ValidateAboutFormat(about);

            if (avatarUrl != null)
                ValidateUrlStringFormat(avatarUrl);

            if (bannerUrl != null)
                ValidateUrlStringFormat(bannerUrl);

            if (username != null)
            {
                ValidateUsernameFormat(username);
                await ValidateUserIdentifierDoesNotExistAsync(UserIdentifier.OfUsername(username));
            }

            if (emailAddress != null)
            {
                var emailValidator = _serviceProvider.GetService<EmailValidator>();
                emailValidator.ValidateEmailAddressFormat(emailAddress);
                await emailValidator.ValidateEmailIdentifierDoesNotExistAsync(EmailIdentifier.OfAddress(emailAddress));
            }

            ValidateBothOldPasswordAndPasswordAreNotNullIfRequested(oldPassword, password);
            if (oldPassword != null && password != null)
            {
                ValidatePasswordFormat(password);
                ValidateOldPasswordIsCorrect(user, oldPassword);
            }
        }

        public async Task<UserIdentifier> ValidateMultipleIdentifiers(string encodedId,
            string username, bool throwException = true)
        {
            var id = ValidateEncodedIdFormat(encodedId, "username", false);
            if (id.HasValue)
            {
                var identifier = UserIdentifier.OfId(id.Value);
                if (await ValidateUserIdentifierExists(identifier, false))
                    return identifier;
            }

            if (ValidateUsernameFormat(username, false))
            {
                var identifier = UserIdentifier.OfUsername(username);
                if (await ValidateUserIdentifierExists(identifier, false))
                    return identifier;
            }

            if (throwException)
            {
                var serverMessage = $"Input encodedIdOrUsername ({new { encodedId, id, username }.ToJson()}) is not valid!";
                throw new ApiException(ErrorName.USER_DOES_NOT_EXIST_OR_ACCESS_DENIED, serverMessage);
            }
            return default;
        }

        public void ValidateDisplayNameFormat(string displayName)
        {
            if (displayName.Length > 80)
            {
                var serverMessage = $"Invalid displayName format! ({displayName})!";
                throw new ApiException(ErrorName.DISPLAY_NAME_FORMAT_IS_NOT_VALID, serverMessage);
            }
        }

        public void ValidateAboutFormat(string about)
        {
            if (about.Length > 2000)
            {
                var serverMessage = $"Invalid about format! ({about})!";
                throw new ApiException(ErrorName.ABOUT_FORMAT_IS_NOT_VALID, serverMessage);
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

        public void ValidateOldPasswordIsCorrect(User requestingUser, Password oldPassword)
        {
            if (Equals(requestingUser.Password.Hash, oldPassword.Hash))
            {
                var serverMessage = $"Old paassword is not correct!";
                throw new ApiException(ErrorName.OLD_PASSWORD_NOT_CORRECT, serverMessage);
            }
        }

        public async Task<bool> ValidateUserIdentifierExists(UserIdentifier identifier,
            bool throwException = true)
        {
            if (await _userOperator.DoesUserIdentifierExistAsync(identifier))
                return true;

            if (throwException)
            {
                var serverMessage = $"User {identifier.ToJson()} doesn't exists!";
                throw new ApiException(ErrorName.USER_DOES_NOT_EXIST_OR_ACCESS_DENIED, serverMessage);
            }
            return false;
        }

        public async Task<bool> ValidateUserIdentifierDoesNotExistAsync(UserIdentifier identifier,
            bool throwException = true)
        {
            if (await _userOperator.DoesUserIdentifierExistAsync(identifier) == false)
                return true;

            if (throwException)
            {
                switch (identifier)
                {
                    case UserUsernameIdentifier usernameIdentifier:
                        var serverMessage = $"Username {usernameIdentifier.Username} already exists!";
                        throw new ApiException(ErrorName.USERNAME_ALREADY_EXISTS, serverMessage);
                }
            }
            return false;
        }

        public bool ValidateUserIdentifierFormat(
            UserIdentifier identifier, bool throwException = true)
        {
            switch (identifier)
            {
                case UserIdIdentifier idIdentifier:
                    break;
                case UserUsernameIdentifier usernameIdentifier:
                    if (!ValidateUsernameFormat(usernameIdentifier.Username, throwException))
                        return false;
                    break;
            }
            return true;
        }

        public bool ValidateUsernameFormat(string username, bool throwException = true)
        {
            if (Regexes.UsernameMinLength.IsMatch(username) == false)
            {
                var serverMessage = $"Username ({username}) doesn't have the minimum length!";
                return throwException ?
                    throw new ApiException(ErrorName.USERNAME_MIN_LENGTH, serverMessage) : false;
            }
            if (Regexes.UsernameMaxLength.IsMatch(username) == false)
            {
                var serverMessage = $"Username ({username}) exceeds the maximum length!";
                return throwException ?
                    throw new ApiException(ErrorName.USERNAME_MAX_LENGTH, serverMessage) : false;
            }
            if (Regexes.UsernameStart.IsMatch(username) == false)
            {
                var serverMessage = $"Username ({username}) has wrong starts!";
                return throwException ?
                    throw new ApiException(ErrorName.USERNAME_WRONG_START, serverMessage) : false;
            }
            if (Regexes.UsernameEnd.IsMatch(username) == false)
            {
                var serverMessage = $"Username ({username}) has wrong end!";
                return throwException ?
                    throw new ApiException(ErrorName.USERNAME_WRONG_END, serverMessage) : false;
            }
            if (Regexes.UsernameSafeConsecutives.IsMatch(username) == false)
            {
                var serverMessage = $"Username ({username}) has invalid consecutive!";
                return throwException ?
                    throw new ApiException(ErrorName.USERNAME_INVALID_CONSECUTIVE, serverMessage) : false;
            }
            if (Regexes.UsernameValidCharacters.IsMatch(username) == false)
            {
                var serverMessage = $"Username ({username}) has invalid characters!";
                return throwException ?
                    throw new ApiException(ErrorName.USERNAME_VALID_CHARACTERS, serverMessage) : false;
            }

            return true;
        }

        public void ValidatePasswordFormat(Password password)
        {
            if (Regexes.PasswordMinLength.IsMatch(password.Value) == false)
            {
                var serverMessage = $"Input password doesn't have the minimum length!";
                throw new ApiException(ErrorName.PASSWORD_MIN_LENGTH, serverMessage);
            }
            if (Regexes.PasswordMaxLength.IsMatch(password.Value) == false)
            {
                var serverMessage = $"Input password exceeds the maximum length!";
                throw new ApiException(ErrorName.PASSWORD_MAX_LENGTH, serverMessage);
            }
            if (Regexes.PasswordAnUppercaseLetter.IsMatch(password.Value) == false)
            {
                var serverMessage = $"Input password doesn't have an uppercase letter!";
                throw new ApiException(ErrorName.PASSWORD_AN_UPPERCASE_LETTER, serverMessage);
            }
            if (Regexes.PasswordANumber.IsMatch(password.Value) == false)
            {
                var serverMessage = $"Input password doesn't have a number!";
                throw new ApiException(ErrorName.PASSWORD_A_NUMBER, serverMessage);
            }
            if (Regexes.PasswordALowercaseLetter.IsMatch(password.Value) == false)
            {
                var serverMessage = $"Input password doesn't have a lowercase letter!";
                throw new ApiException(ErrorName.PASSWORD_A_LOWERCASE_LETTER, serverMessage);
            }
            if (Regexes.PasswordValidCharacters.IsMatch(password.Value) == false)
            {
                var serverMessage = $"Input password doesn't have valid characters!";
                throw new ApiException(ErrorName.PASSWORD_VALID_CHARACTERS, serverMessage);
            }
        }

        public async Task ValidateUsernameMatchWithPasswordAsync(string username, Password password)
        {
            var user = await _userOperator.GetUserByIdentifierAsync(UserIdentifier.OfUsername(username));
            if (user == null)
            {
                var serverMessage = $"Username {username} doesn't exist! Password Hash: {password.Hash}";
                throw new ApiException(ErrorName.AUTHENTICATION_FAILED, serverMessage);
            }

            if (string.Equals(user.Password.Hash, password.Hash) == false)
            {
                var serverMessage = $"Input password is not correct! Username: {username}, Password Hash: {password.Hash}";
                throw new ApiException(ErrorName.AUTHENTICATION_FAILED, serverMessage);
            }
        }

        public void ValidateRequestingUserCanAlterUser(User requestingUser, UserIdentifier requestedUserIdentifier)
        {
            switch (requestedUserIdentifier)
            {
                case UserIdIdentifier idIdentifier:
                    if (requestingUser.Id == idIdentifier.Id)
                        return;
                    break;
                case UserUsernameIdentifier usernameIdentifier:
                    if (string.Equals(requestingUser.Username, usernameIdentifier.Username,
                        StringComparison.OrdinalIgnoreCase))
                        return;
                    break;
            }
            var serverMessage = $"requestingUser {requestingUser.Id} can't alter " +
                $"user {requestedUserIdentifier.ToJson()}";
            throw new ApiException(ErrorName.USER_DOES_NOT_EXIST_OR_ACCESS_DENIED, serverMessage);

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

        //public async Task<bool?> DoesUsernameBelong?ToUser(string username, ulong? userId)
        //{
        //    var userEntity = await _userOperator.GetIdentityByUsernameAsync(username);
        //    if (string.Equals(userEntity.Username, username, StringComparison.OrdinalIgnoreCase))
        //        return true;
        //    return false;
        //}
    }
}
