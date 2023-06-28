using FireplaceApi.Domain.Communities;
using System.Threading;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Communities;

public class SearchCommunitiesQueryHandler : RequestHandler<SearchCommunitiesQuery, QueryResult<Community>>
{
    private readonly ICommunityRepository _communityRepository;

    public SearchCommunitiesQueryHandler(ICommunityRepository communityRepository)
    {
        _communityRepository = communityRepository;
    }

    protected override async Task ValidateAsync(SearchCommunitiesQuery query, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }

    protected override async Task<QueryResult<Community>> OperateAsync(SearchCommunitiesQuery query, CancellationToken cancellationToken)
    {
        var sort = query.Sort ?? CommunitySortType.CREATION;
        var communities = await _communityRepository.ListCommunitiesAsync(query.Search, sort);
        var queryResult = new QueryResult<Community>(communities);
        return queryResult;
    }
}
