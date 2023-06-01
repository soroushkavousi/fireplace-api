using FireplaceApi.Domain.Exceptions;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Identifiers;
using FireplaceApi.Domain.Interfaces;
using FireplaceApi.Domain.Models;
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

public class CommunityMembershipRepository : ICommunityMembershipRepository
{
    private readonly ILogger<CommunityMembershipRepository> _logger;
    private readonly ProjectDbContext _dbContext;
    private readonly DbSet<CommunityMembershipEntity> _communityMembershipEntities;
    private readonly CommunityMembershipConverter _communityMembershipConverter;

    public CommunityMembershipRepository(ILogger<CommunityMembershipRepository> logger,
        ProjectDbContext dbContext, CommunityMembershipConverter communityMembershipConverter)
    {
        _logger = logger;
        _dbContext = dbContext;
        _communityMembershipEntities = dbContext.CommunityMembershipEntities;
        _communityMembershipConverter = communityMembershipConverter;
    }

    public async Task<List<CommunityMembership>> SearchCommunityMembershipsAsync(
        UserIdentifier userIdentifier = null, CommunityIdentifier communityIdentifier = null,
        bool includeUser = false, bool includeCommunity = false)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT",
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

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT",
            parameters: new { ids = communityMembershipEntity.Select(e => e.Id) });
        return communityMembershipEntity.Select(
            _communityMembershipConverter.ConvertToModel).ToList();
    }

    public async Task<CommunityMembership> GetCommunityMembershipByIdentifierAsync(
        CommunityMembershipIdentifier identifier, bool includeUser = false,
        bool includeCommunity = false)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT",
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

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT",
            parameters: new { communityMembershipEntity });
        return _communityMembershipConverter.ConvertToModel(communityMembershipEntity);
    }

    public async Task<CommunityMembership> CreateCommunityMembershipAsync(ulong id,
        ulong userId, string username, ulong communityId, string communityName)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT",
            parameters: new { id, userId, username, communityId, communityName });
        var sw = Stopwatch.StartNew();
        var communityMembershipEntity = new CommunityMembershipEntity(id, userId,
            username, communityId, communityName);
        _communityMembershipEntities.Add(communityMembershipEntity);
        await _dbContext.SaveChangesAsync();
        _dbContext.DetachAllEntries();

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { communityMembershipEntity });
        return _communityMembershipConverter.ConvertToModel(communityMembershipEntity);
    }

    public async Task<CommunityMembership> UpdateCommunityMembershipAsync(
        CommunityMembership communityMembership)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { communityMembership });
        var sw = Stopwatch.StartNew();
        var communityMembershipEntity = _communityMembershipConverter.ConvertToEntity(communityMembership);
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

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { communityMembershipEntity });
        return _communityMembershipConverter.ConvertToModel(communityMembershipEntity);
    }

    public async Task DeleteCommunityMembershipByIdentifierAsync(CommunityMembershipIdentifier identifier)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { identifier });
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

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { communityMembershipEntity });
    }


    public async Task<bool> DoesCommunityMembershipIdentifierExistAsync(CommunityMembershipIdentifier identifier)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { identifier });
        var sw = Stopwatch.StartNew();
        var doesExist = await _communityMembershipEntities
            .AsNoTracking()
            .Search(
                identifier: identifier,
                userIdentifier: null,
                communityIdentifier: null
            )
            .AnyAsync();

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { doesExist });
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
                    q = q.Where(e => e.UserEntityUsername == usernameIdentifier.Username);
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
                    q = q.Where(e => e.CommunityEntityName == nameIdentifier.Name);
                    break;
            }
        }

        q = q.OrderByDescending(e => e.CreationDate);

        return q;
    }
}
