using FireplaceApi.Presentation.Dtos;
using FireplaceApi.Application.Models;
using FireplaceApi.Application.Tools;
using FireplaceApi.Application.ValueObjects;

namespace FireplaceApi.Presentation.Converters;

public static class CommunityMembershipConverter
{
    public static CommunityMembershipDto ToDto(this CommunityMembership communityMembership)
    {
        if (communityMembership == null)
            return null;

        var communityMembershipDto = new CommunityMembershipDto(communityMembership.Id.IdEncode(),
            communityMembership.UserId.IdEncode(), communityMembership.Username,
            communityMembership.CommunityId.IdEncode(), communityMembership.CommunityName,
            communityMembership.CreationDate);

        return communityMembershipDto;
    }

    public static QueryResultDto<CommunityMembershipDto> ToDto(this QueryResult<CommunityMembership> queryResult)
        => queryResult.ToDto(ToDto);
}
