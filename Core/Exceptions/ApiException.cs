using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Models;

namespace FireplaceApi.Core.Exceptions
{
    public class ApiException : Exception
    {
        public ErrorName ErrorName { get; set; }
        public string ErrorServerMessage { get; set; }
        public Exception Exception { get; set; }

        public ApiException(ErrorName errorName, string errorServerMessage = null, Exception systemException = null) : base(errorServerMessage, systemException)
        {
            ErrorName = errorName;
            ErrorServerMessage = errorServerMessage;
            Exception = systemException ?? this;
        }
    }
}
