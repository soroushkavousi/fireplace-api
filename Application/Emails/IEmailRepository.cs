using FireplaceApi.Domain.Emails;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Emails;

public interface IEmailRepository
{
    public Task<List<Email>> ListEmailsAsync(bool includeUser = false);
    public Task<Email> GetEmailByIdentifierAsync(EmailIdentifier identifier, bool includeUser = false);
    public Task<Email> CreateEmailAsync(ulong userId, string address,
        Activation activation);
    public Task<Email> UpdateEmailAsync(Email email);
    public Task DeleteEmailAsync(EmailIdentifier identifier);
    public Task<bool> DoesEmailIdentifierExistAsync(EmailIdentifier identifier);
}
