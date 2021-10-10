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
            var sw = Stopwatch.StartNew();
            var errorEntites = await _errorEntities
                .AsNoTracking()
                .Include(
                )
                .OrderBy(e => e.Code)
                .ToListAsync();

            _logger.LogIOInformation(sw, "Database", null, new { errorEntites });
            return errorEntites.Select(e => _errorConverter.ConvertToModel(e)).ToList();
        }

        public async Task<Error> GetErrorByNameAsync(ErrorName name)
        {
            var sw = Stopwatch.StartNew();
            var errorEntity = await _errorEntities
                .AsNoTracking()
                .Where(e => e.Name == name.ToString())
                .Include(
                )
                .SingleOrDefaultAsync();

            _logger.LogIOInformation(sw, "Database", new { name }, new { errorEntity });
            return _errorConverter.ConvertToModel(errorEntity);
        }

        public async Task<Error> GetErrorByCodeAsync(int code)
        {
            var sw = Stopwatch.StartNew();
            var errorEntity = await _errorEntities
                .AsNoTracking()
                .Where(e => e.Code == code)
                .Include(
                )
                .SingleOrDefaultAsync();

            _logger.LogIOInformation(sw, "Database", new { code }, new { errorEntity });
            return _errorConverter.ConvertToModel(errorEntity);
        }

        public async Task<Error> CreateErrorAsync(ErrorName name, int code, string clientMessage,
            int httpStatusCode)
        {
            var sw = Stopwatch.StartNew();
            var errorEntity = new ErrorEntity(name.ToString(), code, clientMessage, httpStatusCode);
            _errorEntities.Add(errorEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database", new { name, code, clientMessage, httpStatusCode }, new { errorEntity });
            return _errorConverter.ConvertToModel(errorEntity);
        }

        public async Task<Error> UpdateErrorAsync(Error error)
        {
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

            _logger.LogIOInformation(sw, "Database", new { error }, new { errorEntity });
            return _errorConverter.ConvertToModel(errorEntity);
        }

        public async Task DeleteErrorAsync(int code)
        {
            var sw = Stopwatch.StartNew();
            var errorEntity = await _errorEntities
                .Where(e => e.Code == code)
                .SingleOrDefaultAsync();

            _errorEntities.Remove(errorEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database", new { code }, new { errorEntity });
        }

        public async Task<bool> DoesErrorNameExistAsync(ErrorName name)
        {
            var sw = Stopwatch.StartNew();
            var doesExist = await _errorEntities
                .AsNoTracking()
                .Where(e => e.Name == name.ToString())
                .AnyAsync();

            _logger.LogIOInformation(sw, "Database", new { name }, new { doesExist });
            return doesExist;
        }

        public async Task<bool> DoesErrorCodeExistAsync(int code)
        {
            var sw = Stopwatch.StartNew();
            var doesExist = await _errorEntities
                .AsNoTracking()
                .Where(e => e.Code == code)
                .AnyAsync();

            _logger.LogIOInformation(sw, "Database", new { code }, new { doesExist });
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
