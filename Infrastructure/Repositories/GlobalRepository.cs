﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using FireplaceApi.Infrastructure.Converters;
using FireplaceApi.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.ValueObjects;
using FireplaceApi.Core.Interfaces;
using System.Diagnostics;

namespace FireplaceApi.Infrastructure.Repositories
{
    public class GlobalRepository : IGlobalRepository
    {
        private readonly ILogger<GlobalRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly FireplaceApiContext _fireplaceApiContext;
        private readonly DbSet<GlobalEntity> _globalEntities;
        private readonly GlobalConverter _globalConverter;

        public GlobalRepository(ILogger<GlobalRepository> logger, 
            IConfiguration configuration, FireplaceApiContext fireplaceApiContext,
            GlobalConverter globalConverter)
        {
            _logger = logger;
            _configuration = configuration;
            _fireplaceApiContext = fireplaceApiContext;
            _globalEntities = fireplaceApiContext.GlobalEntities;
            _globalConverter = globalConverter;
        }

        public async Task<List<Global>> ListGlobalsAsync()
        {
            var sw = Stopwatch.StartNew();
            var globalEntites = await _globalEntities
                .AsNoTracking()
                .Include(
                )
                .ToListAsync();

            _logger.LogIOInformation(sw, "Database", null, new { globalEntites });
            return globalEntites.Select(e => _globalConverter.ConvertToModel(e)).ToList();
        }

        public async Task<Global> GetGlobalByIdAsync(GlobalId globalId)
        {
            var sw = Stopwatch.StartNew();
            var globalEntity = await _globalEntities
                .AsNoTracking()
                .Where(e => e.Id == globalId.To<int>())
                .Include(
                )
                .SingleOrDefaultAsync();

            _logger.LogIOInformation(sw, "Database", new { globalId }, new { globalEntity });
            return _globalConverter.ConvertToModel(globalEntity);
        }

        public async Task<Global> CreateGlobalAsync(GlobalId globalId, GlobalValues globalValues)
        {
            var sw = Stopwatch.StartNew();
            var globalEntity = new GlobalEntity(globalId.To<int>(), globalValues);
            _globalEntities.Add(globalEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();
            
            _logger.LogIOInformation(sw, "Database", new { globalId, globalValues }, new { globalEntity });
            return _globalConverter.ConvertToModel(globalEntity);
        }

        public async Task<Global> UpdateGlobalAsync(Global global)
        {
            var sw = Stopwatch.StartNew();
            var globalEntity = _globalConverter.ConvertToEntity(global);
            _globalEntities.Update(globalEntity);
            try
            {
                await _fireplaceApiContext.SaveChangesAsync();
                _fireplaceApiContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var serverMessage = $"Can't update the globalEntity DbUpdateConcurrencyException. {globalEntity.ToJson()}";
                throw new ApiException(ErrorName.INTERNAL_SERVER, serverMessage, systemException: ex);
            }
            
            _logger.LogIOInformation(sw, "Database", new { global }, new { globalEntity });
            return _globalConverter.ConvertToModel(globalEntity);
        }

        public async Task DeleteGlobalAsync(GlobalId globalId)
        {
            var sw = Stopwatch.StartNew();
            var globalEntity = await _globalEntities
                .Where(e => e.Id == globalId.To<int>())
                .SingleOrDefaultAsync();

            _globalEntities.Remove(globalEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();
        
            _logger.LogIOInformation(sw, "Database", new { globalId }, new { globalEntity });
        }

        public async Task<bool> DoesGlobalIdExistAsync(GlobalId globalId)
        {
            var sw = Stopwatch.StartNew();
            var doesExist = await _globalEntities
                .AsNoTracking()
                .Where(e => e.Id == globalId.To<int>())
                .AnyAsync();
        
            _logger.LogIOInformation(sw, "Database", new { globalId }, new { doesExist });
            return doesExist;
        }
    }

    public static class GlobalRepositoryExtensions
    {
        public static IQueryable<GlobalEntity> Include(
                    [NotNull] this IQueryable<GlobalEntity> globalEntitiesQuery)
        {
            return globalEntitiesQuery;
        }
    }
}
