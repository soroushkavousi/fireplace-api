using FireplaceApi.Application.Configurations;
using FireplaceApi.Domain.Errors;
using FireplaceApi.Infrastructure.Converters;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace FireplaceApi.Infrastructure.Repositories;

public class ConfigsRepository : IConfigsRepository, IRepository<IConfigsRepository>
{
    private readonly ILogger<ConfigsRepository> _logger;
    private readonly ApiDbContext _dbContext;
    private readonly IIdGenerator _idGenerator;
    private readonly DbSet<ConfigsEntity> _configsEntities;

    public ConfigsRepository(ILogger<ConfigsRepository> logger, ApiDbContext dbContext,
        IIdGenerator idGenerator)
    {
        _logger = logger;
        _dbContext = dbContext;
        _idGenerator = idGenerator;
        _configsEntities = dbContext.ConfigsEntities;
    }

    public async Task<Configs> GetConfigsByIdentifierAsync(ConfigsIdentifier identifier)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new { identifier });
        var sw = Stopwatch.StartNew();
        var configsEntity = await _configsEntities
            .AsNoTracking()
            .Search(
                identifier: identifier
            )
            .SingleOrDefaultAsync();

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { configsEntity });
        return configsEntity.ToModel();
    }

    public async Task<Configs> CreateConfigsAsync(Configs configs)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT",
            parameters: new { configs });
        var sw = Stopwatch.StartNew();
        var id = _idGenerator.GenerateNewId();
        var configsEntity = configs.ToEntity();
        _configsEntities.Add(configsEntity);
        await _dbContext.SaveChangesAsync();
        _dbContext.DetachAllEntries();

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { configsEntity });
        return configsEntity.ToModel();
    }

    public async Task<Configs> UpdateConfigsAsync(Configs configs)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new { configs });
        var sw = Stopwatch.StartNew();
        var configsEntity = configs.ToEntity();
        _configsEntities.Update(configsEntity);
        try
        {
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachAllEntries();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new InternalServerException("Can't update the configsEntity DbUpdateConcurrencyException!",
                parameters: configsEntity, systemException: ex);
        }

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { configsEntity });
        return configsEntity.ToModel();
    }

    public async Task<bool> DoesConfigsIdentifierExistAsync(ConfigsIdentifier identifier)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new { identifier });
        var sw = Stopwatch.StartNew();
        var doesExist = await _configsEntities
            .AsNoTracking()
            .Search(
                identifier: identifier
            )
            .AnyAsync();

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { doesExist });
        return doesExist;
    }
}

public static class ConfigsRepositoryExtensions
{
    public static IQueryable<ConfigsEntity> Search(
        [NotNull] this IQueryable<ConfigsEntity> q, ConfigsIdentifier identifier)
    {
        if (identifier != null)
        {
            switch (identifier)
            {
                case ConfigsIdIdentifier idIdentifier:
                    q = q.Where(e => e.Id == idIdentifier.Id);
                    break;
                case ConfigsEnvironmentNameIdentifier nameIdentifier:
                    q = q.Where(e => e.EnvironmentName == nameIdentifier.EnvironmentName.ToString());
                    break;
            }
        }

        return q;
    }
}
