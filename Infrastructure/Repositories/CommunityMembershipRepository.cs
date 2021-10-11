using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
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

        public async Task<List<CommunityMembership>> ListCommunityMembershipsAsync(List<long> Ids)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { Ids });
            var sw = Stopwatch.StartNew();
            var communityMembershipEntities = await _communityMembershipEntities
                .AsNoTracking()
                .Where(e => Ids.Contains(e.Id.Value))
                .ToListAsync();

            Dictionary<long, CommunityMembershipEntity> communityMembershipEntityDictionary = new Dictionary<long, CommunityMembershipEntity>();
            communityMembershipEntities.ForEach(e => communityMembershipEntityDictionary[e.Id.Value] = e);
            communityMembershipEntities = new List<CommunityMembershipEntity>();
            Ids.ForEach(id => communityMembershipEntities.Add(communityMembershipEntityDictionary[id]));

            _logger.LogIOInformation(sw, "Database | Output", new { communityMembershipEntities });
            return communityMembershipEntities.Select(e => _communityMembershipConverter.ConvertToModel(e)).ToList();
        }

        public async Task<List<CommunityMembership>> ListCommunityMembershipsAsync(long userId)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { userId });
            var sw = Stopwatch.StartNew();
            var communityMembershipEntities = await _communityMembershipEntities
                .AsNoTracking()
                .Where(e => e.UserEntityId == userId)
                .Include(
                    userEntity: false,
                    communityEntity: true
                )
                .Take(GlobalOperator.GlobalValues.Pagination.TotalItemsCount)
                .ToListAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { communityMembershipEntities });
            return communityMembershipEntities.Select(e => _communityMembershipConverter.ConvertToModel(e)).ToList();
        }

        public async Task<List<long>> ListCommunityMembershipIdsAsync(long userId)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { userId });
            var sw = Stopwatch.StartNew();
            var communityMembershipEntities = await _communityMembershipEntities
                .AsNoTracking()
                .Where(e => e.UserEntityId == userId)
                .Include(
                    userEntity: false,
                    communityEntity: true
                )
                .Take(GlobalOperator.GlobalValues.Pagination.TotalItemsCount)
                .Select(e => e.Id.Value)
                .ToListAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { communityMembershipEntities });
            return communityMembershipEntities;
        }

        public async Task<CommunityMembership> GetCommunityMembershipByIdAsync(long id,
            bool includeUser = false, bool includeCommunity = false)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { id, includeUser, 
                includeCommunity });
            var sw = Stopwatch.StartNew();
            var communityMembershipEntity = await _communityMembershipEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Include(
                    userEntity: includeUser,
                    communityEntity: includeCommunity
                )
                .SingleOrDefaultAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { communityMembershipEntity });
            return _communityMembershipConverter.ConvertToModel(communityMembershipEntity);
        }

        public async Task<CommunityMembership> CreateCommunityMembershipAsync(long userId,
            string username, long communityId, string communityName)
        {
            _logger.LogIOInformation(null, "Database | Iutput",
                new { userId, username, communityId, communityName });
            var sw = Stopwatch.StartNew();
            var communityMembershipEntity = new CommunityMembershipEntity(userId, username,
                communityId, communityName);
            _communityMembershipEntities.Add(communityMembershipEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database | Output", new { communityMembershipEntity });
            return _communityMembershipConverter.ConvertToModel(communityMembershipEntity);
        }

        public async Task<CommunityMembership> UpdateCommunityMembershipAsync(
            CommunityMembership communityMembership)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { communityMembership });
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

            _logger.LogIOInformation(sw, "Database | Output", new { communityMembershipEntity });
            return _communityMembershipConverter.ConvertToModel(communityMembershipEntity);
        }

        public async Task DeleteCommunityMembershipByIdAsync(long id)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { id });
            var sw = Stopwatch.StartNew();
            var communityMembershipEntity = await _communityMembershipEntities
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync();

            _communityMembershipEntities.Remove(communityMembershipEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();

            _logger.LogIOInformation(sw, "Database | Output", new { communityMembershipEntity });
        }

        public async Task<bool> DoesCommunityMembershipIdExistAsync(long id)
        {
            _logger.LogIOInformation(null, "Database | Iutput", new { id });
            var sw = Stopwatch.StartNew();
            var doesExist = await _communityMembershipEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .AnyAsync();

            _logger.LogIOInformation(sw, "Database | Output", new { doesExist });
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
            long? userId, long? communityId)
        {
            if (userId.HasValue)
                q = q.Where(e => e.UserEntityId == userId.Value);

            if (communityId.HasValue)
                q = q.Where(e => e.CommunityEntityId == communityId.Value);

            return q;
        }
    }
}
