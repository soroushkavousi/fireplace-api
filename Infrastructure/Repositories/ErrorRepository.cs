using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GamingCommunityApi.Infrastructure.Converters;
using GamingCommunityApi.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GamingCommunityApi.Core.Interfaces.IRepositories;
using GamingCommunityApi.Core.Enums;
using GamingCommunityApi.Core.Models;
using GamingCommunityApi.Core.Exceptions;
using GamingCommunityApi.Core.Extensions;

namespace GamingCommunityApi.Infrastructure.Repositories
{
    public class ErrorRepository: IErrorRepository
    {
        private readonly ILogger<ErrorRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly GamingCommunityApiContext _gamingCommunityApiContext;
        private readonly DbSet<ErrorEntity> _errorEntities;
        private readonly ErrorConverter _errorConverter;

        public ErrorRepository(ILogger<ErrorRepository> logger, IConfiguration configuration, 
            GamingCommunityApiContext gamingCommunityApiContext, ErrorConverter errorConverter
            )
        {
            _logger = logger;
            _configuration = configuration;
            _gamingCommunityApiContext = gamingCommunityApiContext;
            _errorEntities = gamingCommunityApiContext.ErrorEntities;
            _errorConverter = errorConverter;
        }

        public async Task<List<Error>> ListErrorsAsync()
        {
            var errorEntites = await _errorEntities
                .AsNoTracking()
                .Include(
                )
                .OrderBy(e => e.Code)
                .ToListAsync();

            return errorEntites.Select(e => _errorConverter.ConvertToModel(e)).ToList();
        }

        public async Task<Error> GetErrorByNameAsync(ErrorName name)
        {
            var errorEntity = await _errorEntities
                .AsNoTracking()
                .Where(e => e.Name == name.ToString())
                .Include(
                )
                .SingleOrDefaultAsync();

            return _errorConverter.ConvertToModel(errorEntity);
        }

        public async Task<Error> GetErrorByCodeAsync(int code)
        {
            var errorEntity = await _errorEntities
                .AsNoTracking()
                .Where(e => e.Code == code)
                .Include(
                )
                .SingleOrDefaultAsync();
        
            return _errorConverter.ConvertToModel(errorEntity);
        }

        public async Task<Error> CreateErrorAsync(ErrorName name, int code, string clientMessage, 
            int httpStatusCode)
        {
            var errorEntity = new ErrorEntity(name.ToString(), code, clientMessage, httpStatusCode);
            _errorEntities.Add(errorEntity);
            await _gamingCommunityApiContext.SaveChangesAsync();
            _gamingCommunityApiContext.DetachAllEntries();
            return _errorConverter.ConvertToModel(errorEntity);
        }

        public async Task<Error> UpdateErrorAsync(Error error)
        {
            var errorEntity = _errorConverter.ConvertToEntity(error);
            _errorEntities.Update(errorEntity);
            try
            {
                await _gamingCommunityApiContext.SaveChangesAsync();
                _gamingCommunityApiContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var serverMessage = $"Can't update the errorEntity DbUpdateConcurrencyException. {errorEntity.ToJson()}";
                throw new ApiException(ErrorName.INTERNAL_SERVER, serverMessage, systemException: ex);
            }
            return _errorConverter.ConvertToModel(errorEntity);
        }

        public async Task DeleteErrorAsync(int code)
        {
            var errorEntity = await _errorEntities
                .Where(e => e.Code == code)
                .SingleOrDefaultAsync();

            _errorEntities.Remove(errorEntity);
            await _gamingCommunityApiContext.SaveChangesAsync();
            _gamingCommunityApiContext.DetachAllEntries();
        }

        public async Task<bool> DoesErrorNameExistAsync(ErrorName name)
        {
            return await _errorEntities
                .AsNoTracking()
                .Where(e => e.Name == name.ToString())
                .AnyAsync();
        }

        public async Task<bool> DoesErrorCodeExistAsync(int code)
        {
            return await _errorEntities
                .AsNoTracking()
                .Where(e => e.Code == code)
                .AnyAsync();
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
