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
        private readonly FireplaceApiDbContext _dbContext;
        private readonly DbSet<ErrorEntity> _errorEntities;
        private readonly ErrorConverter _errorConverter;
        private static readonly Dictionary<string, Error> _cache = new();

        public ErrorRepository(ILogger<ErrorRepository> logger,
            FireplaceApiDbContext dbContext, ErrorConverter errorConverter)
        {
            _logger = logger;
            _dbContext = dbContext;
            _errorEntities = dbContext.ErrorEntities;
            _errorConverter = errorConverter;
        }

        public async Task<List<Error>> ListErrorsAsync()
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: null);
            var sw = Stopwatch.StartNew();
            var errorEntites = await _errorEntities
                .AsNoTracking()
                .Search(
                    identifier: null
                )
                .Include(
                )
                .OrderBy(e => e.Code)
                .ToListAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT",
                parameters: new { errorEntites = errorEntites.Select(e => e.Id) });
            return errorEntites.Select(_errorConverter.ConvertToModel).ToList();
        }

        public async Task<Error> GetErrorAsync(ErrorIdentifier identifier)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { identifier });
            if(_cache.TryGetValue(identifier.Key, out Error cachedError))
            {
                _logger.LogAppInformation(title: "CACHED_DATABASE_OUTPUT", parameters: new { cachedError });
                return cachedError;
            }
            var sw = Stopwatch.StartNew();
            var errorEntity = await _errorEntities
                .AsNoTracking()
                .Search(
                    identifier: identifier
                )
                .Include(
                )
                .SingleOrDefaultAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { errorEntity });
            var error = _errorConverter.ConvertToModel(errorEntity);
            _cache.Add(identifier.Key, error);
            return error;
        }

        public async Task<Error> CreateErrorAsync(ulong id, int code, ErrorType type,
            FieldName field, string clientMessage, int httpStatusCode)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT",
                parameters: new { id, code, type, field, clientMessage, httpStatusCode });
            var sw = Stopwatch.StartNew();
            var errorEntity = new ErrorEntity(id, code, type.Name, field.Name,
                clientMessage, httpStatusCode);
            _errorEntities.Add(errorEntity);
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachAllEntries();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { errorEntity });
            return _errorConverter.ConvertToModel(errorEntity);
        }

        public async Task<Error> UpdateErrorAsync(Error error)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { error });
            var sw = Stopwatch.StartNew();
            var errorEntity = _errorConverter.ConvertToEntity(error);
            _errorEntities.Update(errorEntity);
            try
            {
                await _dbContext.SaveChangesAsync();
                _dbContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new InternalServerException("Can't update the errorEntity DbUpdateConcurrencyException!",
                    parameters: errorEntity, systemException: ex);
            }

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { errorEntity });
            return _errorConverter.ConvertToModel(errorEntity);
        }

        public async Task DeleteErrorAsync(ErrorIdentifier identifier)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { identifier });
            var sw = Stopwatch.StartNew();
            var errorEntity = await _errorEntities
                .Search(
                    identifier: identifier
                )
                .SingleOrDefaultAsync();

            _errorEntities.Remove(errorEntity);
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachAllEntries();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { errorEntity });
        }

        public async Task<bool> DoesErrorExistAsync(ErrorIdentifier identifier)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { identifier });
            var sw = Stopwatch.StartNew();
            var doesExist = await _errorEntities
                .AsNoTracking()
                .Search(
                    identifier: identifier
                )
                .AnyAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { doesExist });
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

        public static IQueryable<ErrorEntity> Search(
            [NotNull] this IQueryable<ErrorEntity> q, ErrorIdentifier identifier)
        {
            if (identifier != null)
            {
                switch (identifier)
                {
                    case ErrorIdIdentifier idIdentifier:
                        q = q.Where(e => e.Id == idIdentifier.Id);
                        break;
                    case ErrorCodeIdentifier codeIdentifier:
                        q = q.Where(e => e.Code == codeIdentifier.Code);
                        break;
                    case ErrorTypeAndFieldIdentifier typeAndFieldIdentifier:
                        q = q.Where(e =>
                            e.Type == typeAndFieldIdentifier.Type.Name
                            && e.Field == typeAndFieldIdentifier.Field.Name);
                        break;
                }
            }

            return q;
        }
    }
}
