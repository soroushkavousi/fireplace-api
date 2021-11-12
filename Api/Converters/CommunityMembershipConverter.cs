using FireplaceApi.Api.Controllers;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace FireplaceApi.Api.Converters
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

            UserDto userDto = null;
            if (communityMembership.User != null)
                userDto = _serviceProvider.GetService<UserConverter>()
                    .ConvertToDto(communityMembership.User.PureCopy());

            CommunityDto communityDto = null;
            if (communityMembership.Community != null)
                communityDto = _serviceProvider.GetService<CommunityConverter>()
                    .ConvertToDto(communityMembership.Community.PureCopy());

            var communityMembershipDto = new CommunityMembershipDto(communityMembership.Id.IdEncode(),
                communityMembership.UserId.IdEncode(), communityMembership.Username,
                communityMembership.CommunityId.IdEncode(), communityMembership.CommunityName,
                communityMembership.CreationDate, userDto, communityDto);

            return communityMembershipDto;
        }
    }
}
