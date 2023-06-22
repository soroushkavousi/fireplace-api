using FireplaceApi.Application.Enums;
using FireplaceApi.Application.Identifiers;

namespace FireplaceApi.Application.Exceptions;

public class CommunityNotExistException : ApiException
{
    public CommunityNotExistException(CommunityIdentifier communityIdentifier)
        : base(
            errorType: ErrorType.NOT_EXIST_OR_ACCESS_DENIED,
            errorField: FieldName.COMMUNITY,
            errorServerMessage: "The community does not exist!",
            parameters: new { communityIdentifier },
            systemException: null
        )
    { }
}

public class CommunityMembershipNotExistException : ApiException
{
    public CommunityMembershipNotExistException(CommunityMembershipIdentifier communityMembershipIdentifier)
        : base(
            errorType: ErrorType.NOT_EXIST_OR_ACCESS_DENIED,
            errorField: FieldName.COMMUNITY_MEMBERSHIP,
            errorServerMessage: "The community membership does not exist!",
            parameters: new { communityMembershipIdentifier },
            systemException: null
        )
    { }
}

public class PostNotExistException : ApiException
{
    public PostNotExistException(ulong postId)
        : base(
            errorType: ErrorType.NOT_EXIST_OR_ACCESS_DENIED,
            errorField: FieldName.POST,
            errorServerMessage: "The post does not exist!",
            parameters: new { postId },
            systemException: null
        )
    { }
}

public class PostVoteNotExistException : ApiException
{
    public PostVoteNotExistException(ulong userId, ulong postId)
        : base(
            errorType: ErrorType.NOT_EXIST_OR_ACCESS_DENIED,
            errorField: FieldName.POST_VOTE,
            errorServerMessage: "The post vote does not exist!",
            parameters: new { userId, postId },
            systemException: null
        )
    { }
}

public class CommentNotExistException : ApiException
{
    public CommentNotExistException(ulong commentId)
        : base(
            errorType: ErrorType.NOT_EXIST_OR_ACCESS_DENIED,
            errorField: FieldName.COMMENT,
            errorServerMessage: "The comment does not exist!",
            parameters: new { commentId },
            systemException: null
        )
    { }
}

public class CommentVoteNotExistException : ApiException
{
    public CommentVoteNotExistException(ulong userId, ulong commentId)
        : base(
            errorType: ErrorType.NOT_EXIST_OR_ACCESS_DENIED,
            errorField: FieldName.COMMENT_VOTE,
            errorServerMessage: "The comment vote does not exist!",
            parameters: new { userId, commentId },
            systemException: null
        )
    { }
}

public class UserNotExistException : ApiException
{
    public UserNotExistException(UserIdentifier userIdentifier)
        : base(
            errorType: ErrorType.NOT_EXIST_OR_ACCESS_DENIED,
            errorField: FieldName.USER,
            errorServerMessage: "The user does not exist!",
            parameters: new { userIdentifier },
            systemException: null
        )
    { }
}

public class UsernameNotExistException : ApiException
{
    public UsernameNotExistException(string username)
        : base(
            errorType: ErrorType.NOT_EXIST_OR_ACCESS_DENIED,
            errorField: FieldName.USERNAME,
            errorServerMessage: "The username does not exist!",
            parameters: new { username },
            systemException: null
        )
    { }
}

public class EmailNotExistException : ApiException
{
    public EmailNotExistException(EmailIdentifier emailIdentifier)
        : base(
            errorType: ErrorType.NOT_EXIST_OR_ACCESS_DENIED,
            errorField: FieldName.EMAIL,
            errorServerMessage: "The email does not exist!",
            parameters: new { emailIdentifier },
            systemException: null
        )
    { }
}

public class AccessTokenNotExistException : ApiException
{
    public AccessTokenNotExistException(string accessToken,
        string serverMessage = "The accessToken does not exist!")
        : base(
            errorType: ErrorType.NOT_EXIST_OR_ACCESS_DENIED,
            errorField: FieldName.ACCESS_TOKEN,
            errorServerMessage: serverMessage,
            parameters: new { accessToken },
            systemException: null
        )
    { }
}

public class SessionNotExistException : ApiException
{
    public SessionNotExistException(ulong sessionId)
        : base(
            errorType: ErrorType.NOT_EXIST_OR_ACCESS_DENIED,
            errorField: FieldName.SESSION,
            errorServerMessage: "The session does not exist!",
            parameters: new { sessionId },
            systemException: null
        )
    { }
}

public class ErrorNotExistException : ApiException
{
    public ErrorNotExistException(ErrorIdentifier errorIdentifier)
        : base(
            errorType: ErrorType.NOT_EXIST_OR_ACCESS_DENIED,
            errorField: FieldName.ERROR,
            errorServerMessage: "The error does not exist!",
            parameters: new { errorIdentifier },
            systemException: null
        )
    { }
}

public class PasswordNotExistException : ApiException
{
    public PasswordNotExistException(ulong userId)
        : base(
            errorType: ErrorType.NOT_EXIST_OR_ACCESS_DENIED,
            errorField: FieldName.PASSWORD,
            errorServerMessage: "The user doesn't have a password!",
            parameters: new { userId },
            systemException: null
        )
    { }
}
