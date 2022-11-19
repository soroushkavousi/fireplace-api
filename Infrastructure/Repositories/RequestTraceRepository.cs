using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Models;
using FireplaceApi.Infrastructure.Converters;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FireplaceApi.Infrastructure.Repositories
{
    public class RequestTraceRepository : IRequestTraceRepository
    {
        private readonly ILogger<RequestTraceRepository> _logger;
        private readonly FireplaceApiDbContext _dbContext;
        private readonly DbSet<RequestTraceEntity> _requestTraceEntities;
        private readonly RequestTraceConverter _requestTraceConverter;

        public RequestTraceRepository(ILogger<RequestTraceRepository> logger,
            FireplaceApiDbContext dbContext, RequestTraceConverter requestTraceConverter)
        {
            _logger = logger;
            _dbContext = dbContext;
            _requestTraceEntities = dbContext.RequestTraceEntities;
            _requestTraceConverter = requestTraceConverter;
        }

        public async Task<List<RequestTrace>> ListRequestTracesAsync(string method,
            string action, string url, IPAddress ip, string country, string userAgentSearch,
            ulong? userId, int? statusCode, long? fromDuration, ErrorName? errorName, DateTime? fromDate)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT",
                parameters: new
                {
                    method,
                    action,
                    url,
                    ip = ip?.ToString(),
                    country,
                    userAgentSearch,
                    userId,
                    statusCode,
                    fromDuration,
                    errorName,
                    fromDate
                });
            var sw = Stopwatch.StartNew();

            var requestTraceEntities = await _requestTraceEntities
                .AsNoTracking()
                .Search(
                    method: method,
                    action: action,
                    url: url,
                    ip: ip,
                    country: country,
                    userAgentSearch: userAgentSearch,
                    userId: userId,
                    statusCode: statusCode,
                    fromDuration: fromDuration,
                    errorName: errorName,
                    fromDate: fromDate
                )
                .Take(Configs.Current.QueryResult.TotalLimit)
                .ToListAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { requestTraceEntities });
            return requestTraceEntities.Select(e => _requestTraceConverter.ConvertToModel(e)).ToList();
        }

        public async Task<RequestTrace> GetRequestTraceByIdAsync(ulong id)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { id });
            var sw = Stopwatch.StartNew();
            var requestTraceEntity = await _requestTraceEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { requestTraceEntity });
            return _requestTraceConverter.ConvertToModel(requestTraceEntity);
        }

        public async Task<RequestTrace> CreateRequestTraceAsync(ulong id, string method,
            string url, IPAddress ip, string country, string userAgent, ulong? userId,
            int statusCode, long duration, string action = null, ErrorName? errorName = null)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT",
                parameters: new
                {
                    id,
                    method,
                    url,
                    ip = ip?.ToString(),
                    country,
                    userAgent,
                    userId,
                    statusCode,
                    duration,
                    action,
                    errorName
                });
            var sw = Stopwatch.StartNew();
            var requestTraceEntity = new RequestTraceEntity(id, method, url,
                ip.ToString(), country, userAgent, userId, statusCode, duration,
                action, errorName.ToString());
            _requestTraceEntities.Add(requestTraceEntity);
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachAllEntries();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { requestTraceEntity });
            return _requestTraceConverter.ConvertToModel(requestTraceEntity);
        }

        public async Task<RequestTrace> UpdateRequestTraceAsync(RequestTrace requestTrace)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { requestTrace });
            var sw = Stopwatch.StartNew();
            var requestTraceEntity = _requestTraceConverter.ConvertToEntity(requestTrace);
            _requestTraceEntities.Update(requestTraceEntity);
            try
            {
                await _dbContext.SaveChangesAsync();
                _dbContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var serverMessage = $"Can't update the requestTraceEntity DbUpdateConcurrencyException. {requestTraceEntity.ToJson()}";
                throw new ApiException(ErrorName.INTERNAL_SERVER, serverMessage, systemException: ex);
            }

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { requestTraceEntity });
            return _requestTraceConverter.ConvertToModel(requestTraceEntity);
        }

        public async Task DeleteRequestTraceAsync(ulong id)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { id });
            var sw = Stopwatch.StartNew();
            var requestTraceEntity = await _requestTraceEntities
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync();

            _requestTraceEntities.Remove(requestTraceEntity);
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachAllEntries();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { requestTraceEntity });
        }

        public async Task<bool> DoesRequestTraceIdExistAsync(ulong id)
        {
            _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { id });
            var sw = Stopwatch.StartNew();
            var doesExist = await _requestTraceEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .AnyAsync();

            _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { doesExist });
            return doesExist;
        }
    }

    public static class RequestTraceRepositoryExtensions
    {
        public static IQueryable<RequestTraceEntity> Search(
            [NotNull] this IQueryable<RequestTraceEntity> q, string method,
            string action, string url, IPAddress ip, string country, string userAgentSearch, ulong? userId,
            int? statusCode, long? fromDuration, ErrorName? errorName, DateTime? fromDate)
        {
            if (method != null)
                q = q.Where(e => e.Method == method);

            if (action != null)
                q = q.Where(e => e.Action == action);

            if (url != null)
                q = q.Where(e => e.Url == url);

            if (ip != null)
                q = q.Where(e => e.IP == ip.ToString());

            if (country != null)
                q = q.Where(e => e.Country == country);

            if (userAgentSearch != null)
                q = q.Where(e => EF.Functions
                    .ILike(EF.Functions.Collate(e.UserAgent, "default"), $"%{userAgentSearch}%"));

            if (userId != null)
                q = q.Where(e => e.UserId == userId);

            if (statusCode != null)
                q = q.Where(e => e.StatusCode == statusCode);

            if (fromDuration != null)
                q = q.Where(e => e.Duration >= fromDuration);

            if (errorName != null)
                q = q.Where(e => e.ErrorName == errorName.ToString());

            if (fromDate != null)
                q = q.Where(e => e.CreationDate > fromDate);

            return q;
        }
    }
}
