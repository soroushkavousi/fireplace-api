using FireplaceApi.Domain.Emails;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Emails;

public class EmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly EmailValidator _emailValidator;
    private readonly EmailOperator _emailOperator;

    public EmailService(ILogger<EmailService> logger, EmailValidator emailValidator, EmailOperator emailOperator)
    {
        _logger = logger;
        _emailValidator = emailValidator;
        _emailOperator = emailOperator;
    }

    public async Task<Email> ActivateRequestingUserEmailAsync(ulong userId, int activationCode)
    {
        await _emailValidator.ValidateActivateRequestingUserEmailInputParametersAsync(userId,
            activationCode);
        var email = await _emailOperator.ActivateEmailByIdentifierAsync(_emailValidator.EmailIdentifier);
        return email;
    }

    public async Task ResendActivationCodeAsync(ulong userId)
    {
        await _emailValidator.ValidateResendActivationCodeInputParametersAsync(userId);
        await _emailOperator.ResendActivationCodeAsync(_emailValidator.Email);
    }

    public async Task<Email> GetRequestingUserEmailAsync(ulong userId)
    {
        await _emailValidator.ValidateGetRequestingUserEmailInputParametersAsync(userId);
        var email = await _emailOperator.GetEmailByIdentifierAsync(
            _emailValidator.EmailIdentifier);
        return email;
    }

    public async Task<Email> PatchEmailAsync(ulong userId, string newAddress)
    {
        await _emailValidator.ValidatePatchEmailInputParametersAsync(userId, newAddress);
        var email = await _emailOperator.PatchEmailByIdentifierAsync(
            _emailValidator.EmailIdentifier, address: newAddress);
        return email;
    }
}
