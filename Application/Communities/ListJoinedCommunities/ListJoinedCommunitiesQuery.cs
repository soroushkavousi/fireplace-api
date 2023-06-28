using FireplaceApi.Domain.Communities;
using MediatR;

namespace FireplaceApi.Application.Communities;

public class ListJoinedCommunitiesQuery : IRequest<QueryResult<Community>>
{
    public ulong UserId { get; init; }
    public CommunitySortType? Sort { get; init; }

    public ListJoinedCommunitiesQuery(ulong userId, CommunitySortType? sort)
    {
        UserId = userId;
        Sort = sort;
    }
}
