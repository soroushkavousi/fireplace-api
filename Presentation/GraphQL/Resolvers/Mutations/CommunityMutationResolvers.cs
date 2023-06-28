using FireplaceApi.Application.Communities;
using FireplaceApi.Domain.Communities;
using FireplaceApi.Presentation.Auth;
using FireplaceApi.Presentation.Converters;
using FireplaceApi.Presentation.Dtos;
using HotChocolate;
using HotChocolate.Types;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Presentation.GraphQL;

[ExtendObjectType(typeof(GraphQLMutation))]
public class CommunityMutationResolvers
{
    public async Task<CommunityDto> CreateCommunitiesAsync(
        [Service(ServiceKind.Resolver)] ISender sender,
        [Service] IServiceProvider serviceProvider,
        [User] RequestingUser requestingUser,
        [GraphQLNonNullType] CreateCommunityInput input)
    {
        var command = new CreateCommunityCommand(requestingUser.Id.Value, input.Name);
        var community = await sender.Send(command);
        var communityDto = community.ToDto();
        return communityDto;
    }
}

public class CreateCommunityInput
{
    public CommunityName Name { get; set; }
}
