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

namespace FireplaceApi.Application.Resolvers
{
    [ExtendObjectType(typeof(CommunityDto))]
    public class CommunityPostQueryResolvers
    {
        [AllowAnonymous]
        public async Task<QueryResultDto<PostDto>> GetPostsAsync(
            [Service(ServiceKind.Resolver)] PostService postService,
            [Service(ServiceKind.Resolver)] PostValidator postValidator,
            [Service] PostConverter postConverter,
            [User] User requestingUser,
            [Parent] CommunityDto community,
            SortType? sort = null)
        {
            var communityIdentifier = CommunityIdentifier.OfId(community.Id.IdDecode());
            var queryResult = await postService.ListCommunityPostsAsync(communityIdentifier, sort, requestingUser);
            var queryResultDto = postConverter.ConvertToDto(queryResult);
            return queryResultDto;
        }
    }

    [ExtendObjectType(typeof(GraphQLQuery))]
    public class PostQueryResolvers
    {
        [AllowAnonymous]
        public async Task<QueryResultDto<PostDto>> GetPostsAsync(
            [Service(ServiceKind.Resolver)] PostService postService,
            [Service(ServiceKind.Resolver)] PostValidator postValidator,
            [Service] PostConverter postConverter,
            [User] User requestingUser,
            [GraphQLNonNullType] string search, SortType? sort = null)
        {
            var queryResult = await postService.ListPostsAsync(search, sort, requestingUser);
            var queryResultDto = postConverter.ConvertToDto(queryResult);
            return queryResultDto;
        }

        [AllowAnonymous]
        public async Task<PostDto> GetPostAsync(
            [Service(ServiceKind.Resolver)] PostService postService,
            [Service(ServiceKind.Resolver)] PostValidator postValidator,
            [Service] PostConverter postConverter,
            [User] User requestingUser,
            [GraphQLNonNullType] string id)
        {
            var ulongId = postValidator.ValidateEncodedIdFormat(id, FieldName.POST_ID).Value;
            var post = await postService.GetPostByIdAsync(ulongId, false, false, requestingUser);
            var postDto = postConverter.ConvertToDto(post);
            return postDto;
        }
    }
}
