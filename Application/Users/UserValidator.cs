using FireplaceApi.Application.Emails;
using FireplaceApi.Application.Errors;
using FireplaceApi.Domain.Emails;
using FireplaceApi.Domain.Errors;
using FireplaceApi.Domain.Users;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Users;

public class UserValidator : ApplicationValidator
{
    private readonly ILogger<UserValidator> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly UserOperator _userOperator;

    public User User { get; private set; }
    public UserIdentifier UserIdentifier { get; private set; }

    public UserValidator(ILogger<UserValidator> logger,
        IServiceProvider serviceProvider, UserOperator userOperator)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _userOperator = userOperator;
    }

    //public async Task ValidateOpenGoogleLogInPagenputParametersAsync(IPAddress ipAddress, string accessToken,
    //    string refreshToken, string tokenType, int? expiresIn, string idToken)
    //{
    //    await Task.CompletedTask;
    //}

    public async Task ValidateLogInWithGoogleInputParametersAsync(IPAddress ipAddress, string state,
        string code, string scope, string authUser, string prompt, string error)
    {
        if (string.IsNullOrWhiteSpace(error) == false)
            throw new InternalServerException("Google error message!", new { error });

        await Task.CompletedTask;
    }

    public async Task ValidateSignUpWithEmailInputParametersAsync(IPAddress ipAddress,
        string emailAddress, string username, Password password)
    {
        var emailValidator = _serviceProvider.GetService<EmailValidator>();
        await ValidateUserIdentifierDoesNotExistAsync(UserIdentifier.OfUsername(username));
        await emailValidator.ValidateEmailIdentifierDoesNotExistAsync(EmailIdentifier.OfAddress(emailAddress));
    }

    public async Task ValidateLogInWithEmailInputParametersAsync(IPAddress ipAddress,
        string emailAddress, Password password)
    {
        var emailValidator = _serviceProvider.GetService<EmailValidator>();
        await emailValidator.ValidateEmailAddressMatchWithPasswordAsync(emailAddress, password);
    }

    public async Task ValidateLogInWithUsernameInputParametersAsync(IPAddress ipAddress,
        string username, Password password)
    {
        await ValidateUsernameMatchWithPasswordAsync(username, password);
    }

    public async Task ValidateGetRequestingUserInputParametersAsync(ulong userId,
        bool? includeEmail, bool? includeSessions)
    {
        await Task.CompletedTask;
    }

    public async Task ValidateGetUserProfileInputParametersAsync(
        ulong userId, UserIdentifier identifier)
    {
        await ValidateUserIdentifierExists(identifier);
    }

    public async Task ValidateCreateRequestingUserPasswordInputParametersAsync(ulong userId,
        Password password)
    {
        User = await _userOperator.GetUserByIdentifierAsync(UserIdentifier.OfId(userId));
        ValidateUserPasswordNotExist(User);
        await Task.CompletedTask;
    }

    public async Task ValidateSendResetPasswordCodeInputParametersAsync(string emailAddress, string resetPasswordWithCodeUrlFormat)
    {
        var emailValidator = _serviceProvider.GetService<EmailValidator>();
        emailValidator.ValidateEmailAddressFormat(emailAddress);
        await emailValidator.ValidateEmailIdentifierExistsAsync(EmailIdentifier.OfAddress(emailAddress));
    }

    public async Task ValidateResetPasswordWithCodeInputParametersAsync(string emailAddress,
        string resetPasswordCode, Password newPassword)
    {
        var emailValidator = _serviceProvider.GetService<EmailValidator>();
        var email = await emailValidator.ValidateAndGetEmailAsync(EmailIdentifier.OfAddress(emailAddress));
        User = email.User;
        ValidateResetPasswordCodeIsCorrectAsync(User, resetPasswordCode);
    }

    public async Task ValidateDeleteUserInputParametersAsync(ulong userId)
    {
        UserIdentifier = UserIdentifier.OfId(userId);
        await Task.CompletedTask;
    }

    public async Task ValidatePatchUserInputParametersAsync(ulong userId, string displayName,
        string about, string avatarUrl, string bannerUrl, string username)
    {
        if (username != null)
        {
            await ValidateUserIdentifierDoesNotExistAsync(UserIdentifier.OfUsername(username));
        }
    }

    public async Task ValidatePatchRequestingUserPasswordInputParametersAsync(ulong userId,
        Password password, Password newPassword)
    {
        User = await _userOperator.GetUserByIdentifierAsync(UserIdentifier.OfId(userId));
        ValidateUserPasswordExists(User);
        ValidateInputPasswordIsCorrectForRequestingUser(User, password);
        await Task.CompletedTask;
    }

    public void ValidateDisplayNameFormat(string displayName)
    {
        if (displayName.Length > 80)
            throw new DisplayNameInvalidFormatException(displayName);
    }

    public void ValidateAboutFormat(string about)
    {
        if (about.Length > 2000)
            throw new AboutInvalidFormatException(about);
    }

    public void ValidateInputPasswordIsCorrectForRequestingUser(User user, Password password)
    {
        if (!Equals(user.Password.Hash, password.Hash))
            throw new PasswordIncorrectValueException(password.Hash);
    }

    public async Task<bool> ValidateUserIdentifierExists(UserIdentifier identifier,
        bool throwException = true)
    {
        if (await _userOperator.DoesUserIdentifierExistAsync(identifier))
            return true;

        if (throwException)
            throw new UserNotExistException(identifier);

        return false;
    }

    public async Task<bool> ValidateUserIdentifierDoesNotExistAsync(UserIdentifier identifier,
        bool throwException = true)
    {
        if (await _userOperator.DoesUserIdentifierExistAsync(identifier) == false)
            return true;

        if (throwException)
            throw new UserAlreadyExistsException(identifier);

        return false;
    }

    public bool ValidateUsernameFormat(string username, bool throwException = true)
    {
        if (Regexes.UsernameMinLength.IsMatch(username) == false)
            return throwException ? throw new UsernameInvalidFormatException(username,
                "The username doesn't have the minimum length!") : false;

        if (Regexes.UsernameMaxLength.IsMatch(username) == false)
            return throwException ? throw new UsernameInvalidFormatException(username,
                "The username exceeds the maximum length!") : false;

        if (Regexes.UsernameStart.IsMatch(username) == false)
            return throwException ? throw new UsernameInvalidFormatException(username,
                "The username has wrong starts!") : false;

        if (Regexes.UsernameEnd.IsMatch(username) == false)
            return throwException ? throw new UsernameInvalidFormatException(username,
                "The username has wrong end!") : false;

        if (Regexes.UsernameSafeConsecutives.IsMatch(username) == false)
            return throwException ? throw new UsernameInvalidFormatException(username,
                "The username has invalid consecutive!") : false;

        if (Regexes.UsernameValidCharacters.IsMatch(username) == false)
            return throwException ? throw new UsernameInvalidFormatException(username,
                "The username has invalid characters!") : false;

        return true;
    }

    public Password ValidatePasswordFormat(string passwordValue, FieldName field = null)
    {
        if (field == null)
            field = FieldName.PASSWORD;

        string reason = FindPasswordProblem(passwordValue);
        if (!string.IsNullOrEmpty(reason))
        {
            throw field.Name switch
            {
                nameof(FieldName.PASSWORD) => throw new PasswordInvalidFormatException(passwordValue, reason),
                nameof(FieldName.NEW_PASSWORD) => throw new NewPasswordInvalidFormatException(passwordValue, reason),
                _ => throw new InternalServerException("Not known password field!")
            };
        }
        return Password.OfValue(passwordValue);
    }

    private string FindPasswordProblem(string passwordValue)
    {
        if (Regexes.PasswordMinLength.IsMatch(passwordValue) == false)
            return "Input password doesn't have the minimum length!";

        if (Regexes.PasswordMaxLength.IsMatch(passwordValue) == false)
            return "Input password exceeds the maximum length!";

        if (Regexes.PasswordALowercaseLetter.IsMatch(passwordValue) == false)
            return "Input password doesn't have a lowercase letter!";

        if (Regexes.PasswordAnUppercaseLetter.IsMatch(passwordValue) == false)
            return "Input password doesn't have an uppercase letter!";

        if (Regexes.PasswordANumber.IsMatch(passwordValue) == false)
            return "Input password doesn't have a number!";

        if (Regexes.PasswordASpecialLetter.IsMatch(passwordValue) == false)
            return "Input password doesn't have a special character!";

        if (Regexes.PasswordValidCharacters.IsMatch(passwordValue) == false)
            return "Input password doesn't have valid characters!";

        return null;
    }

    public async Task ValidateUsernameMatchWithPasswordAsync(string username, Password password)
    {
        var user = await _userOperator.GetUserByIdentifierAsync(UserIdentifier.OfUsername(username));
        if (user == null)
            throw new UsernameNotExistException(username);

        if (string.Equals(user.Password.Hash, password.Hash) == false)
            throw new UsernameAndPasswordAuthenticationFailedException(username, password.Hash);
    }

    public void ValidateResetPasswordCodeIsCorrectAsync(User user, string resetPasswordCode)
    {
        if (resetPasswordCode != user.ResetPasswordCode)
            throw new ResetPasswordCodeIncorrectValueException(resetPasswordCode);
    }

    public void ValidateResetPasswordCodeFormat(string resetPasswordCode)
    {

    }

    public bool ValidateUserPasswordExists(User user,
        bool throwException = true)
    {
        if (user.Password != null && !string.IsNullOrWhiteSpace(user.Password.Hash))
            return true;

        if (throwException)
            throw new PasswordNotExistException(user.Id);

        return false;
    }

    public bool ValidateUserPasswordNotExist(User user,
        bool throwException = true)
    {
        if (user.Password == null || string.IsNullOrWhiteSpace(user.Password.Hash))
            return true;

        if (throwException)
            throw new PasswordAlreadyExistException(user.Id, user.Password.Hash);

        return false;
    }
}
