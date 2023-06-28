using FireplaceApi.Application.Users;
using FireplaceApi.Domain.Communities;
using FireplaceApi.Domain.Users;
using System.Threading;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Communities;

public class JoinCommunityCommandHandler : RequestHandler<JoinCommunityCommand, CommunityMembership>
{
    private readonly ICommunityRepository _communityRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICommunityMembershipRepository _communityMembershipRepository;
    private Community _community;

    public JoinCommunityCommandHandler(
        ICommunityRepository communityRepository,
        IUserRepository userRepository,
        ICommunityMembershipRepository communityMembershipRepository)
    {
        _communityRepository = communityRepository;
        _userRepository = userRepository;
        _communityMembershipRepository = communityMembershipRepository;
    }

    protected override async Task ValidateAsync(JoinCommunityCommand command, CancellationToken cancellationToken)
    {
        _community = await _communityRepository.GetCommunityAsync(command.CommunityIdentifier);
        if (_community == null)
            throw new CommunityNotExistException(command.CommunityIdentifier);

        var userIdentifier = UserIdentifier.OfId(command.UserId);
        var communityMembershipIdentifier = CommunityMembershipIdentifier
            .OfUserAndCommunity(userIdentifier, command.CommunityIdentifier);
        if (await _communityMembershipRepository.DoesCommunityMembershipIdentifierExistAsync(
                communityMembershipIdentifier))
            throw new CommunityMembershipAlreadyExistsException(communityMembershipIdentifier);
    }

    protected override async Task<CommunityMembership> OperateAsync(JoinCommunityCommand command, CancellationToken cancellationToken)
    {
        var username = command.Username;
        username ??= await _userRepository.GetUsernameByIdAsync(command.UserId);

        var communityMembership = await _communityMembershipRepository
            .CreateCommunityMembershipAsync(command.UserId,
                username, _community.Id, _community.Name);

        return communityMembership;
    }
}
