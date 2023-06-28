using FireplaceApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace FireplaceApi.Infrastructure.Repositories;

public class ServerRepository : IServerRepository, IRepository<IServerRepository>
{
    private readonly ILogger<ServerRepository> _logger;
    private readonly ApiDbContext _dbContext;
    private readonly DbSet<ServerEntity> _serverEntities;

    public ServerRepository(ILogger<ServerRepository> logger, ApiDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
        _serverEntities = dbContext.ServerEntities;
    }

    public async Task<List<ulong>> ListServerIdsAsync()
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT");
        var sw = Stopwatch.StartNew();

        var serverIds = await _serverEntities
            .AsNoTracking()
            .Search(
                identifier: null
            )
            .Select(e => e.Id)
            .ToListAsync();

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT",
            parameters: new { serverIds });
        return serverIds;
    }

    public async Task<ServerEntity> GetServerAsync(ServerIdentifier identifier)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new { identifier });
        var sw = Stopwatch.StartNew();
        var serverEntity = await _serverEntities
            .AsNoTracking()
            .Search(
                identifier: identifier
            )
            .SingleOrDefaultAsync();

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { serverEntity });
        return serverEntity;
    }

    public async Task<ServerEntity> CreateServerAsync(ulong id, string macAddress)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT",
            parameters: new
            {
                macAddress,
            });
        var sw = Stopwatch.StartNew();
        var serverEntity = new ServerEntity(id, macAddress);
        _serverEntities.Add(serverEntity);
        await _dbContext.SaveChangesAsync();
        _dbContext.DetachAllEntries();

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { serverEntity });
        return serverEntity;
    }

    public async Task DeleteServerAsync(ulong id)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new { id });
        var sw = Stopwatch.StartNew();
        var serverEntity = await _serverEntities
            .Where(e => e.Id == id)
            .SingleOrDefaultAsync();

        _serverEntities.Remove(serverEntity);
        await _dbContext.SaveChangesAsync();
        _dbContext.DetachAllEntries();

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { serverEntity });
    }

    public async Task<bool> DoesServerIdExistAsync(ulong id)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new { id });
        var sw = Stopwatch.StartNew();
        var doesExist = await _serverEntities
            .AsNoTracking()
            .Where(e => e.Id == id)
            .AnyAsync();

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { doesExist });
        return doesExist;
    }
}

public static class ServerRepositoryExtensions
{
    public static IQueryable<ServerEntity> Search(
        [NotNull] this IQueryable<ServerEntity> q, ServerIdentifier identifier)
    {
        if (identifier != null)
        {
            switch (identifier)
            {
                case ServerIdIdentifier idIdentifier:
                    q = q.Where(e => e.Id == idIdentifier.Id);
                    break;
                case ServerMacAddressIdentifier macAddressIdentifier:
                    q = q.Where(e => e.MacAddress == macAddressIdentifier.MacAddress);
                    break;
            }
        }

        q = q.OrderByDescending(e => e.CreationDate);
        return q;
    }
}
