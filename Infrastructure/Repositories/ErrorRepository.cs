using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Models;
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
    public class ErrorRepository : IErrorRepository
    {
        private readonly ILogger<ErrorRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly FireplaceApiContext _fireplaceApiContext;
        private readonly DbSet<ErrorEntity> _errorEntities;
        private readonly ErrorConverter _errorConverter;

        public ErrorRepository(ILogger<ErrorRepository> logger, IConfiguration configuration,
            FireplaceApiContext fireplaceApiContext, ErrorConverter errorConverter
            )
        {
            _logger = logger;
            _configuration = configuration;
            _fireplaceApiContext = fireplaceApiContext;
            _errorEntities = fireplaceApiContext.ErrorEntities;
            _errorConverter = errorConverter;
        }

        public async Task<List<Error>> ListErrorsAsync()
        {
            _logger.LogIOInformation(null, "Database | Input", null);
            var sw = Stopwatch.StartNew();
            var errorEntites = await _errorEntities
                .AsNoTracking()
                .Include(
                )
                .OrderBy(e => e.Code)
                .ToListAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { errorEntites });
            return errorEntites.Select(e => _errorConverter.ConvertToModel(e)).ToList();
        }

        public async Task<Error> GetErrorByNameAsync(ErrorName name)
        {
            _logger.LogIOInformation(null, "Database | Input", new { name });
            var sw = Stopwatch.StartNew();
            var errorEntity = await _errorEntities
                .AsNoTracking()
                .Where(e => e.Name == name.ToString())
                .Include(
                )
                .SingleOrDefaultAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { errorEntity });
            return _errorConverter.ConvertToModel(errorEntity);
        }

        public async Task<Error> GetErrorByCodeAsync(int code)
        {
            _logger.LogIOInformation(null, "Database | Input", new { code });
            var sw = Stopwatch.StartNew();
            var errorEntity = await _errorEntities
                .AsNoTracking()
                .Where(e => e.Code == code)
                .Include(
                )
                .SingleOrDefaultAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { errorEntity });
            return _errorConverter.ConvertToModel(errorEntity);
        }

        public async Task<Error> CreateErrorAsync(ulong id, ErrorName name,
            int code, string clientMessage, int httpStatusCode)
        {
            _logger.LogIOInformation(null, "Database | Input",
                new { id, name, code, clientMessage, httpStatusCode });
            var sw = Stopwatch.StartNew();
            var errorEntity = new ErrorEntity(id, name.ToString(), code, clientMessage, httpStatusCode);
            _errorEntities.Add(errorEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database | Output", new { errorEntity });
            return _errorConverter.ConvertToModel(errorEntity);
        }

        public async Task<Error> UpdateErrorAsync(Error error)
        {
            _logger.LogIOInformation(null, "Database | Input", new { error });
            var sw = Stopwatch.StartNew();
            var errorEntity = _errorConverter.ConvertToEntity(error);
            _errorEntities.Update(errorEntity);
            try
            {
                await _fireplaceApiContext.SaveChangesAsync();
                _fireplaceApiContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var serverMessage = $"Can't update the errorEntity DbUpdateConcurrencyException. {errorEntity.ToJson()}";
                throw new ApiException(ErrorName.INTERNAL_SERVER, serverMessage, systemException: ex);
            }

            _logger.LogIOInformation(sw, "Database | Output", new { errorEntity });
            return _errorConverter.ConvertToModel(errorEntity);
        }

        public async Task DeleteErrorAsync(int code)
        {
            _logger.LogIOInformation(null, "Database | Input", new { code });
            var sw = Stopwatch.StartNew();
            var errorEntity = await _errorEntities
                .Where(e => e.Code == code)
                .SingleOrDefaultAsync();

            _errorEntities.Remove(errorEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database | Output", new { errorEntity });
        }

        public async Task<bool> DoesErrorNameExistAsync(ErrorName name)
        {
            _logger.LogIOInformation(null, "Database | Input", new { name });
            var sw = Stopwatch.StartNew();
            var doesExist = await _errorEntities
                .AsNoTracking()
                .Where(e => e.Name == name.ToString())
                .AnyAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { doesExist });
            return doesExist;
        }

        public async Task<bool> DoesErrorCodeExistAsync(int code)
        {
            _logger.LogIOInformation(null, "Database | Input", new { code });
            var sw = Stopwatch.StartNew();
            var doesExist = await _errorEntities
                .AsNoTracking()
                .Where(e => e.Code == code)
                .AnyAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { doesExist });
            return doesExist;
        }

        public async Task<bool> DoesErrorIdExistAsync(ulong id)
        {
            _logger.LogIOInformation(null, "Database | Input", new { id });
            var sw = Stopwatch.StartNew();
            var doesExist = await _errorEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .AnyAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { doesExist });
            return doesExist;
        }
    }

    public static class ErrorRepositoryExtensions
    {
        public static IQueryable<ErrorEntity> Include(
                    [NotNull] this IQueryable<ErrorEntity> errorEntitiesQuery)
        {
            return errorEntitiesQuery;
        }
    }
}
