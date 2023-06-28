using FireplaceApi.Domain.Communities;
using MediatR;

namespace FireplaceApi.Application.Communities;

public class DeleteCommunityCommand : IRequest<Unit>
{
    public ulong UserId { get; init; }
    public CommunityIdentifier CommunityIdentifier { get; init; }

    public DeleteCommunityCommand(ulong userId, CommunityIdentifier communityIdentifier)
    {
        UserId = userId;
        CommunityIdentifier = communityIdentifier ?? throw new CommunityIdOrNameMissingFieldException();
    }
}
