using FireplaceApi.Application.Exceptions;
using FireplaceApi.Application.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FireplaceApi.Presentation.Auth;

public class SessionAuthorizationRequirement : IAuthorizationRequirement
{
    public static readonly SessionAuthorizationRequirement Instance = new();
}

public class SessionAuthorizationHandler : AuthorizationHandler<SessionAuthorizationRequirement>,
    IApiAuthorizationHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly SessionValidator _sessionValidator;
    public ApiException ApiException { get; private set; }

    public SessionAuthorizationHandler(IHttpContextAccessor httpContextAccessor,
        SessionValidator sessionValidator)
    {
        _httpContextAccessor = httpContextAccessor;
        _sessionValidator = sessionValidator;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        SessionAuthorizationRequirement requirement)
    {
        try
        {
            if (context.HasFailed)
                return;

            var requestinUser = _httpContextAccessor.HttpContext.GetRequestingUser();
            if (requestinUser == null || !requestinUser.SessionId.HasValue)
                return;

            await _sessionValidator.ValidateSessionIsActiveAsync(requestinUser.SessionId.Value);
        }
        catch (ApiException apiException)
        {
            ApiException = apiException;
            var reason = new AuthorizationFailureReason(this, ApiException.Message);
            context.Fail(reason);
            return;
        }
        context.Succeed(requirement);
    }
}