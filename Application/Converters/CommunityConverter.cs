using FireplaceApi.Application.Controllers;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

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

            QueryResultDto<PostDto> postDtos = null;
            if (community.Posts != null)
            {
                community.Posts.Items = community.Posts.Items
                    .Select(comment => comment.PureCopy()).ToList();
                postDtos = _serviceProvider.GetService<PostConverter>()
                    .ConvertToDto(community.Posts);
            }

            var communityDto = new CommunityDto(community.Id.IdEncode(), community.Name,
                community.CreatorId.IdEncode(), community.CreatorUsername,
                community.CreationDate, postDtos);

            return communityDto;
        }
    }
}
