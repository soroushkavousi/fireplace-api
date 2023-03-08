using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Interfaces;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Tools;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Operators
{
    public class RequestTraceOperator
    {
        private readonly ILogger<RequestTraceOperator> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IRequestTraceRepository _requestTraceRepository;

        public RequestTraceOperator(ILogger<RequestTraceOperator> logger,
            IServiceProvider serviceProvider, IRequestTraceRepository requestTraceRepository)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _requestTraceRepository = requestTraceRepository;
        }

        public async Task<string> FindCountryName(IPAddress ip)
        {
            await Task.CompletedTask;
            return "TBD";
        }

        public async Task<List<RequestTrace>> ListRequestTracesAsync(string method = null,
            string action = null, string url = null, IPAddress ip = null, string country = null,
            string userAgentSearch = null, ulong? userId = null, int? statusCode = null,
            long? fromDuration = null, ErrorType errorType = null, FieldName errorField = null,
            DateTime? fromDate = null, bool? withAction = null)
        {
            var requestTraces = await _requestTraceRepository
                .ListRequestTracesAsync(method, action, url, ip, country, userAgentSearch,
                userId, statusCode, fromDuration, errorType, errorField, fromDate, withAction);
            return requestTraces;
        }

        public async Task<int> CountRequestTracesAsync(string method = null,
            string action = null, string url = null, IPAddress ip = null, string country = null,
            string userAgentSearch = null, ulong? userId = null, int? statusCode = null,
            long? fromDuration = null, ErrorType errorType = null, FieldName errorField = null,
            DateTime? fromDate = null, bool? withAction = null)
        {
            var count = await _requestTraceRepository
                .CountRequestTracesAsync(method, action, url, ip, country, userAgentSearch,
                userId, statusCode, fromDuration, errorType, errorField, fromDate, withAction);
            return count;
        }

        public async Task<RequestTrace> GetRequestTraceByIdAsync(ulong id)
        {
            var requestTrace = await _requestTraceRepository
                .GetRequestTraceByIdAsync(id);
            if (requestTrace == null)
                return requestTrace;

            return requestTrace;
        }

        public async Task<RequestTrace> CreateRequestTraceAsync(string method,
            string url, IPAddress ip, string userAgent, ulong? userId,
            int statusCode, long duration, string action = null,
            ErrorType errorType = null, FieldName errorField = null)
        {
            var id = await IdGenerator.GenerateNewIdAsync(DoesRequestTraceIdExistAsync);
            var country = await FindCountryName(ip);
            var requestTrace = await _requestTraceRepository.CreateRequestTraceAsync(id,
                method, url, ip, country, userAgent, userId, statusCode, duration,
                action, errorType, errorField);
            return requestTrace;
        }

        public async Task DeleteRequestTraceAsync(ulong id)
        {
            await _requestTraceRepository.DeleteRequestTraceAsync(id);
        }

        public async Task<bool> DoesRequestTraceIdExistAsync(ulong id)
        {
            var requestTraceIdExists = await _requestTraceRepository.DoesRequestTraceIdExistAsync(id);
            return requestTraceIdExists;
        }
    }
}
