using FireplaceApi.Domain.Users;
using System.Collections.Generic;
using System.Security.Claims;

namespace FireplaceApi.Presentation.Auth;

public static class AuthConverters
{
    public static RequestingUser ToRequestingUser(this ClaimsPrincipal claimsPrincipal)
    {
        if (claimsPrincipal == null || claimsPrincipal.Claims.IsNullOrEmpty())
            return null;

        ulong? userId = null;
        ulong? sessionId = null;
        UserState? userState = null;
        List<UserRole> userRoles = null;
        foreach (var claim in claimsPrincipal.Claims)
        {
            switch (claim.Type)
            {
                case ClaimTypes.NameIdentifier:
                    userId = ulong.Parse(claim.Value);
                    break;
                case AuthConstants.SessionIdClaimKey:
                    sessionId = ulong.Parse(claim.Value);
                    break;
                case AuthConstants.UserStateClaimKey:
                    userState = claim.Value.ToEnum<UserState>();
                    break;
                case ClaimTypes.Role:
                    userRoles ??= new();
                    userRoles.Add(claim.Value.ToEnum<UserRole>());
                    break;
            }
        }
        var requestingUser = new RequestingUser(userId, sessionId, userState, userRoles);
        return requestingUser;
    }

    public static List<Claim> ToClaims(this RequestingUser requestingUser)
    {
        if (requestingUser == null)
            return null;

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, requestingUser.Id.Value.ToString()),
            new Claim(AuthConstants.SessionIdClaimKey, requestingUser.SessionId.Value.ToString()),
            new Claim(AuthConstants.UserStateClaimKey, requestingUser.State.ToString()),
        };

        requestingUser.Roles.ForEach(role =>
            claims.Add(new Claim(ClaimTypes.Role, role.ToString())));

        return claims;
    }
}
