using FireplaceApi.Domain.Communities;
using FireplaceApi.Domain.Errors;
using FireplaceApi.Domain.Users;
using MediatR;

namespace FireplaceApi.Application.Communities;

public class CreateCommunityCommand : IRequest<Community>
{
    public ulong UserId { get; init; }
    public CommunityName CommunityName { get; init; }
    public Username Username { get; init; }

    public CreateCommunityCommand(ulong userId, CommunityName communityName,
        Username username = null)
    {
        UserId = userId;
        CommunityName = communityName ?? throw new CommunityNameMissingFieldException();
        Username = username;
    }
}
