﻿using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Tools;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Operators
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

        public async Task<List<RequestTrace>> ListRequestTracesAsync(string method,
            string action, string url, IPAddress ip, string country, string userAgentSearch, ulong? userId,
            int? statusCode, long? fromDuration, ErrorName? errorName, DateTime? fromDate)
        {
            var requestTrace = await _requestTraceRepository
                .ListRequestTracesAsync(method, action, url, ip, country, userAgentSearch,
                userId, statusCode, fromDuration, errorName, fromDate);
            return requestTrace;
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
            int statusCode, long duration, string action = null, ErrorName? errorName = null)
        {
            var id = await IdGenerator.GenerateNewIdAsync(DoesRequestTraceIdExistAsync);
            var country = await FindCountryName(ip);
            var requestTrace = await _requestTraceRepository.CreateRequestTraceAsync(id,
                method, url, ip, country, userAgent, userId, statusCode, duration,
                action, errorName);
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