using FireplaceApi.Application.Emails;
using FireplaceApi.Domain.Emails;
using FireplaceApi.Domain.Errors;
using FireplaceApi.Infrastructure.Converters;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace FireplaceApi.Infrastructure.Repositories;

public class EmailRepository : IEmailRepository, IRepository<IEmailRepository>
{
    private readonly ILogger<EmailRepository> _logger;
    private readonly ApiDbContext _dbContext;
    private readonly IIdGenerator _idGenerator;
    private readonly DbSet<EmailEntity> _emailEntities;

    public EmailRepository(ILogger<EmailRepository> logger, ApiDbContext dbContext,
        IIdGenerator idGenerator)
    {
        _logger = logger;
        _dbContext = dbContext;
        _idGenerator = idGenerator;
        _emailEntities = dbContext.EmailEntities;
    }

    public async Task<List<Email>> ListEmailsAsync(
                bool includeUser = false)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new { includeUser });
        var sw = Stopwatch.StartNew();
        var emailEntities = await _emailEntities
            .AsNoTracking()
            .Include(
                userEntity: includeUser
            )
            .ToListAsync();

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT",
            parameters: new { emailEntities = emailEntities.Select(e => e.Id) });
        return emailEntities.Select(EmailConverter.ToModel).ToList();
    }

    public async Task<Email> GetEmailByIdentifierAsync(EmailIdentifier identifier, bool includeUser = false)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new { identifier, includeUser });
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

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { emailEntity });
        return EmailConverter.ToModel(emailEntity);
    }

    public async Task<Email> CreateEmailAsync(ulong userId,
        string address, Activation activation)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT",
            parameters: new { userId, address, activation });
        var sw = Stopwatch.StartNew();
        var id = _idGenerator.GenerateNewId();
        var emailEntity = new EmailEntity(id, userId, address,
            activation.Status.ToString(), activationCode: activation.Code);
        _emailEntities.Add(emailEntity);
        await _dbContext.SaveChangesAsync();
        _dbContext.DetachAllEntries();

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { emailEntity });
        return EmailConverter.ToModel(emailEntity);
    }

    public async Task<Email> UpdateEmailAsync(Email email)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new { email });
        var sw = Stopwatch.StartNew();
        var emailEntity = email.ToEntity();
        _emailEntities.Update(emailEntity);
        try
        {
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachAllEntries();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new InternalServerException("Can't update the emailEntity DbUpdateConcurrencyException!",
                parameters: emailEntity, systemException: ex);
        }

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { emailEntity });
        return emailEntity.ToModel();
    }

    public async Task DeleteEmailAsync(EmailIdentifier identifier)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new { identifier });
        var sw = Stopwatch.StartNew();
        var emailEntity = await _emailEntities
            .Search(
                identifier: identifier
            )
            .SingleOrDefaultAsync();

        _emailEntities.Remove(emailEntity);
        await _dbContext.SaveChangesAsync();
        _dbContext.DetachAllEntries();

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { emailEntity });
    }

    public async Task<bool> DoesEmailIdentifierExistAsync(EmailIdentifier identifier)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new { identifier });
        var sw = Stopwatch.StartNew();
        var doesExist = await _emailEntities
            .AsNoTracking()
            .Search(
                identifier: identifier
            )
            .AnyAsync();

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { doesExist });
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
}
