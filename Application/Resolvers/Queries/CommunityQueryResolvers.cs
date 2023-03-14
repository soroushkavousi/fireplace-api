﻿using FireplaceApi.Application.Controllers;
using FireplaceApi.Application.Converters;
using FireplaceApi.Application.Validators;
using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Services;
using HotChocolate;
using HotChocolate.Types;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Resolvers
{
    [ExtendObjectType(typeof(GraphQLQuery))]
    public class CommunityQueryResolvers
    {
        public async Task<QueryResultDto<CommunityDto>> GetCommunitiesAsync(
            [Service(ServiceKind.Resolver)] CommunityService communityService,
            [Service(ServiceKind.Resolver)] CommunityValidator communityValidator,
            [Service] CommunityConverter communityConverter,
            string name, CommunitySortType? sort = null)
        {
            var sortType = sort.HasValue ? (SortType)sort.Value : (SortType?)null;
            var queryResult = await communityService.ListCommunitiesAsync(name, sortType);
            var queryResultDto = communityConverter.ConvertToDto(queryResult);
            return queryResultDto;
        }

        public async Task<CommunityDto> GetCommunityAsync(
            [Service(ServiceKind.Resolver)] CommunityService communityService,
            [Service(ServiceKind.Resolver)] CommunityValidator communityValidator,
            [Service] CommunityConverter communityConverter,
            string idOrName)
        {
            var communityIdentifier = communityValidator.ValidateEncodedIdOrName(idOrName);
            var community = await communityService.GetCommunityByIdentifierAsync(communityIdentifier);
            var communityDto = communityConverter.ConvertToDto(community);
            return communityDto;
        }
    }
}