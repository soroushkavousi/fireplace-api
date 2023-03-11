using FireplaceApi.Application.Controllers;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Tools;
using Microsoft.Extensions.Logging;
using System;

namespace FireplaceApi.Application.Converters
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

            var communityDto = new CommunityDto(community.Id.IdEncode(), community.Name,
                community.CreatorId.IdEncode(), community.CreatorUsername,
                community.CreationDate);

            return communityDto;
        }
    }
}
