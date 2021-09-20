using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FireplaceApi.Api.Extensions;
using FireplaceApi.Core.Models;
using FireplaceApi.Api.Controllers;
using FireplaceApi.Core.ValueObjects;

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

        public PageDto<CommunityDto> ConvertToDto(Page<Community> page)
        {
            if (page == null)
                return null;

            var paginationDto = ConvertToDto(page.Pagination);
            var itemDtos = page.Items.Select(community => ConvertToDto(community)).ToList();

            var pageDto = new PageDto<CommunityDto>(page.TotalItemsCount, itemDtos, paginationDto);
            return pageDto;
        }

        public PaginationDto ConvertToDto(Pagination pagination)
        {
            if (pagination == null)
                return null;

            var paginationDto = new PaginationDto(pagination.TotalPagesCount,
                "next-page-url", "previous-page-url",
                pagination.Cursor);

            return paginationDto;
        }
    }
}
