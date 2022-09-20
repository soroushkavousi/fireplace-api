using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Identifiers;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Models;
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
    public class CommunityMembershipRepository : ICommunityMembershipRepository
    {
        private readonly ILogger<CommunityMembershipRepository> _logger;
        private readonly FireplaceApiDbContext _dbContext;
        private readonly DbSet<CommunityMembershipEntity> _communityMembershipEntities;
        private readonly CommunityMembershipConverter _communityMembershipConverter;

        public CommunityMembershipRepository(ILogger<CommunityMembershipRepository> logger,
            FireplaceApiDbContext dbContext, CommunityMembershipConverter communityMembershipConverter)
        {
            _logger = logger;
            _dbContext = dbContext;
            _communityMembershipEntities = dbContext.CommunityMembershipEntities;
            _communityMembershipConverter = communityMembershipConverter;
        }

        public async Task<List<CommunityMembership>> ListCommunityMembershipsAsync(List<ulong> Ids)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { Ids });
            var sw = Stopwatch.StartNew();
            var communityMembershipEntities = await _communityMembershipEntities
                .AsNoTracking()
                .Where(e => Ids.Contains(e.Id))
                .ToListAsync();

            var communityMembershipEntityDictionary = new Dictionary<ulong, CommunityMembershipEntity>();
            communityMembershipEntities.ForEach(e => communityMembershipEntityDictionary[e.Id] = e);
            communityMembershipEntities = new List<CommunityMembershipEntity>();
            Ids.ForEach(id => communityMembershipEntities.Add(communityMembershipEntityDictionary[id]));

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { communityMembershipEntities });
            return communityMembershipEntities.Select(e => _communityMembershipConverter.ConvertToModel(e)).ToList();
        }

        public async Task<List<CommunityMembership>> ListCommunityMembershipsAsync(UserIdentifier userIdentifier)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { userIdentifier });
            var sw = Stopwatch.StartNew();
            var communityMembershipEntities = await _communityMembershipEntities
                .AsNoTracking()
                .Search(
                    identifier: null,
                    userIdentifier: userIdentifier,
                    communityIdentifier: null
                )
                .Include(
                    userEntity: false,
                    communityEntity: false
                )
                .Take(Configs.Current.Pagination.TotalItemsCount)
                .ToListAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { communityMembershipEntities });
            return communityMembershipEntities.Select(e => _communityMembershipConverter.ConvertToModel(e)).ToList();
        }

        public async Task<List<ulong>> ListCommunityMembershipIdsAsync(UserIdentifier userIdentifier)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { userIdentifier });
            var sw = Stopwatch.StartNew();
            var communityMembershipEntityIds = await _communityMembershipEntities
                .AsNoTracking()
                .Search(
                    identifier: null,
                    userIdentifier: userIdentifier,
                    communityIdentifier: null
                )
                .Include(
                    userEntity: false,
                    communityEntity: false
                )
                .Take(Configs.Current.Pagination.TotalItemsCount)
                .Select(e => e.Id)
                .ToListAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { communityMembershipEntityIds });
            return communityMembershipEntityIds;
        }

        public async Task<CommunityMembership> GetCommunityMembershipByIdentifierAsync(
            CommunityMembershipIdentifier identifier, bool includeUser = false,
            bool includeCommunity = false)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT",
                parameters: new { identifier, includeUser, includeCommunity });
            var sw = Stopwatch.StartNew();
            var communityMembershipEntity = await _communityMembershipEntities
                .AsNoTracking()
                .Search(
                    identifier: identifier,
                    userIdentifier: null,
                    communityIdentifier: null
                )
                .Include(
                    userEntity: includeUser,
                    communityEntity: includeCommunity
                )
                .SingleOrDefaultAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT",
                parameters: new { communityMembershipEntity });
            return _communityMembershipConverter.ConvertToModel(communityMembershipEntity);
        }

        public async Task<CommunityMembership> CreateCommunityMembershipAsync(ulong id,
            ulong userId, string username, ulong communityId, string communityName)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT",
                parameters: new { id, userId, username, communityId, communityName });
            var sw = Stopwatch.StartNew();
            var communityMembershipEntity = new CommunityMembershipEntity(id, userId,
                username, communityId, communityName);
            _communityMembershipEntities.Add(communityMembershipEntity);
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachAllEntries();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { communityMembershipEntity });
            return _communityMembershipConverter.ConvertToModel(communityMembershipEntity);
        }

        public async Task<CommunityMembership> UpdateCommunityMembershipAsync(
            CommunityMembership communityMembership)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { communityMembership });
            var sw = Stopwatch.StartNew();
            var communityMembershipEntity = _communityMembershipConverter.ConvertToEntity(communityMembership);
            _communityMembershipEntities.Update(communityMembershipEntity);
            try
            {
                await _dbContext.SaveChangesAsync();
                _dbContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var serverMessage = $"Can't update the communityMembershipEntity DbUpdateConcurrencyException. {communityMembershipEntity.ToJson()}";
                throw new ApiException(ErrorName.INTERNAL_SERVER, serverMessage, systemException: ex);
            }

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { communityMembershipEntity });
            return _communityMembershipConverter.ConvertToModel(communityMembershipEntity);
        }

        public async Task DeleteCommunityMembershipByIdentifierAsync(CommunityMembershipIdentifier identifier)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { identifier });
            var sw = Stopwatch.StartNew();
            var communityMembershipEntity = await _communityMembershipEntities
                .Search(
                    identifier: identifier,
                    userIdentifier: null,
                    communityIdentifier: null
                )
                .SingleOrDefaultAsync();

            _communityMembershipEntities.Remove(communityMembershipEntity);
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachAllEntries();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { communityMembershipEntity });
        }


        public async Task<bool> DoesCommunityMembershipIdentifierExistAsync(CommunityMembershipIdentifier identifier)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { identifier });
            var sw = Stopwatch.StartNew();
            var doesExist = await _communityMembershipEntities
                .AsNoTracking()
                .Search(
                    identifier: identifier,
                    userIdentifier: null,
                    communityIdentifier: null
                )
                .AnyAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { doesExist });
            return doesExist;
        }
    }

    public static class CommunityMembershipRepositoryExtensions
    {
        public static IQueryable<CommunityMembershipEntity> Include(
            [NotNull] this IQueryable<CommunityMembershipEntity> q,
            bool userEntity, bool communityEntity)
        {
            if (userEntity)
                q = q.Include(e => e.UserEntity);

            if (communityEntity)
                q = q.Include(e => e.CommunityEntity);

            return q;
        }

        public static IQueryable<CommunityMembershipEntity> Search(
            [NotNull] this IQueryable<CommunityMembershipEntity> q,
            CommunityMembershipIdentifier identifier,
            UserIdentifier userIdentifier, CommunityIdentifier communityIdentifier)
        {
            if (identifier != null)
            {
                switch (identifier)
                {
                    case CommunityMembershipIdIdentifier idIdentifier:
                        q = q.Where(e => e.UserEntityId == idIdentifier.Id);
                        break;
                    case CommunityMembershipUserAndCommunityIdentifier userAndCommunity:
                        userIdentifier = userAndCommunity.UserIdentifier;
                        communityIdentifier = userAndCommunity.CommunityIdentifier;
                        break;
                }
            }

            if (userIdentifier != null)
            {
                switch (userIdentifier)
                {
                    case UserIdIdentifier idIdentifier:
                        q = q.Where(e => e.UserEntityId == idIdentifier.Id);
                        break;
                    case UserUsernameIdentifier usernameIdentifier:
                        q = q.Where(e => e.UserEntityUsername == usernameIdentifier.Username);
                        break;
                }
            }

            if (communityIdentifier != null)
            {
                switch (communityIdentifier)
                {
                    case CommunityIdIdentifier idIdentifier:
                        q = q.Where(e => e.CommunityEntityId == idIdentifier.Id);
                        break;
                    case CommunityNameIdentifier nameIdentifier:
                        q = q.Where(e => e.CommunityEntityName == nameIdentifier.Name);
                        break;
                }
            }

            return q;
        }
    }
}
