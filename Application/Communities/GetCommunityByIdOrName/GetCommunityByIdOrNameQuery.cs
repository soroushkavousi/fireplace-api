using FireplaceApi.Domain.Communities;
using FireplaceApi.Domain.Errors;
using MediatR;

namespace FireplaceApi.Application.Communities;

public class GetCommunityByIdOrNameQuery : IRequest<Community>
{
    public CommunityIdentifier Identifier { get; init; }

    public GetCommunityByIdOrNameQuery(CommunityIdentifier identifier)
    {
        Identifier = identifier ?? throw new CommunityIdOrNameMissingFieldException();
    }
}
