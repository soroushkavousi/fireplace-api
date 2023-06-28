using FireplaceApi.Domain.Communities;
using FireplaceApi.Domain.Users;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Communities;

public class ListJoinedCommunitiesQueryHandler : RequestHandler<ListJoinedCommunitiesQuery, QueryResult<Community>>
{
    private readonly ICommunityMembershipRepository _communityMembershipRepository;

    public ListJoinedCommunitiesQueryHandler(ICommunityMembershipRepository communityMembershipRepository)
    {
        _communityMembershipRepository = communityMembershipRepository;
    }

    protected override async Task ValidateAsync(ListJoinedCommunitiesQuery query, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }

    protected override async Task<QueryResult<Community>> OperateAsync(ListJoinedCommunitiesQuery query, CancellationToken cancellationToken)
    {
        var sort = query.Sort ?? CommunitySortType.CREATION;
        var communityMemberships = await _communityMembershipRepository.SearchCommunityMembershipsAsync(
            userIdentifier: UserIdentifier.OfId(query.UserId), includeCommunity: true);
        var communities = communityMemberships.Select(cm => cm.Community).ToList();
        var queryResult = new QueryResult<Community>(communities);
        return queryResult;
    }
}
