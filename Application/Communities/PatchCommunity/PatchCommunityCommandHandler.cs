using FireplaceApi.Domain.Communities;
using System.Threading;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Communities;

public class PatchCommunityCommandHandler : RequestHandler<PatchCommunityCommand, Community>
{
    private readonly ICommunityRepository _communityRepository;
    private Community _community;

    public PatchCommunityCommandHandler(ICommunityRepository communityRepository)
    {
        _communityRepository = communityRepository;
    }

    protected override async Task ValidateAsync(PatchCommunityCommand command, CancellationToken cancellationToken)
    {
        var newCommunityNameIdentifier = CommunityIdentifier.OfName(command.NewCommunityName);
        if (await _communityRepository.DoesCommunityExistAsync(newCommunityNameIdentifier))
            throw new CommunityAlreadyExistsException(newCommunityNameIdentifier);

        _community = await _communityRepository.GetCommunityAsync(command.CommunityIdentifier, false);
        if (_community == null)
            throw new CommunityNotExistException(command.CommunityIdentifier);

        if (command.UserId != _community.CreatorId)
            throw new CommunityAccessDeniedException(command.UserId, _community.Id);
    }

    protected override async Task<Community> OperateAsync(PatchCommunityCommand command, CancellationToken cancellationToken)
    {
        _community.Name = command.NewCommunityName;
        await _communityRepository.UpdateCommunityNameAsync(_community.Id, command.NewCommunityName);
        return _community;
    }
}
