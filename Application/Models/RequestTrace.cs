using FireplaceApi.Application.Enums;
using System;
using System.Net;

namespace FireplaceApi.Application.Models;

public class RequestTrace : BaseModel
{
    public string Method { get; set; }
    public string Action { get; set; }
    public string Url { get; set; }
    public IPAddress IP { get; set; }
    public string Country { get; set; }
    public string UserAgent { get; set; }
    public ulong? UserId { get; set; }
    public int StatusCode { get; set; }
    public long Duration { get; set; }
    public ErrorType ErrorType { get; set; }
    public FieldName ErrorField { get; set; }

    public RequestTrace(ulong id, string method, string url, IPAddress ip,
        string country, string userAgent, ulong? userId, int statusCode, long duration,
        string action, ErrorType errorType, FieldName fieldName, DateTime creationDate,
        DateTime? modifiedDate = null) : base(id, creationDate, modifiedDate)
    {
        Method = method ?? throw new ArgumentNullException(nameof(method));
        Url = url ?? throw new ArgumentNullException(nameof(url));
        IP = ip ?? throw new ArgumentNullException(nameof(ip));
        Country = country ?? throw new ArgumentNullException(nameof(country));
        UserAgent = userAgent ?? throw new ArgumentNullException(nameof(userAgent));
        UserId = userId;
        StatusCode = statusCode;
        Duration = duration;
        Action = action;
        ErrorType = errorType;
        ErrorField = fieldName;
    }

    public RequestTrace PureCopy() => new(Id, Method, Url,
        IP, Country, UserAgent, UserId, StatusCode, Duration,
        Action, ErrorType, ErrorField, CreationDate, ModifiedDate);
}
