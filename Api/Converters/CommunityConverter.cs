using FireplaceApi.Api.Controllers;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace FireplaceApi.Api.Converters
{
    public class CommunityConverter : BaseConverter<Community, CommunityDto>
    {
        private readonly ILogger<CommunityConverter> _logger;
        private readonly IServiceProvider _serviceProvider;

        public CommunityConverter(ILogger<CommunityConverter> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public override CommunityDto ConvertToDto(Community community)
        {
            if (community == null)
                return null;

            UserDto creatorDto = null;
            if (community.Creator != null)
                creatorDto = _serviceProvider.GetService<UserConverter>()
                    .ConvertToDto(community.Creator.PureCopy());

            var communityDto = new CommunityDto(community.Id, community.Name, community.CreatorId,
                community.CreationDate, creatorDto);

            return communityDto;
        }
    }
}
