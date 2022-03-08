using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Identifiers;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
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
    public class CommunityMembershipRepository : ICommunityMembershipRepository
    {
        private readonly ILogger<CommunityMembershipRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly FireplaceApiContext _fireplaceApiContext;
        private readonly DbSet<CommunityMembershipEntity> _communityMembershipEntities;
        private readonly CommunityMembershipConverter _communityMembershipConverter;

        public CommunityMembershipRepository(ILogger<CommunityMembershipRepository> logger, IConfiguration configuration,
            FireplaceApiContext fireplaceApiContext, CommunityMembershipConverter communityMembershipConverter)
        {
            _logger = logger;
            _configuration = configuration;
            _fireplaceApiContext = fireplaceApiContext;
            _communityMembershipEntities = fireplaceApiContext.CommunityMembershipEntities;
            _communityMembershipConverter = communityMembershipConverter;
        }

        public async Task<List<CommunityMembership>> ListCommunityMembershipsAsync(List<ulong> Ids)
        {
            _logger.LogAppIOInformation("Database | Input", new { Ids });
            var sw = Stopwatch.StartNew();
            var communityMembershipEntities = await _communityMembershipEntities
                .AsNoTracking()
                .Where(e => Ids.Contains(e.Id))
                .ToListAsync();

            var communityMembershipEntityDictionary = new Dictionary<ulong, CommunityMembershipEntity>();
            communityMembershipEntities.ForEach(e => communityMembershipEntityDictionary[e.Id] = e);
            communityMembershipEntities = new List<CommunityMembershipEntity>();
            Ids.ForEach(id => communityMembershipEntities.Add(communityMembershipEntityDictionary[id]));

            _logger.LogAppIOInformation(sw, "Database | Output", new { communityMembershipEntities });
            return communityMembershipEntities.Select(e => _communityMembershipConverter.ConvertToModel(e)).ToList();
        }

        public async Task<List<CommunityMembership>> ListCommunityMembershipsAsync(UserIdentifier userIdentifier)
        {
            _logger.LogAppIOInformation("Database | Input", new { userIdentifier });
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
                .Take(GlobalOperator.GlobalValues.Pagination.TotalItemsCount)
                .ToListAsync();

            _logger.LogAppIOInformation(sw, "Database | Output", new { communityMembershipEntities });
            return communityMembershipEntities.Select(e => _communityMembershipConverter.ConvertToModel(e)).ToList();
        }

        public async Task<List<ulong>> ListCommunityMembershipIdsAsync(UserIdentifier userIdentifier)
        {
            _logger.LogAppIOInformation("Database | Input", new { userIdentifier });
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
                .Take(GlobalOperator.GlobalValues.Pagination.TotalItemsCount)
                .Select(e => e.Id)
                .ToListAsync();

            _logger.LogAppIOInformation(sw, "Database | Output", new { communityMembershipEntityIds });
            return communityMembershipEntityIds;
        }

        public async Task<CommunityMembership> GetCommunityMembershipByIdentifierAsync(
            CommunityMembershipIdentifier identifier, bool includeUser = false,
            bool includeCommunity = false)
        {
            _logger.LogAppIOInformation("Database | Input",
                new { identifier, includeUser, includeCommunity });
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

            _logger.LogAppIOInformation(sw, "Database | Output",
                new { communityMembershipEntity });
            return _communityMembershipConverter.ConvertToModel(communityMembershipEntity);
        }

        public async Task<CommunityMembership> CreateCommunityMembershipAsync(ulong id,
            ulong userId, string username, ulong communityId, string communityName)
        {
            _logger.LogAppIOInformation("Database | Input",
                new { id, userId, username, communityId, communityName });
            var sw = Stopwatch.StartNew();
            var communityMembershipEntity = new CommunityMembershipEntity(id, userId,
                username, communityId, communityName);
            _communityMembershipEntities.Add(communityMembershipEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogAppIOInformation(sw, "Database | Output", new { communityMembershipEntity });
            return _communityMembershipConverter.ConvertToModel(communityMembershipEntity);
        }

        public async Task<CommunityMembership> UpdateCommunityMembershipAsync(
            CommunityMembership communityMembership)
        {
            _logger.LogAppIOInformation("Database | Input", new { communityMembership });
            var sw = Stopwatch.StartNew();
            var communityMembershipEntity = _communityMembershipConverter.ConvertToEntity(communityMembership);
            _communityMembershipEntities.Update(communityMembershipEntity);
            try
            {
                await _fireplaceApiContext.SaveChangesAsync();
                _fireplaceApiContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var serverMessage = $"Can't update the communityMembershipEntity DbUpdateConcurrencyException. {communityMembershipEntity.ToJson()}";
                throw new ApiException(ErrorName.INTERNAL_SERVER, serverMessage, systemException: ex);
            }

            _logger.LogAppIOInformation(sw, "Database | Output", new { communityMembershipEntity });
            return _communityMembershipConverter.ConvertToModel(communityMembershipEntity);
        }

        public async Task DeleteCommunityMembershipByIdentifierAsync(CommunityMembershipIdentifier identifier)
        {
            _logger.LogAppIOInformation("Database | Input", new { identifier });
            var sw = Stopwatch.StartNew();
            var communityMembershipEntity = await _communityMembershipEntities
                .Search(
                    identifier: identifier,
                    userIdentifier: null,
                    communityIdentifier: null
                )
                .SingleOrDefaultAsync();

            _communityMembershipEntities.Remove(communityMembershipEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogAppIOInformation(sw, "Database | Output", new { communityMembershipEntity });
        }


        public async Task<bool> DoesCommunityMembershipIdentifierExistAsync(CommunityMembershipIdentifier identifier)
        {
            _logger.LogAppIOInformation("Database | Input", new { identifier });
            var sw = Stopwatch.StartNew();
            var doesExist = await _communityMembershipEntities
                .AsNoTracking()
                .Search(
                    identifier: identifier,
                    userIdentifier: null,
                    communityIdentifier: null
                )
                .AnyAsync();

            _logger.LogAppIOInformation(sw, "Database | Output", new { doesExist });
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
