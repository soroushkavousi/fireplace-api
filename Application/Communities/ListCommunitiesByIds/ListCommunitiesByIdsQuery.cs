using FireplaceApi.Domain.Communities;
using MediatR;
using System.Collections.Generic;

namespace FireplaceApi.Application.Communities;

public class ListCommunitiesByIdsQuery : IRequest<List<Community>>
{
    public List<ulong> Ids { get; init; }

    public ListCommunitiesByIdsQuery(List<ulong> ids)
    {
        Ids = ids ?? throw new ListOfIdsMissingFieldException();
    }
}
