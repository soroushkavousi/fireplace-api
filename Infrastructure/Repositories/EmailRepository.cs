using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GamingCommunityApi.Infrastructure.Converters;
using GamingCommunityApi.Infrastructure.Entities;
using GamingCommunityApi.Infrastructure.Entities.UserInformationEntities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GamingCommunityApi.Core.Models.UserInformations;
using GamingCommunityApi.Core.Exceptions;
using GamingCommunityApi.Core.Enums;
using GamingCommunityApi.Core.Extensions;
using GamingCommunityApi.Core.Interfaces.IRepositories;
using GamingCommunityApi.Core.ValueObjects;

namespace GamingCommunityApi.Infrastructure.Repositories
{
    public class EmailRepository : IEmailRepository
    {
        private readonly ILogger<EmailRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly GamingCommunityApiContext _gamingCommunityApiContext;
        private readonly DbSet<EmailEntity> _emailEntities;
        private readonly EmailConverter _emailConverter;

        public EmailRepository(ILogger<EmailRepository> logger, IConfiguration configuration, 
            GamingCommunityApiContext gamingCommunityApiContext, EmailConverter emailConverter
            )
        {
            _logger = logger;
            _configuration = configuration;
            _gamingCommunityApiContext = gamingCommunityApiContext;
            _emailEntities = gamingCommunityApiContext.EmailEntities;
            _emailConverter = emailConverter;
        }

        public async Task<List<Email>> ListEmailsAsync(
                    bool includeUser = false)
        {
            var emailEntities = await _emailEntities
                .AsNoTracking()
                .Include(
                    userEntity: includeUser
                )
                .ToListAsync();

            return emailEntities.Select(e => _emailConverter.ConvertToModel(e)).ToList();
        }

        public async Task<Email> GetEmailByIdAsync(long id, bool includeUser = false)
        {
            var emailEntity = await _emailEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Include(
                    userEntity: includeUser
                )
                .SingleOrDefaultAsync();

            return _emailConverter.ConvertToModel(emailEntity);
        }

        public async Task<Email> GetEmailByAddressAsync(string address, bool includeUser = false)
        {
            var emailEntity = await _emailEntities
                .AsNoTracking()
                .Where(e => e.Address == address)
                .Include(
                    userEntity: includeUser
                )
                .SingleOrDefaultAsync();

            return _emailConverter.ConvertToModel(emailEntity);
        }

        public async Task<Email> CreateEmailAsync(long userId, string address,
            Activation activation)
        {
            var emailEntity = new EmailEntity(userId, address,
                activation.Status.ToString(), activation.Code);
            _emailEntities.Add(emailEntity);
            await _gamingCommunityApiContext.SaveChangesAsync();
            _gamingCommunityApiContext.DetachAllEntries();
            return _emailConverter.ConvertToModel(emailEntity);
        }

        public async Task<Email> UpdateEmailAsync(Email email)
        {
            var emailEntity = _emailConverter.ConvertToEntity(email);
            _emailEntities.Update(emailEntity);
            try
            {
                await _gamingCommunityApiContext.SaveChangesAsync();
                _gamingCommunityApiContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var serverMessage = $"Can't update the emailEntity DbUpdateConcurrencyException. {emailEntity.ToJson()}";
                throw new ApiException(ErrorName.INTERNAL_SERVER, serverMessage, systemException: ex);
            }
            return _emailConverter.ConvertToModel(emailEntity);
        }

        public async Task DeleteEmailAsync(long id)
        {
            var emailEntity = await _emailEntities
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync();

            _emailEntities.Remove(emailEntity);
            await _gamingCommunityApiContext.SaveChangesAsync();
            _gamingCommunityApiContext.DetachAllEntries();
        }

        public async Task<bool> DoesEmailIdExistAsync(long id)
        {
            return await _emailEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .AnyAsync();
        }

        public async Task<bool> DoesEmailAddressExistAsync(string emailAddress)
        {
            return await _emailEntities
                .AsNoTracking()
                .Where(e => e.Address == emailAddress)
                .AnyAsync();
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

        public static EmailEntity RemoveLoopReferencing([NotNull] this EmailEntity emailEntity)
        {
            if (emailEntity.UserEntity != null)
                emailEntity.UserEntity.EmailEntity = null;

            return emailEntity;
        }
    }
}
