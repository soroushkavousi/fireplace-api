using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Infrastructure.Entities
{
    public class RequestTraceEntity : BaseEntity
    {
        [Required]
        public string Method { get; set; }
        public string Action { get; set; }
        [Required]
        public string IP { get; set; }
        [Required]
        public string Country { get; set; }
        public ulong? UserId { get; set; }
        [Required]
        public long Duration { get; set; }
        [Required]
        public int StatusCode { get; set; }
        public string ErrorName { get; set; }
        [Required]
        public string Url { get; set; }
        public string UserAgent { get; set; }

        private RequestTraceEntity() : base() { }

        public RequestTraceEntity(ulong id, string method, string url, string ip,
            string country, string userAgent, ulong? userId, int statusCode, long duration,
            string action = null, string errorName = null, DateTime? creationDate = null,
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
            ErrorName = errorName;
        }

        public RequestTraceEntity PureCopy() => new(Id, Method, Url, IP, Country,
            UserAgent, UserId, StatusCode, Duration, Action, ErrorName,
            CreationDate, ModifiedDate);
    }

    public class RequestTraceEntityConfiguration : IEntityTypeConfiguration<RequestTraceEntity>
    {
        public void Configure(EntityTypeBuilder<RequestTraceEntity> modelBuilder)
        {
            // p => principal / d => dependent

            modelBuilder.DoBaseConfiguration();
        }
    }
}
