using FireplaceApi.Domain.Enums;

namespace FireplaceApi.Domain.Exceptions;

public class CommunityAccessDeniedException : ApiException
{
    public CommunityAccessDeniedException(ulong requestingUserId, ulong communityId)
        : base(
            errorType: ErrorType.NOT_EXIST_OR_ACCESS_DENIED,
            errorField: FieldName.COMMUNITY,
            errorServerMessage: "The user does not have access to the community!",
            parameters: new { requestingUserId, communityId },
            systemException: null
        )
    { }
}

public class CommunityMembershipAccessDeniedException : ApiException
{
    public CommunityMembershipAccessDeniedException(ulong requestingUserId, ulong communityId)
        : base(
            errorType: ErrorType.NOT_EXIST_OR_ACCESS_DENIED,
            errorField: FieldName.COMMUNITY_MEMBERSHIP,
            errorServerMessage: "The user can not alter the community membership!",
            parameters: new { requestingUserId, communityId },
            systemException: null
        )
    { }
}

public class PostAccessDeniedException : ApiException
{
    public PostAccessDeniedException(ulong requestingUserId, ulong postId)
        : base(
            errorType: ErrorType.NOT_EXIST_OR_ACCESS_DENIED,
            errorField: FieldName.POST,
            errorServerMessage: "The user does not have access to the post!",
            parameters: new { requestingUserId, postId },
            systemException: null
        )
    { }
}

public class CommentAccessDeniedException : ApiException
{
    public CommentAccessDeniedException(ulong requestingUserId, ulong commentId)
        : base(
            errorType: ErrorType.NOT_EXIST_OR_ACCESS_DENIED,
            errorField: FieldName.COMMENT,
            errorServerMessage: "The user does not have access to the comment!",
            parameters: new { requestingUserId, commentId },
            systemException: null
        )
    { }
}

public class AccessTokenAccessDeniedException : ApiException
{
    public AccessTokenAccessDeniedException(ulong requestingUserId, string accessToken)
        : base(
            errorType: ErrorType.NOT_EXIST_OR_ACCESS_DENIED,
            errorField: FieldName.ACCESS_TOKEN,
            errorServerMessage: "The user does not have access to the access token!",
            parameters: new { requestingUserId, accessToken },
            systemException: null
        )
    { }
}

public class SessionAccessDeniedException : ApiException
{
    public SessionAccessDeniedException(ulong requestingUserId, ulong sessionId)
        : base(
            errorType: ErrorType.NOT_EXIST_OR_ACCESS_DENIED,
            errorField: FieldName.SESSION,
            errorServerMessage: "The user does not have access to the session!",
            parameters: new { requestingUserId, sessionId },
            systemException: null
        )
    { }
}
