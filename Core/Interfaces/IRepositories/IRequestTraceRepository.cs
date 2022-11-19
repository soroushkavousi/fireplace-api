using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Interfaces
{
    public interface IRequestTraceRepository
    {
        public Task<List<RequestTrace>> ListRequestTracesAsync(string method,
            string action, string url, IPAddress ip, string country, string userAgentSearch,
            ulong? userId, int? statusCode, long? fromDuration, ErrorName? errorName, DateTime? fromDate);
        public Task<RequestTrace> GetRequestTraceByIdAsync(ulong id);
        public Task<RequestTrace> CreateRequestTraceAsync(ulong id, string method,
            string url, IPAddress ip, string country, string userAgent, ulong? userId,
            int statusCode, long duration, string action = null, ErrorName? errorName = null);
        public Task<RequestTrace> UpdateRequestTraceAsync(RequestTrace requestTrace);
        public Task DeleteRequestTraceAsync(ulong id);
        public Task<bool> DoesRequestTraceIdExistAsync(ulong id);
    }
}
