using FireplaceApi.Application.Auth;
using FireplaceApi.Application.Converters;
using FireplaceApi.Application.Dtos;
using FireplaceApi.Application.Tools;
using FireplaceApi.Application.Validators;
using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Identifiers;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Services;
using FireplaceApi.Domain.Tools;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Resolvers;

[ExtendObjectType(typeof(GraphQLQuery))]
public class CommunityQueryResolvers
{
    [AllowAnonymous]
    public async Task<QueryResultDto<CommunityDto>> GetCommunitiesAsync(
        [Service(ServiceKind.Resolver)] CommunityService communityService,
        [Service(ServiceKind.Resolver)] CommunityValidator communityValidator,
        [GraphQLNonNullType] string search, CommunitySortType? sort = null)
    {
        var queryResult = await communityService.ListCommunitiesAsync(search, sort);
        var queryResultDto = queryResult.ToDto();
        return queryResultDto;
    }

    [AllowAnonymous]
    public async Task<CommunityDto> GetCommunityAsync(
        [Service(ServiceKind.Resolver)] CommunityService communityService,
        [Service(ServiceKind.Resolver)] CommunityValidator communityValidator,
        [GraphQLNonNullType] string idOrName)
    {
        var communityIdentifier = communityValidator.ValidateEncodedIdOrName(idOrName);
        var community = await communityService.GetCommunityByIdentifierAsync(communityIdentifier);
        var communityDto = community.ToDto();
        return communityDto;
    }
}

[ExtendObjectType(typeof(PostDto))]
public class PostCommunityQueryResolvers
{
    [AllowAnonymous]
    public async Task<CommunityDto> GetCommunityAsync(
        [Service(ServiceKind.Resolver)] CommunityService communityService,
        [Service(ServiceKind.Resolver)] CommunityValidator communityValidator,
        [User] RequestingUser requestingUser, [Parent] PostDto post)
    {
        var communityIdentifier = CommunityIdentifier.OfId(post.CommunityId.IdDecode());
        var community = await communityService.GetCommunityByIdentifierAsync(communityIdentifier);
        var communityDto = community.ToDto();
        return communityDto;
    }
}

[ExtendObjectType(typeof(UserDto))]
public class UserCommunitiesQueryResolvers
{
    [AllowAnonymous]
    public async Task<QueryResultDto<CommunityDto>> GetJoinedCommunitiesAsync(
        [Service(ServiceKind.Resolver)] CommunityService communityService,
        [Service(ServiceKind.Resolver)] CommunityValidator communityValidator,
        [User] RequestingUser requestingUser, [Parent] UserDto user,
        CommunitySortType? sort = null)
    {
        var queryResult = await communityService.ListJoinedCommunitiesAsync(requestingUser.Id.Value, sort);
        var queryResultDto = queryResult.ToDto();
        return queryResultDto;
    }
}
