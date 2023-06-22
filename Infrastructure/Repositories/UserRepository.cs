using FireplaceApi.Application.Users;
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

public class UserRepository : IUserRepository
{
    private readonly ILogger<UserRepository> _logger;
    private readonly ProjectDbContext _dbContext;
    private readonly DbSet<UserEntity> _userEntities;

    public UserRepository(ILogger<UserRepository> logger,
        ProjectDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
        _userEntities = dbContext.UserEntities;
    }

    public async Task<List<User>> ListUsersAsync(
        bool includeEmail = false, bool includeGoogleUser = false,
        bool includeSessions = false)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT",
            parameters: new { includeEmail, includeGoogleUser, includeSessions });
        var sw = Stopwatch.StartNew();
        var userEntities = await _userEntities
            .AsNoTracking()
            .Include(
                emailEntity: includeEmail,
                googleUserEntity: includeGoogleUser,
                sessionEntities: includeSessions
            )
            .ToListAsync();

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT",
            parameters: new { userEntities = userEntities.Select(e => e.Id) });
        return userEntities.Select(UserConverter.ToModel).ToList();
    }

    public async Task<User> GetUserByIdentifierAsync(UserIdentifier identifier,
        bool includeEmail = false, bool includeGoogleUser = false,
        bool includeSessions = false)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT",
            parameters: new
            {
                identifier,
                includeEmail,
                includeGoogleUser,
                includeSessions
            });
        var sw = Stopwatch.StartNew();
        var userEntity = await _userEntities
            .AsNoTracking()
            .Search(
                identifier: identifier
            )
            .Include(
                emailEntity: includeEmail,
                googleUserEntity: includeGoogleUser,
                sessionEntities: includeSessions
            )
            .SingleOrDefaultAsync();

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT",
            parameters: new { userEntity });
        return userEntity.ToModel();
    }

    public async Task<string> GetUsernameByIdAsync(ulong id)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { id });
        var sw = Stopwatch.StartNew();
        var username = (await _userEntities
            .AsNoTracking()
            .Select(e => new { e.Id, e.Username })
            .SingleAsync(e => e.Id == id))
            .Username;

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { username });
        return username;
    }

    public async Task<ulong> GetIdByUsernameAsync(string username)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { username });
        var sw = Stopwatch.StartNew();
        var userId = (await _userEntities
            .AsNoTracking()
            .Select(e => new { e.Id, e.Username })
            .SingleAsync(e => string.Equals(e.Username, username)))
            .Id;

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { userId });
        return userId;
    }

    public async Task<User> CreateUserAsync(ulong id, string username,
        UserState state, List<UserRole> roles, Password password = null,
        string displayName = null, string about = null, string avatarUrl = null,
        string bannerUrl = null)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new
        {
            id,
            username,
            state,
            roles,
            password,
            displayName,
            about,
            avatarUrl,
            bannerUrl
        });
        var sw = Stopwatch.StartNew();
        var userEntityRoles = roles.Select(r => r.ToString()).ToList();
        var userEntity = new UserEntity(id, username, state.ToString(),
            roles: userEntityRoles, displayName: displayName, about: about,
            avatarUrl: avatarUrl, bannerUrl: bannerUrl,
            passwordHash: password?.Hash);
        _userEntities.Add(userEntity);
        await _dbContext.SaveChangesAsync();
        _dbContext.DetachAllEntries();

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT",
            parameters: new { userEntity });
        return userEntity.ToModel();
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { user });
        var sw = Stopwatch.StartNew();
        var userEntity = user.ToEntity();
        _userEntities.Update(userEntity);
        try
        {
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachAllEntries();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new InternalServerException("Can't update the userEntity DbUpdateConcurrencyException!",
                parameters: userEntity, systemException: ex);
        }

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { userEntity });
        return userEntity.ToModel();
    }

    public async Task UpdateUsernameAsync(ulong id, string newUsername)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { id, newUsername });
        var sw = Stopwatch.StartNew();
        int rowAffectedCount = 0;
        try
        {
            rowAffectedCount = await _dbContext.Database.ExecuteSqlInterpolatedAsync(
                $"CALL public.\"UpdateUsername\"({id}, {newUsername});");
            _dbContext.DetachAllEntries();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new InternalServerException("Can't update the userEntity DbUpdateConcurrencyException!",
                parameters: new { id, newUsername, rowAffectedCount }, systemException: ex);
        }

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { rowAffectedCount });
    }

    public async Task DeleteUserByIdentifierAsync(UserIdentifier identifier)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { identifier });
        var sw = Stopwatch.StartNew();
        var userEntity = await _userEntities
            .Search(
                identifier: identifier
            )
            .SingleOrDefaultAsync();

        _userEntities.Remove(userEntity);
        await _dbContext.SaveChangesAsync();
        _dbContext.DetachAllEntries();

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { userEntity });
    }

    public async Task<bool> DoesUserIdentifierExistAsync(UserIdentifier identifier)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { identifier });
        var sw = Stopwatch.StartNew();
        var doesExist = await _userEntities
            .AsNoTracking()
            .Search(
                identifier: identifier
            )
            .AnyAsync();

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { doesExist });
        return doesExist;
    }
}

public static class UserRepositoryExtensions
{
    public static IQueryable<UserEntity> Include(
        [NotNull] this IQueryable<UserEntity> userEntitiesQuery,
        bool emailEntity, bool googleUserEntity, bool sessionEntities)
    {
        if (emailEntity)
            userEntitiesQuery = userEntitiesQuery.Include(e => e.EmailEntity);

        if (googleUserEntity)
            userEntitiesQuery = userEntitiesQuery.Include(e => e.GoogleUserEntity);

        if (sessionEntities)
            userEntitiesQuery = userEntitiesQuery.Include(e => e.SessionEntities);

        return userEntitiesQuery;
    }

    public static IQueryable<UserEntity> Search(
        [NotNull] this IQueryable<UserEntity> q, UserIdentifier identifier)
    {
        if (identifier != null)
        {
            switch (identifier)
            {
                case UserIdIdentifier idIdentifier:
                    q = q.Where(e => e.Id == idIdentifier.Id);
                    break;
                case UserUsernameIdentifier usernameIdentifier:
                    q = q.Where(e => e.Username == usernameIdentifier.Username);
                    break;
            }
        }

        return q;
    }
}
