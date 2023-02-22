﻿using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Models;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace FireplaceApi.Infrastructure.Converters
{
    public class RequestTraceConverter
    {
        private readonly ILogger<RequestTraceConverter> _logger;
        private readonly IServiceProvider _serviceProvider;

        public RequestTraceConverter(ILogger<RequestTraceConverter> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public RequestTraceEntity ConvertToEntity(RequestTrace requestTrace)
        {
            if (requestTrace == null)
                return null;


            var requestTraceEntity = new RequestTraceEntity(requestTrace.Id, requestTrace.Method,
                requestTrace.Url, requestTrace.IP.ToString(), requestTrace.Country, requestTrace.UserAgent,
                requestTrace.UserId, requestTrace.StatusCode, requestTrace.Duration, requestTrace.Action,
                requestTrace.ErrorName?.ToString(), requestTrace.CreationDate, requestTrace.ModifiedDate);

            return requestTraceEntity;
        }

        public RequestTrace ConvertToModel(RequestTraceEntity requestTraceEntity)
        {
            if (requestTraceEntity == null)
                return null;

            ErrorName? errorName = string.IsNullOrWhiteSpace(requestTraceEntity.ErrorName)
                ? null : requestTraceEntity.ErrorName.ToEnum<ErrorName>();

            var requestTrace = new RequestTrace(requestTraceEntity.Id, requestTraceEntity.Method,
                requestTraceEntity.Url, IPAddress.Parse(requestTraceEntity.IP),
                requestTraceEntity.Country, requestTraceEntity.UserAgent, requestTraceEntity.UserId,
                requestTraceEntity.StatusCode, requestTraceEntity.Duration,
                requestTraceEntity.Action, errorName,
                requestTraceEntity.CreationDate, requestTraceEntity.ModifiedDate);

            return requestTrace;
        }
    }
}
