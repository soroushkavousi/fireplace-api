using FireplaceApi.Domain.Communities;
using FireplaceApi.Domain.Users;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Communities;

public class LeaveCommunityCommandHandler : RequestHandler<LeaveCommunityCommand, Unit>
{
    private readonly ICommunityRepository _communityRepository;
    private readonly ICommunityMembershipRepository _communityMembershipRepository;
    private Community _community;
    private CommunityMembershipIdentifier _communityMembershipIdentifier;

    public LeaveCommunityCommandHandler(
        ICommunityRepository communityRepository,
        ICommunityMembershipRepository communityMembershipRepository)
    {
        _communityRepository = communityRepository;
        _communityMembershipRepository = communityMembershipRepository;
    }

    protected override async Task ValidateAsync(LeaveCommunityCommand command, CancellationToken cancellationToken)
    {
        _community = await _communityRepository.GetCommunityAsync(command.CommunityIdentifier);
        if (_community == null)
            throw new CommunityNotExistException(command.CommunityIdentifier);

        if (_community.CreatorId == command.UserId)
            throw new CommunityMembershipAccessDeniedException(command.UserId, _community.Id);

        var userIdentifier = UserIdentifier.OfId(command.UserId);
        _communityMembershipIdentifier = CommunityMembershipIdentifier
            .OfUserAndCommunity(userIdentifier, command.CommunityIdentifier);
        if (!await _communityMembershipRepository.DoesCommunityMembershipIdentifierExistAsync(
                _communityMembershipIdentifier))
            throw new CommunityMembershipNotExistException(_communityMembershipIdentifier);
    }

    protected override async Task<Unit> OperateAsync(LeaveCommunityCommand command, CancellationToken cancellationToken)
    {
        await _communityMembershipRepository
            .DeleteCommunityMembershipByIdentifierAsync(_communityMembershipIdentifier);

        return Unit.Value;
    }
}
