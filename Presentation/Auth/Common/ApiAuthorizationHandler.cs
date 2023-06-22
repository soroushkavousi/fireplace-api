using FireplaceApi.Application.Exceptions;

namespace FireplaceApi.Presentation.Auth;

public interface IApiAuthorizationHandler
{
    public ApiException ApiException { get; }
}