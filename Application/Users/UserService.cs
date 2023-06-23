using FireplaceApi.Domain.Users;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Users;

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
        Username username, Password password)
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

    public async Task<User> LogInWithUsernameAsync(IPAddress ipAddress, Username username, Password password)
    {
        await _userValidator.ValidateLogInWithUsernameInputParametersAsync(ipAddress, username, password);
        var user = await _userOperator.LogInWithUsernameAsync(ipAddress, username, password);
        return user;
    }

    public async Task<User> GetRequestingUserAsync(ulong userId,
        bool? includeEmail, bool? includeSessions)
    {
        await _userValidator.ValidateGetRequestingUserInputParametersAsync(userId,
            includeEmail, includeSessions);
        var user = await _userOperator.GetUserByIdentifierAsync(UserIdentifier.OfId(userId),
            includeEmail.Value, false, includeSessions.Value);
        return user;
    }

    public async Task<Profile> GetUserProfileAsync(ulong userId, UserIdentifier identifier)
    {
        await _userValidator.ValidateGetUserProfileInputParametersAsync(
            userId, identifier);
        var user = await _userOperator.GetUserByIdentifierAsync(identifier,
            false, false, false);
        var profile = new Profile(user);
        return profile;
    }

    public async Task CreateRequestingUserPasswordAsync(ulong userId, Password password)
    {
        await _userValidator.ValidateCreateRequestingUserPasswordInputParametersAsync(userId,
            password);
        await _userOperator.PatchRequestingUserPasswordAsync(
            _userValidator.User, password);
    }

    public async Task SendResetPasswordCodeAsync(string emailAddress, string resetPasswordWithCodeUrlFormat)
    {
        await _userValidator.ValidateSendResetPasswordCodeInputParametersAsync(emailAddress, resetPasswordWithCodeUrlFormat);
        await _userOperator.SendResetPasswordCode(emailAddress, resetPasswordWithCodeUrlFormat);
    }

    public async Task ResetPasswordWithCodeAsync(string emailAddress, string resetPasswordCode, Password newPassword)
    {
        await _userValidator.ValidateResetPasswordWithCodeInputParametersAsync(emailAddress, resetPasswordCode, newPassword);
        await _userOperator.PatchRequestingUserPasswordAsync(_userValidator.User, newPassword);
    }

    public async Task<User> PatchRequestingUserAsync(ulong userId, string displayName,
        string about, string avatarUrl, string bannerUrl, Username username)
    {
        await _userValidator.ValidatePatchUserInputParametersAsync(userId, displayName,
            about, avatarUrl, bannerUrl, username);
        var user = await _userOperator.PatchUserByIdentifierAsync(UserIdentifier.OfId(userId),
            displayName, about, avatarUrl, bannerUrl, username);
        return user;
    }

    public async Task PatchRequestingUserPasswordAsync(ulong userId, Password password, Password newPassword)
    {
        await _userValidator.ValidatePatchRequestingUserPasswordInputParametersAsync(userId,
            password, newPassword);
        await _userOperator.PatchRequestingUserPasswordAsync(
            _userValidator.User, newPassword);
    }

    public async Task DeleteRequestingUserAsync(ulong userId)
    {
        await _userValidator.ValidateDeleteUserInputParametersAsync(userId);
        await _userOperator.DeleteUserByIdentifierAsync(_userValidator.UserIdentifier);
    }
}
