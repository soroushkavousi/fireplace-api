using FireplaceApi.Domain.Communities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Communities;

public class DeleteCommunityCommandHandler : RequestHandler<DeleteCommunityCommand, Unit>
{
    private readonly ICommunityRepository _communityRepository;
    private Community _community;

    public DeleteCommunityCommandHandler(ICommunityRepository communityRepository)
    {
        _communityRepository = communityRepository;
    }

    protected override async Task ValidateAsync(DeleteCommunityCommand command, CancellationToken cancellationToken)
    {
        _community = await _communityRepository.GetCommunityAsync(command.CommunityIdentifier, false);
        if (_community == null)
            throw new CommunityNotExistException(command.CommunityIdentifier);

        if (command.UserId != _community.CreatorId)
            throw new CommunityAccessDeniedException(command.UserId, _community.Id);
    }

    protected override async Task<Unit> OperateAsync(DeleteCommunityCommand command, CancellationToken cancellationToken)
    {
        await _communityRepository.DeleteCommunityAsync(command.CommunityIdentifier);
        return Unit.Value;
    }
}
