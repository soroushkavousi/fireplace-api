using GamingCommunityApi.Core.Enums;
using GamingCommunityApi.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingCommunityApi.Core.ValueObjects
{
    public class Activation
    {
        public ActivationStatus Status { get; set; }
        public int? Code { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }

        public Activation(ActivationStatus status, int? code = null, 
            string subject = null, string message = null)
        {
            Status = status;
            Code = code;
            Subject = subject;
            Message = message;
        }
    }
}
