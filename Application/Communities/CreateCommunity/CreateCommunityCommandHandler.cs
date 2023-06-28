using FireplaceApi.Application.Users;
using FireplaceApi.Domain.Communities;
using System.Threading;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Communities;

public class CreateCommunityCommandHandler : RequestHandler<CreateCommunityCommand, Community>
{
    private readonly ICommunityRepository _communityRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICommunityMembershipRepository _communityMembershipRepository;

    public CreateCommunityCommandHandler(
        ICommunityRepository communityRepository,
        IUserRepository userRepository,
        ICommunityMembershipRepository communityMembershipRepository)
    {
        _communityRepository = communityRepository;
        _userRepository = userRepository;
        _communityMembershipRepository = communityMembershipRepository;
    }

    protected override async Task ValidateAsync(CreateCommunityCommand command, CancellationToken cancellationToken)
    {
        var communityIdentifier = CommunityIdentifier.OfName(command.CommunityName);
        if (await _communityRepository.DoesCommunityExistAsync(communityIdentifier))
            throw new CommunityAlreadyExistsException(communityIdentifier);
    }

    protected override async Task<Community> OperateAsync(CreateCommunityCommand command, CancellationToken cancellationToken)
    {
        var username = command.Username;
        username ??= await _userRepository.GetUsernameByIdAsync(command.UserId);

        var community = await _communityRepository.CreateCommunityAsync(
            command.CommunityName, command.UserId, username);

        // Create community membership
        var membership = await _communityMembershipRepository
            .CreateCommunityMembershipAsync(command.UserId, username,
                community.Id, command.CommunityName);

        return community;
    }
}
