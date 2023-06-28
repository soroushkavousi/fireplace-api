using FireplaceApi.Application.Communities;
using FireplaceApi.Domain.Communities;
using FireplaceApi.Domain.Errors;
using FireplaceApi.Presentation.Auth;
using FireplaceApi.Presentation.Converters;
using FireplaceApi.Presentation.Dtos;
using FireplaceApi.Presentation.ValueObjects;
using HotChocolate;
using HotChocolate.Types;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace FireplaceApi.Presentation.GraphQL.Resolvers.Queries;

[ExtendObjectType(typeof(GraphQLQuery))]
public class CommunityQueryResolvers
{
    [AllowAnonymous]
    public async Task<QueryResultDto<CommunityDto>> SearchCommunitiesAsync(
        [Service(ServiceKind.Resolver)] ISender sender,
        [GraphQLNonNullType] string search, CommunitySortType? sort = null)
    {
        var query = new SearchCommunitiesQuery(search, sort);
        var queryResult = await sender.Send(query);
        var queryResultDto = queryResult.ToDto();
        return queryResultDto;
    }

    [AllowAnonymous]
    public async Task<CommunityDto> GetCommunityAsync(
        [Service(ServiceKind.Resolver)] ISender sender,
        [GraphQLNonNullType] string idOrName)
    {
        var communityIdentifier = idOrName.ToCommunityIdentifier();
        var query = new GetCommunityByIdOrNameQuery(communityIdentifier);
        var community = await sender.Send(query);
        var communityDto = community.ToDto();
        return communityDto;
    }
}

[ExtendObjectType(typeof(PostDto))]
public class PostCommunityQueryResolvers
{
    [AllowAnonymous]
    public async Task<CommunityDto> GetCommunityAsync(
        [Service(ServiceKind.Resolver)] ISender sender,
        [User] RequestingUser requestingUser, [Parent] PostDto post)
    {
        var communityIdentifier = CommunityIdentifier.OfId(post.CommunityId.ToId(FieldName.COMMUNITY_ID));
        var query = new GetCommunityByIdOrNameQuery(communityIdentifier);
        var community = await sender.Send(query);
        var communityDto = community.ToDto();
        return communityDto;
    }
}

[ExtendObjectType(typeof(UserDto))]
public class UserCommunitiesQueryResolvers
{
    [AllowAnonymous]
    public async Task<QueryResultDto<CommunityDto>> GetJoinedCommunitiesAsync(
        [Service(ServiceKind.Resolver)] ISender sender,
        [User] RequestingUser requestingUser, [Parent] UserDto user,
        CommunitySortType? sort = null)
    {
        var query = new ListJoinedCommunitiesQuery(requestingUser.Id.Value, sort);
        var queryResult = await sender.Send(query);
        var queryResultDto = queryResult.ToDto();
        return queryResultDto;
    }
}
