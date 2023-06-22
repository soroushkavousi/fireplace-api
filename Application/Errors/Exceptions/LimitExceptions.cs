using FireplaceApi.Domain.Configurations;
using FireplaceApi.Domain.Errors;
using System.Net;

namespace FireplaceApi.Application.Errors;

public class MaxRequestPerIpLimitException : ApiException
{
    public MaxRequestPerIpLimitException(IPAddress ip, int requestCount)
        : base(
            errorType: ApplicationErrorType.LIMITATION,
            errorField: FieldName.MAX_REQUEST_PER_IP,
            errorServerMessage: "Max request limit reached for ip!",
            parameters: new { ip, requestCount, limitCount = Configs.Current.Api.MaxRequestPerIP },
            systemException: null
        )
    { }
}
