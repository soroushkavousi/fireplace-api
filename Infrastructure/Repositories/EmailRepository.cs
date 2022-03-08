using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Identifiers;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.ValueObjects;
using FireplaceApi.Infrastructure.Converters;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;
        private readonly FireplaceApiContext _fireplaceApiContext;
        private readonly DbSet<EmailEntity> _emailEntities;
        private readonly EmailConverter _emailConverter;

        public EmailRepository(ILogger<EmailRepository> logger, IConfiguration configuration,
            FireplaceApiContext fireplaceApiContext, EmailConverter emailConverter
            )
        {
            _logger = logger;
            _configuration = configuration;
            _fireplaceApiContext = fireplaceApiContext;
            _emailEntities = fireplaceApiContext.EmailEntities;
            _emailConverter = emailConverter;
        }

        public async Task<List<Email>> ListEmailsAsync(
                    bool includeUser = false)
        {
            _logger.LogAppIOInformation("Database | Input", new { includeUser });
            var sw = Stopwatch.StartNew();
            var emailEntities = await _emailEntities
                .AsNoTracking()
                .Include(
                    userEntity: includeUser
                )
                .ToListAsync();

            _logger.LogAppIOInformation(sw, "Database | Output", new { emailEntities });
            return emailEntities.Select(e => _emailConverter.ConvertToModel(e)).ToList();
        }

        public async Task<Email> GetEmailByIdentifierAsync(EmailIdentifier identifier, bool includeUser = false)
        {
            _logger.LogAppIOInformation("Database | Input", new { identifier, includeUser });
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

            _logger.LogAppIOInformation(sw, "Database | Output", new { emailEntity });
            return _emailConverter.ConvertToModel(emailEntity);
        }

        public async Task<Email> CreateEmailAsync(ulong id, ulong userId,
            string address, Activation activation)
        {
            _logger.LogAppIOInformation("Database | Input",
                new { id, userId, address, activation });
            var sw = Stopwatch.StartNew();
            var emailEntity = new EmailEntity(id, userId, address,
                activation.Status.ToString(), activationCode: activation.Code);
            _emailEntities.Add(emailEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogAppIOInformation(sw, "Database | Output", new { emailEntity });
            return _emailConverter.ConvertToModel(emailEntity);
        }

        public async Task<Email> UpdateEmailAsync(Email email)
        {
            _logger.LogAppIOInformation("Database | Input", new { email });
            var sw = Stopwatch.StartNew();
            var emailEntity = _emailConverter.ConvertToEntity(email);
            _emailEntities.Update(emailEntity);
            try
            {
                await _fireplaceApiContext.SaveChangesAsync();
                _fireplaceApiContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var serverMessage = $"Can't update the emailEntity DbUpdateConcurrencyException. {emailEntity.ToJson()}";
                throw new ApiException(ErrorName.INTERNAL_SERVER, serverMessage, systemException: ex);
            }

            _logger.LogAppIOInformation(sw, "Database | Output", new { emailEntity });
            return _emailConverter.ConvertToModel(emailEntity);
        }

        public async Task DeleteEmailAsync(EmailIdentifier identifier)
        {
            _logger.LogAppIOInformation("Database | Input", new { identifier });
            var sw = Stopwatch.StartNew();
            var emailEntity = await _emailEntities
                .Search(
                    identifier: identifier
                )
                .SingleOrDefaultAsync();

            _emailEntities.Remove(emailEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogAppIOInformation(sw, "Database | Output", new { emailEntity });
        }

        public async Task<bool> DoesEmailIdentifierExistAsync(EmailIdentifier identifier)
        {
            _logger.LogAppIOInformation("Database | Input", new { identifier });
            var sw = Stopwatch.StartNew();
            var doesExist = await _emailEntities
                .AsNoTracking()
                .Search(
                    identifier: identifier
                )
                .AnyAsync();

            _logger.LogAppIOInformation(sw, "Database | Output", new { doesExist });
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
