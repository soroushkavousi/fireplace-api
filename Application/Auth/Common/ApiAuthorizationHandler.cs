using FireplaceApi.Domain.Exceptions;

namespace FireplaceApi.Application.Auth;

public interface IApiAuthorizationHandler
{
    public ApiException ApiException { get; }
}