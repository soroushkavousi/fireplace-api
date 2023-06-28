using FireplaceApi.Domain.Communities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Communities;

public class ListCommunitiesByIdsQueryHandler : RequestHandler<ListCommunitiesByIdsQuery, List<Community>>
{
    private readonly ICommunityRepository _communityRepository;

    public ListCommunitiesByIdsQueryHandler(ICommunityRepository communityRepository)
    {
        _communityRepository = communityRepository;
    }

    protected override async Task ValidateAsync(ListCommunitiesByIdsQuery query, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }

    protected override async Task<List<Community>> OperateAsync(ListCommunitiesByIdsQuery query, CancellationToken cancellationToken)
    {
        if (query.Ids.IsNullOrEmpty())
            return null;

        var communities = await _communityRepository
            .ListCommunitiesByIdsAsync(query.Ids);
        return communities;
    }
}
