using FireplaceApi.Domain.Communities;
using FireplaceApi.Domain.Errors;
using MediatR;

namespace FireplaceApi.Application.Communities;

public record SearchCommunitiesQuery : IRequest<QueryResult<Community>>
{
    public string Search { get; init; }
    public CommunitySortType? Sort { get; init; }

    public SearchCommunitiesQuery(string search, CommunitySortType? sort)
    {
        Search = search ?? throw new SearchMissingFieldException();
        Sort = sort;
    }
}
