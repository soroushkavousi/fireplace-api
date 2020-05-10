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
        public long Code { get; set; }
        public ActivationStatus Status { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }

        public Activation(long code, ActivationStatus status, 
            string subject = null, string message = null)
        {
            Code = code;
            Status = status;
            Subject = subject;
            Message = message;
        }
    }
}
