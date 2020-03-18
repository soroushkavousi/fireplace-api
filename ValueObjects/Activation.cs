using GamingCommunityApi.Entities.UserInformationEntities;
using GamingCommunityApi.Enums;
using GamingCommunityApi.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingCommunityApi.ValueObjects
{
    public class Activation
    {
        public long Code { get; set; }
        public ActivationStatus Status { get; set; }
        public string Message { get; set; }

        public Activation(long code, ActivationStatus status, string message = null)
        {
            Code = code;
            Status = status;
            Message = message;
        }
    }
}
