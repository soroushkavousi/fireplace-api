﻿using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Identifiers;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.ValueObjects;
using FireplaceApi.Infrastructure.Converters;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace FireplaceApi.Infrastructure.Repositories
{
    public class EmailRepository : IEmailRepository
    {
        private readonly ILogger<EmailRepository> _logger;
        private readonly FireplaceApiDbContext _dbContext;
        private readonly DbSet<EmailEntity> _emailEntities;
        private readonly EmailConverter _emailConverter;

        public EmailRepository(ILogger<EmailRepository> logger,
            FireplaceApiDbContext dbContext, EmailConverter emailConverter)
        {
            _logger = logger;
            _dbContext = dbContext;
            _emailEntities = dbContext.EmailEntities;
            _emailConverter = emailConverter;
        }

        public async Task<List<Email>> ListEmailsAsync(
                    bool includeUser = false)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { includeUser });
            var sw = Stopwatch.StartNew();
            var emailEntities = await _emailEntities
                .AsNoTracking()
                .Include(
                    userEntity: includeUser
                )
                .ToListAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT",
                parameters: new { emailEntities = emailEntities.Select(e => e.Id) });
            return emailEntities.Select(_emailConverter.ConvertToModel).ToList();
        }

        public async Task<Email> GetEmailByIdentifierAsync(EmailIdentifier identifier, bool includeUser = false)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { identifier, includeUser });
            var sw = Stopwatch.StartNew();
            var emailEntity = await _emailEntities
                .AsNoTracking()
                .Search(
                    identifier: identifier
                )
                .Include(
                    userEntity: includeUser
                )
                .SingleOrDefaultAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { emailEntity });
            return _emailConverter.ConvertToModel(emailEntity);
        }

        public async Task<Email> CreateEmailAsync(ulong id, ulong userId,
            string address, Activation activation)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT",
                parameters: new { id, userId, address, activation });
            var sw = Stopwatch.StartNew();
            var emailEntity = new EmailEntity(id, userId, address,
                activation.Status.ToString(), activationCode: activation.Code);
            _emailEntities.Add(emailEntity);
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachAllEntries();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { emailEntity });
            return _emailConverter.ConvertToModel(emailEntity);
        }

        public async Task<Email> UpdateEmailAsync(Email email)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { email });
            var sw = Stopwatch.StartNew();
            var emailEntity = _emailConverter.ConvertToEntity(email);
            _emailEntities.Update(emailEntity);
            try
            {
                await _dbContext.SaveChangesAsync();
                _dbContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var serverMessage = $"Can't update the emailEntity DbUpdateConcurrencyException. {emailEntity.ToJson()}";
                throw new ApiException(ErrorName.INTERNAL_SERVER, serverMessage, systemException: ex);
            }

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { emailEntity });
            return _emailConverter.ConvertToModel(emailEntity);
        }

        public async Task DeleteEmailAsync(EmailIdentifier identifier)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { identifier });
            var sw = Stopwatch.StartNew();
            var emailEntity = await _emailEntities
                .Search(
                    identifier: identifier
                )
                .SingleOrDefaultAsync();

            _emailEntities.Remove(emailEntity);
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachAllEntries();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { emailEntity });
        }

        public async Task<bool> DoesEmailIdentifierExistAsync(EmailIdentifier identifier)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { identifier });
            var sw = Stopwatch.StartNew();
            var doesExist = await _emailEntities
                .AsNoTracking()
                .Search(
                    identifier: identifier
                )
                .AnyAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { doesExist });
            return doesExist;
        }
    }

    public static class EmailRepositoryExtensions
    {
        public static IQueryable<EmailEntity> Include(
            [NotNull] this IQueryable<EmailEntity> emailEntitiesQuery,
            bool userEntity)
        {
            if (userEntity)
                emailEntitiesQuery = emailEntitiesQuery.Include(e => e.UserEntity);

            return emailEntitiesQuery;
        }

        public static IQueryable<EmailEntity> Search(
            [NotNull] this IQueryable<EmailEntity> q, EmailIdentifier identifier)
        {
            if (identifier != null)
            {
                switch (identifier)
                {
                    case EmailIdIdentifier idIdentifier:
                        q = q.Where(e => e.Id == idIdentifier.Id);
                        break;
                    case EmailAddressIdentifier addressIdentifier:
                        q = q.Where(e => e.Address == addressIdentifier.Address);
                        break;
                    case EmailUserIdIdentifier userIdIdentifier:
                        q = q.Where(e => e.UserEntityId == userIdIdentifier.UserId);
                        break;
                }
            }

            return q;
        }

        public static EmailEntity RemoveLoopReferencing([NotNull] this EmailEntity emailEntity)
        {
            if (emailEntity.UserEntity != null)
                emailEntity.UserEntity.EmailEntity = null;

            return emailEntity;
        }
    }
}
