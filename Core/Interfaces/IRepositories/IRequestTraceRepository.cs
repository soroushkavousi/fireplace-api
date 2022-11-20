﻿using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Interfaces
{
    public interface IRequestTraceRepository
    {
        public Task<List<RequestTrace>> ListRequestTracesAsync(string method = null,
            string action = null, string url = null, IPAddress ip = null, string country = null,
            string userAgentSearch = null, ulong? userId = null, int? statusCode = null,
            long? fromDuration = null, ErrorName? errorName = null, DateTime? fromDate = null,
            bool? withAction = null);
        public Task<int> CountRequestTracesAsync(string method = null,
            string action = null, string url = null, IPAddress ip = null, string country = null,
            string userAgentSearch = null, ulong? userId = null, int? statusCode = null,
            long? fromDuration = null, ErrorName? errorName = null, DateTime? fromDate = null,
            bool? withAction = null);
        public Task<RequestTrace> GetRequestTraceByIdAsync(ulong id);
        public Task<RequestTrace> CreateRequestTraceAsync(ulong id, string method,
            string url, IPAddress ip, string country, string userAgent, ulong? userId,
            int statusCode, long duration, string action = null, ErrorName? errorName = null);
        public Task<RequestTrace> UpdateRequestTraceAsync(RequestTrace requestTrace);
        public Task DeleteRequestTraceAsync(ulong id);
        public Task<bool> DoesRequestTraceIdExistAsync(ulong id);
    }
}
