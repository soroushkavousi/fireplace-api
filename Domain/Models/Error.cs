using FireplaceApi.Domain.Enums;
using NLog;
using System;
using System.Text.Json.Serialization;

namespace FireplaceApi.Domain.Models
{
    public class Error : BaseModel
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public ErrorName Name { get; set; }
        public int Code { get; set; }
        public string ClientMessage { get; set; }
        public int HttpStatusCode { get; set; }
        public string ServerMessage { get; set; }
        [JsonIgnore]
        public Exception Exception { get; set; }
        //public string Field { get; set; }

        public static Error InternalServerError { get; } = new Error
        (
            id: 37,
            name: ErrorName.INTERNAL_SERVER,
            code: 5000,
            clientMessage: "Something Went Wrong!",
            httpStatusCode: 500,
            creationDate: DateTime.UtcNow
        );

        public Error(ulong id, ErrorName name, int code,
            string clientMessage, int httpStatusCode,
            DateTime creationDate, DateTime? modifiedDate = null,
            string serverMessage = null, Exception exception = null)
            : base(id, creationDate, modifiedDate)
        {
            Name = name;
            Code = code;
            ClientMessage = clientMessage ?? throw new ArgumentNullException(nameof(clientMessage));
            HttpStatusCode = httpStatusCode;
            ServerMessage = serverMessage;
            Exception = exception;
        }

        public Error PureCopy() => new Error(Id, Name, Code,
            ClientMessage, HttpStatusCode, CreationDate, ModifiedDate,
            ServerMessage, Exception);

        public void RemoveLoopReferencing()
        {

        }
    }
}
