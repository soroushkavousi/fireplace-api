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

public class CommunityRepository : ICommunityRepository, IRepository<ICommunityRepository>
{
    private readonly ILogger<CommunityRepository> _logger;
    private readonly ApiDbContext _dbContext;
    private readonly IIdGenerator _idGenerator;
    private readonly DbSet<CommunityEntity> _communityEntities;

    public CommunityRepository(ILogger<CommunityRepository> logger, ApiDbContext dbContext,
        IIdGenerator idGenerator)
    {
        _logger = logger;
        _dbContext = dbContext;
        _idGenerator = idGenerator;
        _communityEntities = dbContext.CommunityEntities;
    }

    public async Task<List<Community>> ListCommunitiesAsync(string search, CommunitySortType sort)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new { search });
        var sw = Stopwatch.StartNew();
        var communityEntities = await _communityEntities
            .AsNoTracking()
            .Search(
                identifier: null,
                search: search
            )
            .Sort(sort)
            .Take(Configs.Current.QueryResult.TotalLimit)
            .ToListAsync();

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT",
            parameters: new { communityEntities = communityEntities.Select(e => e.Id) });
        return communityEntities.Select(CommunityConverter.ToModel).ToList();
    }

    public async Task<List<Community>> ListCommunitiesByIdsAsync(List<ulong> Ids)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new { Ids });
        var sw = Stopwatch.StartNew();
        var communityEntities = await _communityEntities
            .AsNoTracking()
            .Where(e => Ids.Contains(e.Id))
            .ToListAsync();

        var communityEntityDictionary = new Dictionary<ulong, CommunityEntity>();
        communityEntities.ForEach(e => communityEntityDictionary[e.Id] = e);
        communityEntities = new List<CommunityEntity>();
        Ids.ForEach(id => communityEntities.Add(communityEntityDictionary[id]));

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT",
            parameters: new { communityEntities = communityEntities.Select(e => e.Id) });
        return communityEntities.Select(CommunityConverter.ToModel).ToList();
    }

    public async Task<Community> GetCommunityAsync(CommunityIdentifier identifier,
        bool includeCreator = false)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new { identifier, includeCreator });
        var sw = Stopwatch.StartNew();
        var communityEntity = await _communityEntities
            .AsNoTracking()
            .Search(
                identifier: identifier,
                search: null
            )
            .Include(
                creatorEntity: includeCreator
            )
            .SingleOrDefaultAsync();

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { communityEntity });
        return communityEntity.ToModel();
    }

    public async Task<CommunityName> GetNameByIdAsync(ulong id)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new { id });
        var sw = Stopwatch.StartNew();
        var communityNameValue = (await _communityEntities
            .AsNoTracking()
            .Select(e => new { e.Id, e.Name })
            .SingleAsync(e => e.Id == id))
            .Name;

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { communityNameValue });
        return new CommunityName(communityNameValue);
    }

    public async Task<ulong> GetIdByNameAsync(CommunityName communityName)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new { communityName });
        var sw = Stopwatch.StartNew();
        var communityId = (await _communityEntities
            .AsNoTracking()
            .Select(e => new { e.Id, e.Name })
            .SingleAsync(e => string.Equals(e.Name, communityName.Value)))
            .Id;

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { communityId });
        return communityId;
    }

    public async Task<Community> CreateCommunityAsync(CommunityName name,
        ulong creatorId, Username creatorUsername)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT",
            parameters: new { name, creatorId, creatorUsername });
        var sw = Stopwatch.StartNew();
        var id = _idGenerator.GenerateNewId();
        var communityEntity = new CommunityEntity(id, name.Value, creatorId, creatorUsername.Value);
        _communityEntities.Add(communityEntity);
        await _dbContext.SaveChangesAsync();
        _dbContext.DetachAllEntries();

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { communityEntity });
        return communityEntity.ToModel();
    }

    public async Task<Community> UpdateCommunityAsync(Community community)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new { community });
        var sw = Stopwatch.StartNew();
        var communityEntity = community.ToEntity();
        _communityEntities.Update(communityEntity);
        try
        {
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachAllEntries();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new InternalServerException("Can't update the communityEntity DbUpdateConcurrencyException!",
                parameters: communityEntity, systemException: ex);
        }

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { communityEntity });
        return communityEntity.ToModel();
    }

    public async Task UpdateCommunityNameAsync(ulong id, CommunityName newCommunityName)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new { id, newCommunityName });
        var sw = Stopwatch.StartNew();
        int rowAffectedCount = 0;
        try
        {
            rowAffectedCount = await _dbContext.Database.ExecuteSqlInterpolatedAsync(
                $"CALL public.\"UpdateCommunityName\"({id}, {newCommunityName});");
            _dbContext.DetachAllEntries();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new InternalServerException("Can't update the community search DbUpdateConcurrencyException!",
                parameters: new { id, newCommunityName, rowAffectedCount }, systemException: ex);
        }

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { rowAffectedCount });
    }

    public async Task DeleteCommunityAsync(CommunityIdentifier identifier)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new { identifier });
        var sw = Stopwatch.StartNew();
        var communityEntity = await _communityEntities
            .Search(
                identifier: identifier,
                search: null
            )
            .SingleOrDefaultAsync();

        _communityEntities.Remove(communityEntity);
        await _dbContext.SaveChangesAsync();
        _dbContext.DetachAllEntries();

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { communityEntity });
    }

    public async Task<bool> DoesCommunityExistAsync(CommunityIdentifier identifier)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new { identifier });
        var sw = Stopwatch.StartNew();
        var doesExist = await _communityEntities
            .AsNoTracking()
            .Search(
                identifier: identifier,
                search: null
            )
            .AnyAsync();

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { doesExist });
        return doesExist;
    }

    public async Task<bool> DoesCommunityNameExistAsync(CommunityName name)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new { name });
        var sw = Stopwatch.StartNew();
        var doesExist = await _communityEntities
            .AsNoTracking()
            .Where(e => e.Name == name.Value)
            .AnyAsync();

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { doesExist });
        return doesExist;
    }
}

public static class CommunityRepositoryExtensions
{
    public static IQueryable<CommunityEntity> Include(
        [NotNull] this IQueryable<CommunityEntity> communityEntitiesQuery,
        bool creatorEntity)
    {
        if (creatorEntity)
            communityEntitiesQuery = communityEntitiesQuery.Include(e => e.CreatorEntity);

        return communityEntitiesQuery;
    }

    public static IQueryable<CommunityEntity> Search(
        [NotNull] this IQueryable<CommunityEntity> q, CommunityIdentifier identifier,
        string search)
    {
        if (identifier != null)
        {
            switch (identifier)
            {
                case CommunityIdIdentifier idIdentifier:
                    q = q.Where(e => e.Id == idIdentifier.Id);
                    break;
                case CommunityNameIdentifier nameIdentifier:
                    q = q.Where(e => e.Name == nameIdentifier.Name.Value);
                    break;
            }
        }

        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(e => EF.Functions
                .ILike(EF.Functions.Collate(e.Name, "default"), $"%{search}%"));

        return q;
    }

    public static IQueryable<CommunityEntity> Sort(
        [NotNull] this IQueryable<CommunityEntity> q, CommunitySortType sort)
    {
        q = sort switch
        {
            CommunitySortType.NEW => q.OrderByDescending(e => e.CreationDate),
            CommunitySortType.CREATION => q.OrderBy(e => e.CreationDate),
            _ => q.OrderByDescending(e => e.CreationDate),
        };

        return q;
    }
}
