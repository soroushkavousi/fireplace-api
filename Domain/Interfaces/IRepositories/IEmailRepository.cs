﻿using FireplaceApi.Domain.Identifiers;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Interfaces;

public interface IEmailRepository
{
    public Task<List<Email>> ListEmailsAsync(bool includeUser = false);
    public Task<Email> GetEmailByIdentifierAsync(EmailIdentifier identifier, bool includeUser = false);
    public Task<Email> CreateEmailAsync(ulong id, ulong userId, string address,
        Activation activation);
    public Task<Email> UpdateEmailAsync(Email email);
    public Task DeleteEmailAsync(EmailIdentifier identifier);
    public Task<bool> DoesEmailIdentifierExistAsync(EmailIdentifier identifier);
}
