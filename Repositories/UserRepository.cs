using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GamingCommunityApi.Converters;
using GamingCommunityApi.Entities;
using GamingCommunityApi.Entities.UserInformationEntities;
using GamingCommunityApi.Enums;
using GamingCommunityApi.Exceptions;
using GamingCommunityApi.Extensions;
using GamingCommunityApi.Models;
using GamingCommunityApi.Models.UserInformations;
using GamingCommunityApi.ValueObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GamingCommunityApi.Repositories
{
    public class UserRepository
    {
        private readonly ILogger<UserRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly GamingCommunityApiContext _gamingCommunityApiContext;
        private readonly DbSet<UserEntity> _userEntities;
        private readonly UserConverter _userConverter;

        public UserRepository(ILogger<UserRepository> logger, IConfiguration configuration, 
                    GamingCommunityApiContext gamingCommunityApiContext, UserConverter userConverter)
        {
            _logger = logger;
            _configuration = configuration;
            _gamingCommunityApiContext = gamingCommunityApiContext;
            _userEntities = gamingCommunityApiContext.UserEntities;
            _userConverter = userConverter;
        }

        public async Task<List<User>> ListUsersAsync(
                    bool includeEmail = false, bool includeAccessTokens = false, 
                    bool includeSessions = false)
        {
            var userEntities = await _userEntities
                .AsNoTracking()
                .Include(
                    emailEntity: includeEmail,
                    accessTokenEntities: includeAccessTokens,
                    sessionEntities: includeSessions
                )
                .ToListAsync();

            return userEntities.Select(e => _userConverter.ConvertToModel(e)).ToList();
        }

        public async Task<User> GetUserByIdAsync(long id, bool includeEmail = false, 
            bool includeAccessTokens = false, bool includeSessions = false)
        {
            var userEntity = await _userEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Include(
                    emailEntity: includeEmail,
                    accessTokenEntities: includeAccessTokens,
                    sessionEntities: includeSessions
                )
                .SingleOrDefaultAsync();

            return _userConverter.ConvertToModel(userEntity);
        }

        public async Task<User> GetUserByUsernameAsync(string username,
            bool includeEmail = false, bool includeAccessTokens = false,
            bool includeSessions = false)
        {
            var userEntity = await _userEntities
                .AsNoTracking()
                .Where(e => e.Username == username)
                .Include(
                    emailEntity: includeEmail,
                    accessTokenEntities: includeAccessTokens,
                    sessionEntities: includeSessions
                )
                .SingleOrDefaultAsync();

            return _userConverter.ConvertToModel(userEntity);
        }

        public async Task<User> CreateUserAsync(string firstName, string lastName,
            string username, Password password, UserState state)
        {
            var userEntity = new UserEntity(firstName, lastName,
                username, password.Hash, state.ToString());
            _userEntities.Add(userEntity);
            await _gamingCommunityApiContext.SaveChangesAsync();
            _gamingCommunityApiContext.DetachAllEntries();
            return _userConverter.ConvertToModel(userEntity);
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            var userEntity = _userConverter.ConvertToEntity(user);
            _userEntities.Update(userEntity);
            try
            {
                await _gamingCommunityApiContext.SaveChangesAsync();
                _gamingCommunityApiContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var serverMessage = $"Can't update the dbUser DbUpdateConcurrencyException. {userEntity.ToJson()}";
                throw new ApiException(ErrorName.INTERNAL_SERVER, serverMessage, systemException: ex);
            }
            return _userConverter.ConvertToModel(userEntity);
        }

        public async Task DeleteUserAsync(long id)
        {
            var userEntity = await _userEntities
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync();

            _userEntities.Remove(userEntity);
            await _gamingCommunityApiContext.SaveChangesAsync();
            _gamingCommunityApiContext.DetachAllEntries();
        }

        public async Task<bool> DoesUserIdExistAsync(long id)
        {
            return await _userEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .AnyAsync();
        }

        public async Task<bool> DoesUsernameExistAsync(string username)
        {
            return await _userEntities
                .AsNoTracking()
                .Where(e => e.Username == username)
                .AnyAsync();
        }
    }

    public static class UserRepositoryExtensions
    {
        public static IQueryable<UserEntity> Include(
                    [NotNull] this IQueryable<UserEntity> userEntitiesQuery,
                    bool emailEntity, bool accessTokenEntities,
                    bool sessionEntities)
        {
            if (emailEntity)
                userEntitiesQuery = userEntitiesQuery.Include(e => e.EmailEntity);

            if (accessTokenEntities)
                userEntitiesQuery = userEntitiesQuery.Include(e => e.AccessTokenEntities);

            if (sessionEntities)
                userEntitiesQuery = userEntitiesQuery.Include(e => e.SessionEntities);

            return userEntitiesQuery;
        }
    }
}
