using FireplaceApi.Core.Models;
using FireplaceApi.Core.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Interfaces
{
    public interface IEmailRepository
    {
        public Task<List<Email>> ListEmailsAsync(bool includeUser = false);
        public Task<Email> GetEmailByIdAsync(ulong id, bool includeUser = false);
        public Task<Email> GetEmailByAddressAsync(string address, bool includeUser = false);
        public Task<Email> CreateEmailAsync(ulong id, ulong userId, string address,
            Activation activation);
        public Task<Email> UpdateEmailAsync(Email email);
        public Task DeleteEmailAsync(ulong id);
        public Task<bool> DoesEmailIdExistAsync(ulong id);
        public Task<bool> DoesEmailAddressExistAsync(string emailAddress);
    }
}
