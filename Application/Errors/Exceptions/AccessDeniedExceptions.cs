using FireplaceApi.Domain.Errors;
using FireplaceApi.Domain.Users;
using System.Collections.Generic;

namespace FireplaceApi.Application.Errors;

public class CommunityAccessDeniedException : ApiException
{
    public CommunityAccessDeniedException(ulong userId, ulong communityId)
        : base(
            errorType: ApplicationErrorType.NOT_EXIST_OR_ACCESS_DENIED,
            errorField: FieldName.COMMUNITY,
            errorServerMessage: "The user does not have access to the community!",
            parameters: new { userId, communityId },
            systemException: null
        )
    { }
}

public class CommunityMembershipAccessDeniedException : ApiException
{
    public CommunityMembershipAccessDeniedException(ulong userId, ulong communityId)
        : base(
            errorType: ApplicationErrorType.NOT_EXIST_OR_ACCESS_DENIED,
            errorField: FieldName.COMMUNITY_MEMBERSHIP,
            errorServerMessage: "The user can not alter the community membership!",
            parameters: new { userId, communityId },
            systemException: null
        )
    { }
}

public class PostAccessDeniedException : ApiException
{
    public PostAccessDeniedException(ulong userId, ulong postId)
        : base(
            errorType: ApplicationErrorType.NOT_EXIST_OR_ACCESS_DENIED,
            errorField: FieldName.POST,
            errorServerMessage: "The user does not have access to the post!",
            parameters: new { userId, postId },
            systemException: null
        )
    { }
}

public class CommentAccessDeniedException : ApiException
{
    public CommentAccessDeniedException(ulong userId, ulong commentId)
        : base(
            errorType: ApplicationErrorType.NOT_EXIST_OR_ACCESS_DENIED,
            errorField: FieldName.COMMENT,
            errorServerMessage: "The user does not have access to the comment!",
            parameters: new { userId, commentId },
            systemException: null
        )
    { }
}

public class AccessTokenAccessDeniedException : ApiException
{
    public AccessTokenAccessDeniedException(ulong userId, string accessToken)
        : base(
            errorType: ApplicationErrorType.NOT_EXIST_OR_ACCESS_DENIED,
            errorField: FieldName.ACCESS_TOKEN,
            errorServerMessage: "The user does not have a valid access token!",
            parameters: new { userId, accessToken },
            systemException: null
        )
    { }

    public AccessTokenAccessDeniedException(string claimType, IEnumerable<string> allowedValues,
        string actualValue)
        : base(
            errorType: ApplicationErrorType.NOT_EXIST_OR_ACCESS_DENIED,
            errorField: FieldName.ACCESS_TOKEN,
            errorServerMessage: "The user does not have a valid access token!",
            parameters: new { claimType, allowedValues, actualValue },
            systemException: null
        )
    { }
}

public class SessionAccessDeniedException : ApiException
{
    public SessionAccessDeniedException(ulong requestinUserId, ulong sessionId)
        : base(
            errorType: ApplicationErrorType.NOT_EXIST_OR_ACCESS_DENIED,
            errorField: FieldName.SESSION,
            errorServerMessage: "The user does not have access to the session!",
            parameters: new { requestinUserId, sessionId },
            systemException: null
        )
    { }
}

public class ExpiredSessionAccessDeniedException : ApiException
{
    public ExpiredSessionAccessDeniedException(ulong sessionId)
        : base(
            errorType: ApplicationErrorType.NOT_EXIST_OR_ACCESS_DENIED,
            errorField: FieldName.EXPIRED_SESSION,
            errorServerMessage: "The session has been expired!",
            parameters: new { sessionId },
            systemException: null
        )
    { }
}

public class RevokedSessionAccessDeniedException : ApiException
{
    public RevokedSessionAccessDeniedException(ulong sessionId)
        : base(
            errorType: ApplicationErrorType.NOT_EXIST_OR_ACCESS_DENIED,
            errorField: FieldName.REVOKED_SESSION,
            errorServerMessage: "The session has been revoked!",
            parameters: new { sessionId },
            systemException: null
        )
    { }
}

public class RoleAccessDeniedException : ApiException
{
    public RoleAccessDeniedException(IEnumerable<string> allowedRules, ulong userId,
        List<UserRole> userRoles)
        : base(
            errorType: ApplicationErrorType.NOT_EXIST_OR_ACCESS_DENIED,
            errorField: FieldName.ROLE,
            errorServerMessage: "The user role is not enough to call this request!",
            parameters: new { allowedRules, userId, userRoles },
            systemException: null
        )
    { }
}

public class UnverifiedUserAccessDeniedException : ApiException
{
    public UnverifiedUserAccessDeniedException(IEnumerable<string> allowedUserStates,
        string actualValue)
        : base(
            errorType: ApplicationErrorType.NOT_EXIST_OR_ACCESS_DENIED,
            errorField: FieldName.USER_STATE,
            errorServerMessage: "The user is not verified!",
            parameters: new { allowedUserStates, actualValue },
            systemException: null
        )
    { }
}

public class CsrfTokenAccessDeniedException : ApiException
{
    public CsrfTokenAccessDeniedException(string headerCsrfToken, string cookieCsrfToken)
        : base(
            errorType: ApplicationErrorType.NOT_EXIST_OR_ACCESS_DENIED,
            errorField: FieldName.CSRF_TOKEN,
            errorServerMessage: "The csrf token in headers does not match!",
            parameters: new { headerCsrfToken, cookieCsrfToken },
            systemException: null
        )
    { }
}
