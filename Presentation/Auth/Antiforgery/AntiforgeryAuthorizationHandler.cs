using FireplaceApi.Application.Errors;
using FireplaceApi.Domain.Errors;
using FireplaceApi.Presentation.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;

namespace FireplaceApi.Presentation.Auth;

public class AntiforgeryAuthorizationRequirement : IAuthorizationRequirement
{
    public static readonly AntiforgeryAuthorizationRequirement Instance = new();
}

public class AntiforgeryAuthorizationHandler : AuthorizationHandler<AntiforgeryAuthorizationRequirement>,
    IApiAuthorizationHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public ApiException ApiException { get; private set; }

    public AntiforgeryAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        AntiforgeryAuthorizationRequirement requirement)
    {
        try
        {
            ValidateCsrfToken(_httpContextAccessor.HttpContext);
        }
        catch (ApiException apiException)
        {
            ApiException = apiException;
            var reason = new AuthorizationFailureReason(this, ApiException.Message);
            context.Fail(reason);
            return Task.CompletedTask;
        }
        context.Succeed(requirement);
        return Task.CompletedTask;
    }

    private static void ValidateCsrfToken(HttpContext httpContext)
    {
        var requestPath = httpContext.Request.Path.Value;
        if (requestPath.StartsWith(Tools.Constants.GraphQLBaseRoute))
            return;

        var isUserEndpoint = httpContext.GetActionAttribute<IAllowAnonymous>() == null;
        if (!isUserEndpoint)
            return;

        if (httpContext.Request.Method.IsSafeHttpMethod())
            return;

        httpContext.Request.Cookies.TryGetValue(AntiforgeryConstants.CsrfTokenKey, out string cookieCsrfToken);
        if (string.IsNullOrWhiteSpace(cookieCsrfToken))
            return;

        httpContext.Request.Headers.TryGetValue(AntiforgeryConstants.CsrfTokenKey, out StringValues headerCsrfTokenStringValues);
        var headerCsrfToken = headerCsrfTokenStringValues.ToString();
        if (headerCsrfToken == cookieCsrfToken)
            return;

        throw new CsrfTokenAccessDeniedException(headerCsrfToken, cookieCsrfToken);
    }
}