using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Models;
using System.Net;

namespace FireplaceApi.Domain.Exceptions
{
    public class MaxRequestPerIpLimitException : ApiException
    {
        public MaxRequestPerIpLimitException(IPAddress ip, int requestCount)
            : base(
                errorType: ErrorType.LIMITATION,
                errorField: FieldName.MAX_REQUEST_PER_IP,
                errorServerMessage: "Max request limit reached for ip!",
                parameters: new { ip, requestCount, limitCount = Configs.Current.Api.MaxRequestPerIP },
                systemException: null
            )
        { }
    }
}
