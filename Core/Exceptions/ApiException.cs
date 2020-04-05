﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GamingCommunityApi.Core.Enums;
using GamingCommunityApi.Core.Extensions;
using GamingCommunityApi.Core.Models;

namespace GamingCommunityApi.Core.Exceptions
{
    public class ApiException : Exception
    {
        //public Error Error { get; set; }
        public ErrorName ErrorId { get; set; }
        public string ErrorServerMessage { get; set; }
        public Exception Exception { get; set; }

        public ApiException(ErrorName errorId, string errorServerMessage = null, Exception systemException = null) : base(errorServerMessage, systemException)
        {
            ErrorId = errorId;
            ErrorServerMessage = errorServerMessage;
            Exception = systemException ?? this;
            //Error = new Error(errorId)
            //{
            //    Id = errorId.To<int>(),
            //    Field = field,
            //    ServerMessage = errorServerMessage,
            //    Exception = systemException ?? this
            //};
        }
    }
}