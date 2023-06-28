using FireplaceApi.Domain.Communities;
using FireplaceApi.Domain.Errors;
using MediatR;

namespace FireplaceApi.Application.Communities;

public class PatchCommunityCommand : IRequest<Community>
{
    public ulong UserId { get; init; }
    public CommunityIdentifier CommunityIdentifier { get; init; }
    public CommunityName NewCommunityName { get; init; }

    public PatchCommunityCommand(ulong userId, CommunityIdentifier communityIdentifier,
        CommunityName newCommunityName)
    {
        UserId = userId;
        CommunityIdentifier = communityIdentifier ?? throw new CommunityIdOrNameMissingFieldException();
        NewCommunityName = newCommunityName ?? throw new CommunityNameMissingFieldException();
    }
}
