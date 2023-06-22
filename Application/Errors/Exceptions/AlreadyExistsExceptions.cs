using FireplaceApi.Domain.Communities;
using FireplaceApi.Domain.Emails;
using FireplaceApi.Domain.Errors;
using FireplaceApi.Domain.Users;

namespace FireplaceApi.Application.Errors;

public class CommunityAlreadyExistsException : ApiException
{
    public CommunityAlreadyExistsException(CommunityIdentifier communityIdentifier)
        : base(
            errorType: ApplicationErrorType.ALREADY_EXISTS,
            errorField: FieldName.COMMUNITY,
            errorServerMessage: "The community already exists!",
            parameters: new { communityIdentifier },
            systemException: null
        )
    { }
}

public class CommunityMembershipAlreadyExistsException : ApiException
{
    public CommunityMembershipAlreadyExistsException(CommunityMembershipIdentifier communityMembershipIdentifier)
        : base(
            errorType: ApplicationErrorType.ALREADY_EXISTS,
            errorField: FieldName.COMMUNITY_MEMBERSHIP,
            errorServerMessage: "The community membership already exists!",
            parameters: new { communityMembershipIdentifier },
            systemException: null
        )
    { }
}

public class PostVoteAlreadyExistsException : ApiException
{
    public PostVoteAlreadyExistsException(ulong requestinUserId, ulong postId)
        : base(
            errorType: ApplicationErrorType.ALREADY_EXISTS,
            errorField: FieldName.POST_VOTE,
            errorServerMessage: "The post vote already exists!",
            parameters: new { requestinUserId, postId },
            systemException: null
        )
    { }
}

public class CommentVoteAlreadyExistsException : ApiException
{
    public CommentVoteAlreadyExistsException(ulong requestinUserId, ulong commentId)
        : base(
            errorType: ApplicationErrorType.ALREADY_EXISTS,
            errorField: FieldName.COMMENT_VOTE,
            errorServerMessage: "The comment vote already exists!",
            parameters: new { requestinUserId, commentId },
            systemException: null
        )
    { }
}

public class UserAlreadyExistsException : ApiException
{
    public UserAlreadyExistsException(UserIdentifier userIdentifier)
        : base(
            errorType: ApplicationErrorType.ALREADY_EXISTS,
            errorField: FieldName.USER,
            errorServerMessage: "The user already exists!",
            parameters: new { userIdentifier },
            systemException: null
        )
    { }
}

public class EmailAlreadyExistsException : ApiException
{
    public EmailAlreadyExistsException(EmailIdentifier emailIdentifier)
        : base(
            errorType: ApplicationErrorType.ALREADY_EXISTS,
            errorField: FieldName.EMAIL,
            errorServerMessage: "The email already exists!",
            parameters: new { emailIdentifier },
            systemException: null
        )
    { }
}

public class PasswordAlreadyExistException : ApiException
{
    public PasswordAlreadyExistException(ulong userId, string passwordHash)
        : base(
            errorType: ApplicationErrorType.ALREADY_EXISTS,
            errorField: FieldName.PASSWORD,
            errorServerMessage: "The user already has a password!",
            parameters: new { userId, passwordHash },
            systemException: null
        )
    { }
}
