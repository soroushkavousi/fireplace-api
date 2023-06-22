using FireplaceApi.Application.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireplaceApi.Presentation.Auth;

public class ApiAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();

    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext httpContext,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        await _defaultHandler.HandleAsync(next, httpContext, policy, authorizeResult);
        var requestingUser = httpContext.GetRequestingUser();
        if (authorizeResult.Challenged)
        {
            throw new AccessTokenAuthenticationFailedException();
        }
        else if (authorizeResult.Forbidden)
        {
            authorizeResult.AuthorizationFailure?.FailedRequirements
                .ThrowExceptionForFailedRquirements(requestingUser, httpContext);
            authorizeResult.AuthorizationFailure?.FailureReasons
                .ThrowExceptionForFailedReasons();
        }
    }
}

public static class ApiAuthorizationMiddlewareResultHandlerExtensions
{
    public static void ThrowExceptionForFailedRquirements(
        this IEnumerable<IAuthorizationRequirement> failedRequirements,
        RequestingUser requestingUser, HttpContext httpContext)
    {
        if (failedRequirements == null || !failedRequirements.Any())
            return;

        var failedRequirement = failedRequirements.ElementAt(0);
        switch (failedRequirement)
        {
            case RolesAuthorizationRequirement roleRequirement:
                throw new RoleAccessDeniedException(roleRequirement.AllowedRoles,
                    requestingUser.Id.Value, requestingUser.Roles);
            case ClaimsAuthorizationRequirement claimsRequirement:
                claimsRequirement.ThrowExceptionForFailedClaim(httpContext);
                break;
        }
    }

    private static void ThrowExceptionForFailedClaim(
        this ClaimsAuthorizationRequirement claimsRequirement,
        HttpContext httpContext)
    {
        string actualValue = httpContext.User.Claims.SingleOrDefault(c => c.Type == claimsRequirement.ClaimType)?.Value;
        throw claimsRequirement.ClaimType switch
        {
            AuthConstants.UserStateClaimKey => new UnverifiedUserAccessDeniedException(
                claimsRequirement.AllowedValues, actualValue),

            _ => new AccessTokenAccessDeniedException(claimsRequirement.ClaimType,
                claimsRequirement.AllowedValues, actualValue),
        };
    }

    public static void ThrowExceptionForFailedReasons(this IEnumerable<AuthorizationFailureReason> failureReasons)
    {
        if (failureReasons == null || !failureReasons.Any())
            return;

        var failureReason = failureReasons.ElementAt(0);
        var handler = failureReason.Handler.To<IApiAuthorizationHandler>();
        throw handler.ApiException;
    }
}
