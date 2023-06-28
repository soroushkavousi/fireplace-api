using FireplaceApi.Domain.Communities;
using FireplaceApi.Domain.Users;
using MediatR;

namespace FireplaceApi.Application.Communities;

public class JoinCommunityCommand : IRequest<CommunityMembership>
{
    public ulong UserId { get; init; }
    public CommunityIdentifier CommunityIdentifier { get; init; }
    public Username Username { get; init; }

    public JoinCommunityCommand(ulong userId, CommunityIdentifier communityIdentifier,
        Username username = null)
    {
        UserId = userId;
        CommunityIdentifier = communityIdentifier ?? throw new CommunityIdOrNameMissingFieldException();
        Username = username;
    }
}
