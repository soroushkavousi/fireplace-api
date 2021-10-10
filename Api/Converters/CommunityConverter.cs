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
    public class CommunityConverter
    {
        private readonly ILogger<CommunityConverter> _logger;
        private readonly IServiceProvider _serviceProvider;

        public CommunityConverter(ILogger<CommunityConverter> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public CommunityDto ConvertToDto(Community community)
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

        public PageDto<CommunityDto> ConvertToDto(Page<Community> page, string listRelativePath)
        {
            if (page == null)
                return null;

            var listPath = $"{GlobalOperator.GlobalValues.Api.BaseUrlPath}{listRelativePath}";

            var paginationDto = new PaginationDto(page.QueryResultPointer,
                listPath, page.Number, page.Start, page.End, page.Limit,
                page.TotalItemsCount, page.TotalPagesCount);

            var itemDtos = page.Items.Select(community => ConvertToDto(community)).ToList();

            var pageDto = new PageDto<CommunityDto>(itemDtos, paginationDto);
            return pageDto;
        }
    }
}
