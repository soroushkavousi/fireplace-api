using FireplaceApi.Domain.Communities;
using MediatR;

namespace FireplaceApi.Application.Communities;

public class LeaveCommunityCommand : IRequest<Unit>
{
    public ulong UserId { get; init; }
    public CommunityIdentifier CommunityIdentifier { get; init; }

    public LeaveCommunityCommand(ulong userId, CommunityIdentifier communityIdentifier)
    {
        UserId = userId;
        CommunityIdentifier = communityIdentifier ?? throw new CommunityIdOrNameMissingFieldException();
    }
}
