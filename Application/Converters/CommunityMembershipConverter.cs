using FireplaceApi.Application.Controllers;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Tools;
using FireplaceApi.Domain.ValueObjects;

namespace FireplaceApi.Application.Converters;

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
