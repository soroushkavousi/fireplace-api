using FireplaceApi.Domain.Communities;
using FireplaceApi.Presentation.Dtos;
using System.Linq;

namespace FireplaceApi.Presentation.Converters;

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

        var communityDto = new CommunityDto(community.Id.IdEncode(), community.Name.Value,
            community.CreatorId.IdEncode(), community.CreatorUsername.Value,
            community.CreationDate, postDtos);

        return communityDto;
    }

    public static QueryResultDto<CommunityDto> ToDto(this QueryResult<Community> queryResult)
        => queryResult.ToDto(ToDto);
}
