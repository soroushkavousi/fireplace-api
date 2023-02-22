using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Exceptions;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Identifiers;
using FireplaceApi.Domain.Interfaces;
using FireplaceApi.Domain.Models;
using FireplaceApi.Infrastructure.Converters;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace FireplaceApi.Infrastructure.Repositories
{
    public class ConfigsRepository : IConfigsRepository
    {
        private readonly ILogger<ConfigsRepository> _logger;
        private readonly FireplaceApiDbContext _dbContext;
        private readonly DbSet<ConfigsEntity> _configsEntities;
        private readonly ConfigsConverter _configsConverter;

        public ConfigsRepository(ILogger<ConfigsRepository> logger,
            FireplaceApiDbContext dbContext, ConfigsConverter configsConverter)
        {
            _logger = logger;
            _dbContext = dbContext;
            _configsEntities = dbContext.ConfigsEntities;
            _configsConverter = configsConverter;
        }

        public async Task<Configs> GetConfigsByIdentifierAsync(ConfigsIdentifier identifier)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { identifier });
            var sw = Stopwatch.StartNew();
            var configsEntity = await _configsEntities
                .AsNoTracking()
                .Search(
                    identifier: identifier
                )
                .SingleOrDefaultAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { configsEntity });
            return _configsConverter.ConvertToModel(configsEntity);
        }

        public async Task<Configs> CreateConfigsAsync(Configs configs)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT",
                parameters: new { configs });
            var sw = Stopwatch.StartNew();
            var configsEntity = _configsConverter.ConvertToEntity(configs);
            _configsEntities.Add(configsEntity);
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachAllEntries();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { configsEntity });
            return _configsConverter.ConvertToModel(configsEntity);
        }

        public async Task<Configs> UpdateConfigsAsync(Configs configs)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { configs });
            var sw = Stopwatch.StartNew();
            var configsEntity = _configsConverter.ConvertToEntity(configs);
            _configsEntities.Update(configsEntity);
            try
            {
                await _dbContext.SaveChangesAsync();
                _dbContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var serverMessage = $"Can't update the configsEntity DbUpdateConcurrencyException. {configsEntity.ToJson()}";
                throw new ApiException(ErrorName.INTERNAL_SERVER, serverMessage, systemException: ex);
            }

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { configsEntity });
            return _configsConverter.ConvertToModel(configsEntity);
        }

        public async Task<bool> DoesConfigsIdentifierExistAsync(ConfigsIdentifier identifier)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { identifier });
            var sw = Stopwatch.StartNew();
            var doesExist = await _configsEntities
                .AsNoTracking()
                .Search(
                    identifier: identifier
                )
                .AnyAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { doesExist });
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
}
