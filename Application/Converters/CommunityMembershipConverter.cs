using FireplaceApi.Application.Controllers;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Tools;
using Microsoft.Extensions.Logging;
using System;

namespace FireplaceApi.Application.Converters
{
    public class CommunityMembershipConverter : BaseConverter<CommunityMembership, CommunityMembershipDto>
    {
        private readonly ILogger<CommunityMembershipConverter> _logger;
        private readonly IServiceProvider _serviceProvider;

        public CommunityMembershipConverter(ILogger<CommunityMembershipConverter> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public override CommunityMembershipDto ConvertToDto(CommunityMembership communityMembership)
        {
            if (communityMembership == null)
                return null;

            var communityMembershipDto = new CommunityMembershipDto(communityMembership.Id.IdEncode(),
                communityMembership.UserId.IdEncode(), communityMembership.Username,
                communityMembership.CommunityId.IdEncode(), communityMembership.CommunityName,
                communityMembership.CreationDate);

            return communityMembershipDto;
        }
    }
}
