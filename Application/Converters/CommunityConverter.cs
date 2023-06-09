using FireplaceApi.Application.Dtos;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Tools;
using FireplaceApi.Domain.ValueObjects;
using System.Linq;

namespace FireplaceApi.Application.Converters;

public static class CommunityConverter
{
    public static CommunityDto ToDto(this Community community)
    {
        if (community == null)
            return null;

        QueryResultDto<PostDto> postDtos = null;
        if (community.Posts != null)
        {
            community.Posts.Items = community.Posts.Items
                .Select(comment => comment.PureCopy()).ToList();
            postDtos = community.Posts.ToDto(PostConverter.ToDto);
        }

        var communityDto = new CommunityDto(community.Id.IdEncode(), community.Name,
            community.CreatorId.IdEncode(), community.CreatorUsername,
            community.CreationDate, postDtos);

        return communityDto;
    }

    public static QueryResultDto<CommunityDto> ToDto(this QueryResult<Community> queryResult)
        => queryResult.ToDto(ToDto);
}
