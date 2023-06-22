using FireplaceApi.Domain.Errors;
using FireplaceApi.Domain.RequestTraces;
using FireplaceApi.Infrastructure.Entities;
using FireplaceApi.Infrastructure.Enums;
using System.Net;

namespace FireplaceApi.Infrastructure.Converters;

public static class RequestTraceConverter
{
    public static RequestTraceEntity ToEntity(this RequestTrace requestTrace)
    {
        if (requestTrace == null)
            return null;


        var requestTraceEntity = new RequestTraceEntity(requestTrace.Id, requestTrace.Method,
            requestTrace.Url, requestTrace.IP.ToString(), requestTrace.Country, requestTrace.UserAgent,
            requestTrace.UserId, requestTrace.StatusCode, requestTrace.Duration, requestTrace.Action,
            requestTrace.ErrorType?.Name, requestTrace.ErrorField?.Name, requestTrace.CreationDate, requestTrace.ModifiedDate);

        return requestTraceEntity;
    }

    public static RequestTrace ToModel(this RequestTraceEntity requestTraceEntity)
    {
        if (requestTraceEntity == null)
            return null;

        ErrorType errorType = string.IsNullOrWhiteSpace(requestTraceEntity.ErrorType)
            ? null : InfrastructureErrorType.FromName(requestTraceEntity.ErrorType);

        FieldName errorField = string.IsNullOrWhiteSpace(requestTraceEntity.ErrorField)
            ? null : InfrastructureFieldName.FromName(requestTraceEntity.ErrorField);

        var requestTrace = new RequestTrace(requestTraceEntity.Id, requestTraceEntity.Method,
            requestTraceEntity.Url, IPAddress.Parse(requestTraceEntity.IP),
            requestTraceEntity.Country, requestTraceEntity.UserAgent, requestTraceEntity.UserId,
            requestTraceEntity.StatusCode, requestTraceEntity.Duration,
            requestTraceEntity.Action, errorType, errorField,
            requestTraceEntity.CreationDate, requestTraceEntity.ModifiedDate);

        return requestTrace;
    }
}
