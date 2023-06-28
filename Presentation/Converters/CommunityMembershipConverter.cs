using FireplaceApi.Domain.Communities;
using FireplaceApi.Presentation.Dtos;

namespace FireplaceApi.Presentation.Converters;

public static class CommunityMembershipConverter
{
    public static CommunityMembershipDto ToDto(this CommunityMembership communityMembership)
    {
        if (communityMembership == null)
            return null;

        var communityMembershipDto = new CommunityMembershipDto(communityMembership.Id.IdEncode(),
            communityMembership.UserId.IdEncode(), communityMembership.Username.Value,
            communityMembership.CommunityId.IdEncode(), communityMembership.CommunityName.Value,
            communityMembership.CreationDate);

        return communityMembershipDto;
    }

    public static QueryResultDto<CommunityMembershipDto> ToDto(this QueryResult<CommunityMembership> queryResult)
        => queryResult.ToDto(ToDto);
}
