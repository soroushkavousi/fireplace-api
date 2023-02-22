using FireplaceApi.Domain.Enums;
using System;
using System.Net;

namespace FireplaceApi.Domain.Models
{
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
        public ErrorName? ErrorName { get; set; }

        public RequestTrace(ulong id, string method, string url, IPAddress ip,
            string country, string userAgent, ulong? userId, int statusCode, long duration,
            string action, ErrorName? errorName, DateTime creationDate, DateTime? modifiedDate = null)
            : base(id, creationDate, modifiedDate)
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
            ErrorName = errorName;
        }

        public RequestTrace PureCopy() => new(Id, Method, Url,
            IP, Country, UserAgent, UserId, StatusCode, Duration,
            Action, ErrorName, CreationDate, ModifiedDate);
    }
}
