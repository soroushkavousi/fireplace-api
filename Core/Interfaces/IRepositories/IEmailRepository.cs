using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.ValueObjects;

namespace FireplaceApi.Core.Interfaces
{
    public interface IEmailRepository
    {
        public Task<List<Email>> ListEmailsAsync(bool includeUser = false);
        public Task<Email> GetEmailByIdAsync(long id, bool includeUser = false);
        public Task<Email> GetEmailByAddressAsync(string address, bool includeUser = false);
        public Task<Email> CreateEmailAsync(long userId, string address, Activation activation);
        public Task<Email> UpdateEmailAsync(Email email);
        public Task DeleteEmailAsync(long id);
        public Task<bool> DoesEmailIdExistAsync(long id);
        public Task<bool> DoesEmailAddressExistAsync(string emailAddress);
    }
}
