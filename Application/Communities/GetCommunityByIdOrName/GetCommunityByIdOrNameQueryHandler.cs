using FireplaceApi.Domain.Communities;
using System.Threading;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Communities;

public class GetCommunityByIdOrNameQueryHandler : RequestHandler<GetCommunityByIdOrNameQuery, Community>
{
    private readonly ICommunityRepository _communityRepository;
    private Community _community;

    public GetCommunityByIdOrNameQueryHandler(ICommunityRepository communityRepository)
    {
        _communityRepository = communityRepository;
    }

    protected override async Task ValidateAsync(GetCommunityByIdOrNameQuery query, CancellationToken cancellationToken)
    {
        _community = await _communityRepository.GetCommunityAsync(query.Identifier, false);
        if (_community == null)
            throw new CommunityNotExistException(query.Identifier);
    }

    protected override Task<Community> OperateAsync(GetCommunityByIdOrNameQuery query, CancellationToken cancellationToken)
    {
        return Task.FromResult(_community);
    }
}
