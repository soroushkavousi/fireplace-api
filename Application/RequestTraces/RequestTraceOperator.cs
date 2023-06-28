using FireplaceApi.Domain.Errors;
using FireplaceApi.Domain.RequestTraces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace FireplaceApi.Application.RequestTraces;

public class RequestTraceOperator
{
    private readonly IServerLogger<RequestTraceOperator> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IRequestTraceRepository _repository;
    private readonly IRequestTraceCacheService _cacheService;

    public RequestTraceOperator(IServerLogger<RequestTraceOperator> logger,
        IServiceProvider serviceProvider, IRequestTraceRepository repository,
        IRequestTraceCacheService cacheService)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _repository = repository;
        _cacheService = cacheService;
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
        var requestTraces = await _repository
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
        var count = await _repository
            .CountRequestTracesAsync(method, action, url, ip, country, userAgentSearch,
            userId, statusCode, fromDuration, errorType, errorField, fromDate, withAction);
        return count;
    }

    public async Task<int> CountIpRequestTimesAsync(IPAddress ip)
    {
        return await _cacheService.CoutIpRequestTimesAsync(ip);
    }

    public async Task<RequestTrace> GetRequestTraceByIdAsync(ulong id)
    {
        var requestTrace = await _repository
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
        var country = await FindCountryName(ip);
        var requestTrace = await _repository.CreateRequestTraceAsync(
            method, url, ip, country, userAgent, userId, statusCode, duration,
            action, errorType, errorField);

        var errorIsTheIpLimitation =
            errorType != null
            && errorType == ApplicationErrorType.LIMITATION
            && errorField == FieldName.MAX_REQUEST_PER_IP;
        if (!string.IsNullOrWhiteSpace(action) && !ip.IsLocalIpAddress() && !errorIsTheIpLimitation)
            await _cacheService.AddIpRequestTimeAsync(ip, Configs.Current.Api.RequestLimitionPeriod);

        return requestTrace;
    }

    public async Task DeleteRequestTraceAsync(ulong id)
    {
        await _repository.DeleteRequestTraceAsync(id);
    }

    public async Task<bool> DoesRequestTraceIdExistAsync(ulong id)
    {
        var requestTraceIdExists = await _repository.DoesRequestTraceIdExistAsync(id);
        return requestTraceIdExists;
    }
}
