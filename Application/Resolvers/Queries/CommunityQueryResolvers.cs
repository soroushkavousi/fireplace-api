using FireplaceApi.Application.Controllers;
using FireplaceApi.Application.Converters;
using FireplaceApi.Application.Tool;
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
        [Service] CommunityConverter communityConverter,
        [GraphQLNonNullType] string search, CommunitySortType? sort = null)
    {
        var queryResult = await communityService.ListCommunitiesAsync(search, sort);
        var queryResultDto = communityConverter.ConvertToDto(queryResult);
        return queryResultDto;
    }

    [AllowAnonymous]
    public async Task<CommunityDto> GetCommunityAsync(
        [Service(ServiceKind.Resolver)] CommunityService communityService,
        [Service(ServiceKind.Resolver)] CommunityValidator communityValidator,
        [Service] CommunityConverter communityConverter,
        [GraphQLNonNullType] string idOrName)
    {
        var communityIdentifier = communityValidator.ValidateEncodedIdOrName(idOrName);
        var community = await communityService.GetCommunityByIdentifierAsync(communityIdentifier);
        var communityDto = communityConverter.ConvertToDto(community);
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
        [Service] CommunityConverter communityConverter, [User] User requestingUser,
        [Parent] PostDto post)
    {
        var communityIdentifier = CommunityIdentifier.OfId(post.CommunityId.IdDecode());
        var community = await communityService.GetCommunityByIdentifierAsync(communityIdentifier);
        var communityDto = communityConverter.ConvertToDto(community);
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
        [Service] CommunityConverter communityConverter,
        [User] User requestingUser, [Parent] UserDto user,
        CommunitySortType? sort = null)
    {
        var queryResult = await communityService.ListJoinedCommunitiesAsync(requestingUser, sort);
        var queryResultDto = communityConverter.ConvertToDto(queryResult);
        return queryResultDto;
    }
}
