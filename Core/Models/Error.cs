using Microsoft.Extensions.DependencyInjection;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Enums;

namespace FireplaceApi.Core.Models
{
    public class Error
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public int Id { get; set; }
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
            httpStatusCode: 500
        );

        public Error(int id, ErrorName name, int code, string clientMessage, int httpStatusCode, 
            string serverMessage = null, Exception exception = null)
        {
            Id = id;
            Name = name;
            Code = code;
            ClientMessage = clientMessage ?? throw new ArgumentNullException(nameof(clientMessage));
            HttpStatusCode = httpStatusCode;
            ServerMessage = serverMessage;
            Exception = exception;
        }

        public Error PureCopy() => new Error(Id, Name, Code, ClientMessage, HttpStatusCode, ServerMessage, Exception);

        public void RemoveLoopReferencing()
        {

        }
    }
}
