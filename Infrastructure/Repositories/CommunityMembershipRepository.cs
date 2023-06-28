using FireplaceApi.Application.Communities;
using FireplaceApi.Domain.Communities;
using FireplaceApi.Domain.Errors;
using FireplaceApi.Domain.Users;
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

public class CommunityMembershipRepository : ICommunityMembershipRepository, IRepository<ICommunityMembershipRepository>
{
    private readonly ILogger<CommunityMembershipRepository> _logger;
    private readonly ApiDbContext _dbContext;
    private readonly IIdGenerator _idGenerator;
    private readonly DbSet<CommunityMembershipEntity> _communityMembershipEntities;

    public CommunityMembershipRepository(ILogger<CommunityMembershipRepository> logger,
        ApiDbContext dbContext, IIdGenerator idGenerator)
    {
        _logger = logger;
        _dbContext = dbContext;
        _idGenerator = idGenerator;
        _communityMembershipEntities = dbContext.CommunityMembershipEntities;
    }

    public async Task<List<CommunityMembership>> SearchCommunityMembershipsAsync(
        UserIdentifier userIdentifier = null, CommunityIdentifier communityIdentifier = null,
        bool includeUser = false, bool includeCommunity = false)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT",
            parameters: new { userIdentifier, communityIdentifier });
        var sw = Stopwatch.StartNew();
        var communityMembershipEntity = await _communityMembershipEntities
            .AsNoTracking()
            .Search(
                identifier: null,
                userIdentifier: userIdentifier,
                communityIdentifier: communityIdentifier
            )
            .Include(
                userEntity: includeUser,
                communityEntity: includeCommunity
            )
            .Take(Configs.Current.QueryResult.TotalLimit)
            .ToListAsync();

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT",
            parameters: new { ids = communityMembershipEntity.Select(e => e.Id) });
        return communityMembershipEntity.Select(
            CommunityMembershipConverter.ToModel).ToList();
    }

    public async Task<CommunityMembership> GetCommunityMembershipByIdentifierAsync(
        CommunityMembershipIdentifier identifier, bool includeUser = false,
        bool includeCommunity = false)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT",
            parameters: new { identifier, includeUser, includeCommunity });
        var sw = Stopwatch.StartNew();
        var communityMembershipEntity = await _communityMembershipEntities
            .AsNoTracking()
            .Search(
                identifier: identifier,
                userIdentifier: null,
                communityIdentifier: null
            )
            .Include(
                userEntity: includeUser,
                communityEntity: includeCommunity
            )
            .SingleOrDefaultAsync();

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT",
            parameters: new { communityMembershipEntity });
        return communityMembershipEntity.ToModel();
    }

    public async Task<CommunityMembership> CreateCommunityMembershipAsync(ulong userId,
        Username username, ulong communityId, CommunityName communityName)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT",
            parameters: new { userId, username, communityId, communityName });
        var sw = Stopwatch.StartNew();
        var id = _idGenerator.GenerateNewId();
        var communityMembershipEntity = new CommunityMembershipEntity(id, userId,
            username.Value, communityId, communityName.Value);
        _communityMembershipEntities.Add(communityMembershipEntity);
        await _dbContext.SaveChangesAsync();
        _dbContext.DetachAllEntries();

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { communityMembershipEntity });
        return communityMembershipEntity.ToModel();
    }

    public async Task<CommunityMembership> UpdateCommunityMembershipAsync(
        CommunityMembership communityMembership)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new { communityMembership });
        var sw = Stopwatch.StartNew();
        var communityMembershipEntity = communityMembership.ToEntity();
        _communityMembershipEntities.Update(communityMembershipEntity);
        try
        {
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachAllEntries();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new InternalServerException("Can't update the communityMembershipEntity DbUpdateConcurrencyException!",
                parameters: communityMembershipEntity, systemException: ex);
        }

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { communityMembershipEntity });
        return communityMembershipEntity.ToModel();
    }

    public async Task DeleteCommunityMembershipByIdentifierAsync(CommunityMembershipIdentifier identifier)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new { identifier });
        var sw = Stopwatch.StartNew();
        var communityMembershipEntity = await _communityMembershipEntities
            .Search(
                identifier: identifier,
                userIdentifier: null,
                communityIdentifier: null
            )
            .SingleOrDefaultAsync();

        _communityMembershipEntities.Remove(communityMembershipEntity);
        await _dbContext.SaveChangesAsync();
        _dbContext.DetachAllEntries();

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { communityMembershipEntity });
    }

    public async Task<bool> DoesCommunityMembershipIdentifierExistAsync(CommunityMembershipIdentifier identifier)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new { identifier });
        var sw = Stopwatch.StartNew();
        var doesExist = await _communityMembershipEntities
            .AsNoTracking()
            .Search(
                identifier: identifier,
                userIdentifier: null,
                communityIdentifier: null
            )
            .AnyAsync();

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { doesExist });
        return doesExist;
    }
}

public static class CommunityMembershipRepositoryExtensions
{
    public static IQueryable<CommunityMembershipEntity> Include(
        [NotNull] this IQueryable<CommunityMembershipEntity> q,
        bool userEntity, bool communityEntity)
    {
        if (userEntity)
            q = q.Include(e => e.UserEntity);

        if (communityEntity)
            q = q.Include(e => e.CommunityEntity);

        return q;
    }

    public static IQueryable<CommunityMembershipEntity> Search(
        [NotNull] this IQueryable<CommunityMembershipEntity> q,
        CommunityMembershipIdentifier identifier,
        UserIdentifier userIdentifier, CommunityIdentifier communityIdentifier)
    {
        if (identifier != null)
        {
            switch (identifier)
            {
                case CommunityMembershipIdIdentifier idIdentifier:
                    q = q.Where(e => e.UserEntityId == idIdentifier.Id);
                    break;
                case CommunityMembershipUserAndCommunityIdentifier userAndCommunity:
                    userIdentifier = userAndCommunity.UserIdentifier;
                    communityIdentifier = userAndCommunity.CommunityIdentifier;
                    break;
            }
        }

        if (userIdentifier != null)
        {
            switch (userIdentifier)
            {
                case UserIdIdentifier idIdentifier:
                    q = q.Where(e => e.UserEntityId == idIdentifier.Id);
                    break;
                case UserUsernameIdentifier usernameIdentifier:
                    q = q.Where(e => e.UserEntityUsername == usernameIdentifier.Username.Value);
                    break;
            }
        }

        if (communityIdentifier != null)
        {
            switch (communityIdentifier)
            {
                case CommunityIdIdentifier idIdentifier:
                    q = q.Where(e => e.CommunityEntityId == idIdentifier.Id);
                    break;
                case CommunityNameIdentifier nameIdentifier:
                    q = q.Where(e => e.CommunityEntityName == nameIdentifier.Name.Value);
                    break;
            }
        }

        q = q.OrderByDescending(e => e.CreationDate);

        return q;
    }
}
