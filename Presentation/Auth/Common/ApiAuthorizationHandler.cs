using FireplaceApi.Domain.Errors;

namespace FireplaceApi.Presentation.Auth;

public interface IApiAuthorizationHandler
{
    public ApiException ApiException { get; }
}