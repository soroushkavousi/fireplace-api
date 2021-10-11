using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.ValueObjects;
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
    public class UserRepository : IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly FireplaceApiContext _fireplaceApiContext;
        private readonly DbSet<UserEntity> _userEntities;
        private readonly UserConverter _userConverter;

        public UserRepository(ILogger<UserRepository> logger, IConfiguration configuration,
                    FireplaceApiContext fireplaceApiContext, UserConverter userConverter)
        {
            _logger = logger;
            _configuration = configuration;
            _fireplaceApiContext = fireplaceApiContext;
            _userEntities = fireplaceApiContext.UserEntities;
            _userConverter = userConverter;
        }

        public async Task<List<User>> ListUsersAsync(
                    bool includeEmail = false, bool includeGoogleUser = false,
                    bool includeAccessTokens = false, bool includeSessions = false)
        {
            _logger.LogIOInformation(null, "Database | Iutput",
                new { includeEmail, includeGoogleUser, includeAccessTokens, includeSessions });
            var sw = Stopwatch.StartNew();
            var userEntities = await _userEntities
                .AsNoTracking()
                .Include(
                    emailEntity: includeEmail,
                    googleUserEntity: includeGoogleUser,
                    accessTokenEntities: includeAccessTokens,
                    sessionEntities: includeSessions
                )
                .ToListAsync();

            _logger.LogIOInformation(sw, "Database | Output",
                new { userEntities });
            return userEntities.Select(e => _userConverter.ConvertToModel(e)).ToList();
        }

        public async Task<User> GetUserByIdAsync(long id,
            bool includeEmail = false, bool includeGoogleUser = false,
            bool includeAccessTokens = false, bool includeSessions = false)
        {
            _logger.LogIOInformation(null, "Database | Iutput",
                new { id, includeEmail, includeGoogleUser, includeAccessTokens, includeSessions });
            var sw = Stopwatch.StartNew();
            var userEntity = await _userEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Include(
                    emailEntity: includeEmail,
                    googleUserEntity: includeGoogleUser,
                    accessTokenEntities: includeAccessTokens,
                    sessionEntities: includeSessions
                )
                .SingleOrDefaultAsync();

            _logger.LogIOInformation(sw, "Database | Output",
                new { userEntity });
            return _userConverter.ConvertToModel(userEntity);
        }

        public async Task<User> GetUserByUsernameAsync(string username,
            bool includeEmail = false, bool includeGoogleUser = false,
            bool includeAccessTokens = false, bool includeSessions = false)
        {
            _logger.LogIOInformation(null, "Database | Iutput",
                new { username, includeEmail, includeGoogleUser, includeAccessTokens, includeSessions });
            var sw = Stopwatch.StartNew();
            var userEntity = await _userEntities
                .AsNoTracking()
                .Where(e => e.Username == username)
                .Include(
                    emailEntity: includeEmail,
                    googleUserEntity: includeGoogleUser,
                    accessTokenEntities: includeAccessTokens,
                    sessionEntities: includeSessions
                )
                .SingleOrDefaultAsync();

            _logger.LogIOInformation(sw, "Database | Output",
                new { userEntity });
            return _userConverter.ConvertToModel(userEntity);
        }

        public async Task<string> GetUsernameByIdAsync(long id)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { id });
            var sw = Stopwatch.StartNew();
            var username = (await _userEntities
                .AsNoTracking()
                .Select(e => new { Id = e.Id.Value, e.Username })
                .SingleAsync(e => e.Id == id))
                .Username;

            _logger.LogIOInformation(sw, "Database | Output", new { username });
            return username;
        }

        public async Task<long> GetIdByUsernameAsync(string username)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { username });
            var sw = Stopwatch.StartNew();
            var userId = (await _userEntities
                .AsNoTracking()
                .Select(e => new { Id = e.Id.Value, e.Username })
                .SingleAsync(e => string.Equals(e.Username, username)))
                .Id;

            _logger.LogIOInformation(sw, "Database | Output", new { userId });
            return userId;
        }

        public async Task<User> CreateUserAsync(string firstName, string lastName,
            string username, UserState state, Password password = null)
        {
            _logger.LogIOInformation(null, "Database | Iutput",
                new { firstName, lastName, username, state, passwordHash = password?.Hash });
            var sw = Stopwatch.StartNew();
            var userEntity = new UserEntity(firstName, lastName,
                username, state.ToString(), passwordHash: password?.Hash);
            _userEntities.Add(userEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database | Output",
                new { userEntity });
            return _userConverter.ConvertToModel(userEntity);
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { user });
            var sw = Stopwatch.StartNew();
            var userEntity = _userConverter.ConvertToEntity(user);
            _userEntities.Update(userEntity);
            try
            {
                await _fireplaceApiContext.SaveChangesAsync();
                _fireplaceApiContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var serverMessage = $"Can't update the userEntity DbUpdateConcurrencyException. {userEntity.ToJson()}";
                throw new ApiException(ErrorName.INTERNAL_SERVER, serverMessage, systemException: ex);
            }

            _logger.LogIOInformation(sw, "Database | Output", new { userEntity });
            return _userConverter.ConvertToModel(userEntity);
        }

        public async Task DeleteUserByIdAsync(long id)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { id });
            var sw = Stopwatch.StartNew();
            var userEntity = await _userEntities
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync();

            _userEntities.Remove(userEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database | Output", new { userEntity });
        }

        public async Task DeleteUserByUsernameAsync(string username)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { username });
            var sw = Stopwatch.StartNew();
            var userEntity = await _userEntities
                .Where(e => e.Username == username)
                .SingleOrDefaultAsync();

            _userEntities.Remove(userEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database | Output", new { userEntity });
        }

        public async Task<bool> DoesUserIdExistAsync(long id)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { id });
            var sw = Stopwatch.StartNew();
            var doesExist = await _userEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .AnyAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { doesExist });
            return doesExist;
        }

        public async Task<bool> DoesUsernameExistAsync(string username)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { username });
            var sw = Stopwatch.StartNew();
            var doesExist = await _userEntities
                .AsNoTracking()
                .Where(e => e.Username == username)
                .AnyAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { doesExist });
            return doesExist;
        }
    }

    public static class UserRepositoryExtensions
    {
        public static IQueryable<UserEntity> Include(
                    [NotNull] this IQueryable<UserEntity> userEntitiesQuery,
                    bool emailEntity, bool googleUserEntity,
                    bool accessTokenEntities, bool sessionEntities)
        {
            if (emailEntity)
                userEntitiesQuery = userEntitiesQuery.Include(e => e.EmailEntity);

            if (googleUserEntity)
                userEntitiesQuery = userEntitiesQuery.Include(e => e.GoogleUserEntity);

            if (accessTokenEntities)
                userEntitiesQuery = userEntitiesQuery.Include(e => e.AccessTokenEntities);

            if (sessionEntities)
                userEntitiesQuery = userEntitiesQuery.Include(e => e.SessionEntities);

            return userEntitiesQuery;
        }
    }
}
